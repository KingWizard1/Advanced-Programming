using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper2D
{

    public class Grid : MonoBehaviour
    {

        public GameObject tilePrefab;

        public int width = 10, height = 10;

        public float spacing = .155f;

        // ----------------------------------------------------- //

        private Tile[,] tiles;

        // ----------------------------------------------------- //

        private void Start()
        {
            GenerateTiles();
        }

        // ----------------------------------------------------- //

        private void Update()
        {
            // Check if mouse button is pressed
            if (Input.GetMouseButtonDown(0))
                SelectATile();
        }

        // ----------------------------------------------------- //

        // Functionality for spawning tiles
        Tile SpawnTile(Vector3 pos)
        {
            // Clone prefab
            GameObject clone = Instantiate(tilePrefab);

            // Edit its properties
            clone.transform.position = pos;
            Tile currentTile = clone.GetComponent<Tile>();

            // Return it
            return currentTile;
        }

        // ----------------------------------------------------- //

        /// <summary>Spawns tiles in a grid like pattern.</summary>
        void GenerateTiles()
        {

            // Create a new 2D array of size width x height
            tiles = new Tile[width, height];

            // Store half size for later use
            Vector2 halfSize = new Vector2(width * 0.5f, height * 0.5f);

            // Loop through the entire tile list
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    // Pivot tiles around grid
                    Vector2 pos = new Vector2(x - halfSize.x, y - halfSize.y);

                    Vector2 offset = new Vector2(.5f, .5f);
                    pos += offset;

                    // Apply spacing
                    pos *= spacing;

                    // Spawn the tile using spawn funciton mode earlier
                    Tile tile = SpawnTile(pos);

                    // Attach newly spawned tile to self (transform)
                    tile.transform.SetParent(transform);

                    // Store its array coordinates within itself for future reference
                    tile.x = x;
                    tile.y = y;

                    // Store tile in array at those coordinates
                    tiles[x, y] = tile;

                }
            }

        }

        // ----------------------------------------------------- //

        public int GetAdjacentMineCount(Tile tile)
        {

            // Set count to 0
            int count = 0;

            // Loop through all the adjacent tiles on the X
            for (int x = -1; x <= 1; x++)
            {

                // Loop through all the adjacent tiles on the Y
                for (int y = -1; y <= 1; y++)
                {

                    // Calc which adjacent tile to look at
                    int desiredX = tile.x + x;
                    int desiredY = tile.y + y;

                    // Check if the desired x and y is outside bounds.
                    // If yes, continue to next element in loop.
                    if (desiredX < 0 || desiredX >= width ||
                        desiredY < 0 || desiredY >= height)
                        continue;

                    // Select current tile
                    Tile currentTile = tiles[desiredX, desiredY];

                    // Check if that tile is a mine. If yes, inc by 1.
                    if (currentTile.isMine)
                        count++;
                }

            }

            // Remember to return the count!
            return count;

        }


        // ----------------------------------------------------- //

        private void SelectATile()
        {

            // Generate a ray from the camera with mouse position
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Perform raycast
            RaycastHit2D hit = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

            // If the mouse hit something
            if (hit.collider != null)
            {

                // Try getting a Tile component form the thing we hit
                var hitTile = hit.collider.GetComponent<Tile>();
                if (hitTile != null)
                {

                    print("Hit " + hitTile.name);

                    // Get a count of all mines around the hit tile
                    int adjacentMines = GetAdjacentMineCount(hitTile);

                    // Reveal what that hit tile is
                    hitTile.Reveal(adjacentMines);
                }

            }

        }

        // ----------------------------------------------------- //

    }

}
