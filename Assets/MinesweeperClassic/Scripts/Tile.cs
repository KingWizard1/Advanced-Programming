using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper2D
{

    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {

        public int x, y;

        public bool isMine = false;

        [Range(0.01f, 0.9f)]
        public float mineChance = 0.15f;

        public bool isRevealed = false;

        [Header("References")]
        public Sprite[] emptySprites;
        public Sprite[] mineSprites;
        private SpriteRenderer rend;


        // ----------------------------------------------------- //

        private void Awake()
        {
            rend = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // Randomly decide if this tile is a mine
            isMine = Random.value < mineChance;
        }

        // ----------------------------------------------------- //

        public void Reveal(int adjacentMines, int mineState = 0)
        {
            // Flags the tlie as being revealed
            isRevealed = true;

            if (isMine)
            {
                // Set sprite to mine sprite
                rend.sprite = mineSprites[mineState];
            }
            else
            {
                // Sets sprite to appropriate texture based on adjacent mines
                rend.sprite = emptySprites[adjacentMines];
            }
        }

        // ----------------------------------------------------- //

    }
}

