using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class LookScript : NetworkBehaviour {

    // Sensitivity of the mouse
    public float mouseSensitivity = 2.0f;
    // Min/max Y axis (degrees)
    public float minimumY = -90f;
    public float maximumY = 90f;

    // ----------------------------------------------------- //

    // Yaw of the camera (Rotation on Y)
    private float yaw = 0f;
    // Pitch of the camera (Rotation on X)
    private float pitch = 0f;
    // Main camera reference
    private GameObject mainCamera;
    
    // ----------------------------------------------------- //

    void Start () {

        // Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;

        // Make cursor invisible
        Cursor.visible = false;

        // Gets reference to the camera inside of this gameobject
        // (the camera should be a child of the player object).
        Camera cam = GetComponentInChildren<Camera>();
        mainCamera = cam.gameObject;

	}

    private void OnDestroy()
    {
        // Release the cursor and make it visible.
        // This is so when we leave the game, or when we die, we can use the mouse again.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ----------------------------------------------------- //

    void Update () {

        // Update for local player only
        if (isLocalPlayer)
            HandleInput();

	}

    // Remember to use LateUpdate for when you move the camera
    void LateUpdate()
    {

        if (isLocalPlayer)
            mainCamera.transform.localEulerAngles = new Vector3(-pitch, 0, 0);

    }

    // ----------------------------------------------------- //

    private void HandleInput()
    {

        yaw = mainCamera.transform.rotation.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch = pitch + Input.GetAxis("Mouse Y") * mouseSensitivity;

        //yaw = mainCamera.transform.rotation.y + Input.GetAxis("Horizontal") * mouseSensitivity;
        //pitch = mainCamera.transform.rotation.x + Input.GetAxis("Vertical") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minimumY, maximumY);

        mainCamera.transform.localEulerAngles = new Vector3(0, yaw);

        
    }

    // ----------------------------------------------------- //

}
