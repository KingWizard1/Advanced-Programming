using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper3D
{

    public class Grid : MonoBehaviour
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

        private void Update()
        {
            
        }

        // ------------------------------------------------- //

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

            // Edit its properties
            clone.transform.position = position;

            // Return the tile component of clone
            return clone.GetComponent<Tile>();


        }
    }

}
