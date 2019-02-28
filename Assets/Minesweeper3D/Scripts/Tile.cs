using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Minesweeper3D
{

    public class Tile : MonoBehaviour
    {

        public int x, y, z;
        public bool isMine = false;
        public bool isRevealed = false;

        public GameObject minePrefab;
        public GameObject textPrefab;

        [Range(0.01f, 0.5f)]
        public float mineChance = 0.15f;

        // ------------------------------------------------- //

        private Animator anim;
        private GameObject mine;
        private GameObject text;
        private Collider col;

        // ------------------------------------------------- //

        private void Awake()
        {

            anim = GetComponent<Animator>();
            col = GetComponent<Collider>();

        }

        private void OnMouseDown()
        {
            Reveal(5);
        }

        private void Start()
        {
            // Randomly decide if this tile is a mine
            isMine = Random.value < mineChance;

            // Check if its a mine
            if (isMine)
            {
                // Create instance of mine object
                mine = Instantiate(minePrefab, transform);
                mine.SetActive(false);
            }
            else
            {
                // Create instance of text gameobject
                text = Instantiate(textPrefab, transform);
                text.SetActive(false);

            }
        }

        // ------------------------------------------------- //

        public void Reveal(int adjacentMines, int mineState = 0)
        {
            // Flag the tile as being revealed
            isRevealed = true;

            anim.SetTrigger("Reveal");

            // Disable collision
            col.enabled = false;

            // Check if tile is a mine
            if (isMine)
            {
                mine.SetActive(true);   // Make visible
            }
            else
            {
                // Enable text
                text.SetActive(true);

                // Set text
                text.GetComponent<TextMeshPro>().text = adjacentMines.ToString();
            }

        }

        // ------------------------------------------------- //


        // ------------------------------------------------- //

    }
}
