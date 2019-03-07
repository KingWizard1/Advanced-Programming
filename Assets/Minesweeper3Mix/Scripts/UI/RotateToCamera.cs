using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minesweeper3Mix
{

    /// <summary>Billboarding!</summary>
    public class RotateToCamera : MonoBehaviour
    {

        private void Update()
        {
            // Get main cam
            Transform cam = Camera.main.transform;

            // Get the direction to look at
            Vector3 direction = transform.position - cam.position;

            // Rotate by looking in the direction of the camera
            transform.rotation = Quaternion.LookRotation(direction);

        }

    }

}