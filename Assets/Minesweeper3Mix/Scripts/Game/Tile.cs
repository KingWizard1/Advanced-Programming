using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Minesweeper3Mix
{

    public class Tile : MonoBehaviour
    {

        public int x, y, z; // Coordinate in 3D array of grid

        public bool isMine = false;

        public bool isRevealed = false;

        public GameObject minePrefab, textPrefab;

        public Gradient textGradient;

        [Range(0, 1)]
        public float mineChance = 0.15f;

        // ----------------------------------------------------- //

        private Animator anim;
        private Collider col;
        private GameObject mine, text;

        // ----------------------------------------------------- //

        // Awake is good for getting references
        private void Awake()
        {

            anim = GetComponent<Animator>();
            col = GetComponent<Collider>();

        }

        // ----------------------------------------------------- //

        /// <summary>Spawns a given prefab as a child.</summary>
        private GameObject SpawnChild(GameObject prefab)
        {
            // Spawn and attach to self
            GameObject child = Instantiate(prefab, transform);

            // Center the child
            child.transform.localPosition = Vector3.zero;

            // Deactivate it for now
            child.SetActive(false);

            // Return
            return child;

        }

        // ----------------------------------------------------- //

        private void Start()
        {
            // Set whether or not we hold a mine
            isMine = Random.value < mineChance;

            if (isMine)
            {
                // Spawn mine as child
                mine = SpawnChild(minePrefab);

                name = "Tile+Mine";
            }
            else
            {
                // Spawn text as child - shows number of adjacent mines.
                text = SpawnChild(textPrefab);

                name = "Tile+Text";
            }
            
        }

        // ----------------------------------------------------- //

        private void OnMouseDown()
        {
            Reveal(Random.Range(1, 9));
        }

        // ----------------------------------------------------- //

        public void Reveal(int adjacentMines = 0)
        {
            // Set flag
            isRevealed = true;

            // Animate the cube to disappear and reveal whats underneath
            anim.SetTrigger("Reveal");

            // Disable the collider of the cube so the GO can be clicked-through
            col.enabled = false;

            // If we have a mine, activate it/make it appear.
            if (isMine)
                mine.SetActive(true);
            else
            {
                if (adjacentMines > 0)
                {

                    // Enable the text and set the text string
                    text.SetActive(true);
                    TextMeshPro tmp = text.GetComponent<TextMeshPro>();
                    tmp.text = adjacentMines.ToString();

                    // Set text color based on number.
                    float step = adjacentMines / 9f;            // 9 = max number of possible adjacent mines
                    tmp.color = textGradient.Evaluate(step);    // Select color based on number value 0.0 to 1.0

                }
            }

        }

        // ----------------------------------------------------- //

        // ----------------------------------------------------- //


    }

}