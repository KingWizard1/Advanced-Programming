using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper3Mix
{

    public class PlayGrid : MonoBehaviour
    {

        public GameObject tilePrefab;
        public int width = 10, height = 10, depth = 10;
        public float spacing = 1.1f;


        /// <summary>3D array to store all the tiles.</summary>
        private Tile[,,] tiles;

        // ------------------------------------------------- //

        private void Start()
        {
            GenerateTiles();
        }

        // ------------------------------------------------- //

        void Update()
        {
            MouseOver();
            //UpdateGrid();
        }

        // ------------------------------------------------- //

        // Raycasts to find a hit tile
        void MouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Tile hitTile = GetHitTile(Input.mousePosition);
                if (hitTile && !hitTile.isFlagged)
                {
                    SelectTile(hitTile);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Tile hitTile = GetHitTile(Input.mousePosition);
                if (hitTile)
                {
                    hitTile.Flag();
                }
            }
        }

        // ------------------------------------------------- //
        /// <summary>Not necessary for gameplay, but...
        /// Adjust the <see cref="spacing"/> value in the inspector during game play for coolness ;)</summary>
        void UpdateGrid()
        {
            // Calc half the size of the grid (half size of an object whose scale = 1)
            Vector3 halfSize = new Vector3(width * .5f, height * .5f, depth * .5f);

            // Loop through all the tiles in the array
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        // Store tile in array at those coordinates
                        Tile tile = tiles[x, y, z];

                        // Generate position for new tile
                        var pos = new Vector3(x - halfSize.x,
                                              y - halfSize.y,
                                              z - halfSize.z);

                        // Offset position to center
                        pos += offset;

                        // Apply spacing
                        pos *= spacing;

                        // Set pos
                        tile.transform.position = pos;
                    }
                }
            }
        }

        // ------------------------------------------------- //

        // Offset
        Vector3 offset = new Vector3(.5f, .5f, .5f);

        void GenerateTiles()
        {

            // Instantiate the new 3D array of size width x height x depth.
            tiles = new Tile[width, height, depth];

            // Calc half the size of the grid (half size of an object whose scale = 1)
            Vector3 halfSize = new Vector3(width * .5f, height * .5f, depth * .5f);

            // Loop through all the tiles in the array
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {

                        // Generate position for new tile
                        var pos = new Vector3(x - halfSize.x,
                                              y - halfSize.y,
                                              z - halfSize.z);

                        // Apply offset
                        pos += offset;

                        // Apply spacing
                        pos *= spacing;

                        // Spawn new tile
                        var newTile = SpawnTile(pos);

                        // Store tiles coordinates inside itself for future reference
                        newTile.x = x;
                        newTile.y = y;
                        newTile.z = z;

                        // Store tile in array at those coordinates
                        tiles[x, y, z] = newTile;


                    }
                }
            }

        }

        // ------------------------------------------------- //

        private Tile SpawnTile(Vector3 position)
        {

            // Clone the tile prefab
            var clone = Instantiate(tilePrefab);

            // Keep the hierarchy clean
            clone.transform.SetParent(transform);

            // Edit its properties
            clone.transform.position = position;

            // Return the tile component of clone
            return clone.GetComponent<Tile>();


        }

        // ------------------------------------------------- //
        
        // Uncovers all mines in the grid
        void UncoverAllMines()
        {
            // Loop through entire grid
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Tile tile = tiles[x, y, z];
                        // Check if tile is a mine
                        if (tile.isMine)
                        {
                            // Reveal that tile
                            tile.Reveal();
                        }
                    }
                }
            }
        }

        // ------------------------------------------------- //

        // Performs set of actions on selected tile
        void SelectTile(Tile selected)
        {
            int adjacentMines = GetAdjacentMineCount(selected);
            selected.Reveal(adjacentMines);

            // Is the selected tile a mine?
            if (selected.isMine)
            {
                // Uncover all mines
                UncoverAllMines();
                // Game Over - Lose
                print("Game Over - You lose.");
            }
            // Else, are there no more mines around this tile?
            else if (adjacentMines == 0)
            {
                int x = selected.x;
                int y = selected.y;
                int z = selected.z;
                // Use Flood Fill to uncover all adjacent mines
                FFuncover(x, y, z, new bool[width, height, depth]);
            }
            // Are there no more empty tiles in the game at this point?
            if (NoMoreEmptyTiles())
            {
                //  Uncover all mines
                UncoverAllMines();
                // Game Over - Win
                print("Game Over - You Win!");
            }
        }

        // ------------------------------------------------- //

        void FFuncover(int x, int y, int z, bool[,,] visited)
        {
            // Is x and y out of bounds of the grid?
            if (IsOutOfBounds(x, y, z))
            {
                // Exit
                return;
            }

            // Have the coordinates already been visited?
            if (visited[x, y, z])
            {
                // Exit
                return;
            }
            // Reveal that tile in that X and Y coordinate
            Tile tile = tiles[x, y, z];
            // Get number of mines around that tile
            int adjacentMines = GetAdjacentMineCount(tile);
            // Reveal the tile
            tile.Reveal(adjacentMines);

            // If there are no adjacent mines around that tile
            if (adjacentMines == 0)
            {
                // This tile has been visited
                visited[x, y, z] = true;
                // Visit all other tiles around this tile
                FFuncover(x - 1, y, z, visited);
                FFuncover(x + 1, y, z, visited);

                FFuncover(x, y - 1, z, visited);
                FFuncover(x, y + 1, z, visited);

                FFuncover(x, y, z - 1, visited);
                FFuncover(x, y, z + 1, visited);
            }
        }

        // ------------------------------------------------- //

        // Scans the grid to check if there are no more empty tiles
        bool NoMoreEmptyTiles()
        {
            // Set empty tile count to 0
            int emptyTileCount = 0;
            // Loop through 2D array
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        Tile tile = tiles[x, y, z];
                        // If tile is revealed or is a mine
                        if (tile.isRevealed || tile.isMine)
                        {
                            // Skip to next loop iteration
                            continue;
                        }
                        // An empty tile has not been revealed
                        emptyTileCount++;
                    }
                }
            }
            // Return true if all empty tiles have been revealed
            return emptyTileCount == 0;
        }

        // ------------------------------------------------- //

        Tile GetHitTile(Vector2 mousePosition)
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit))
            {
                return hit.collider.GetComponent<Tile>();
            }
            return null;
        }

        // ------------------------------------------------- //

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
                    // Loop through all the adjacent tiles on the Y
                    for (int z = -1; z <= 1; z++)
                    {

                        // Calc which adjacent tile to look at
                        int desiredX = tile.x + x;
                        int desiredY = tile.y + y;
                        int desiredZ = tile.z + z;

                        // Check if the desired x and y is outside bounds.
                        // If yes, continue to next element in loop.
                        if (desiredX < 0 || desiredX >= width ||
                            desiredY < 0 || desiredY >= height ||
                            desiredZ < 0 || desiredZ >= depth)
                            continue;

                        // Select current tile
                        Tile currentTile = tiles[desiredX, desiredY, desiredZ];

                        // Check if that tile is a mine. If yes, inc by 1.
                        if (currentTile.isMine)
                            count++;
                    }

                }

            }

            // Remember to return the count!
            return count;

        }

        // ------------------------------------------------- //

        bool IsOutOfBounds(int x, int y, int z)
        {
            return x < 0 || x >= width ||
                   y < 0 || y >= height ||
                   z < 0 || z >= depth;
        }

    }

}
