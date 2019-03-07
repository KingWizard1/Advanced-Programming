using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class ShootScript : NetworkBehaviour {

    /// <summary>Number of bullets that can be fired per second.</summary>
    public float fireRate = 1f;
    /// <summary>Maximum that the bullet can travel.</summary>
    public float range = 100f;
    /// <summary>LayerMask of which layer to hit.</summary>
    public LayerMask mask;

    // ----------------------------------------------------- //

    /// <summary>Timer for the fireRate.</summary>
    private float fireFactor = 0f;
    /// <summary>Reference to the camera child.</summary>
    private new Camera camera;

    // ----------------------------------------------------- //

    void Start () {

        camera = GetComponentInChildren<Camera>();
        
	}

    // ----------------------------------------------------- //

    void Update () {

        if (isLocalPlayer)
            HandleInput();

	}

    // ----------------------------------------------------- //

    private void HandleInput()
    {

        fireFactor = fireFactor + Time.deltaTime;

        var fireInterval = 1 / fireRate;

        if (fireFactor >= fireInterval)
        {
            if (Input.GetMouseButtonDown(0))
                Shoot();
        }

    }

    // ----------------------------------------------------- //

    /// <summary>This function sends a message to all other clients (via the server) to
    /// tell them which client got shot using its id.</summary>
    [Command]
    private void CmdPlayerShot(string id)
    {

        Debug.Log("Player " + id + " has been shot!");


    }
        
    [Client]
    private void Shoot()
    {

        // Couldn't get this to work.
        // Raycast hitinfo doesn't return a collider :/

        Ray camRay = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hitInfo;
        Physics.Raycast(camRay, out hitInfo, range, mask);

        if (hitInfo.collider && hitInfo.collider.tag == "Player")
            CmdPlayerShot(hitInfo.collider.name);


    }

    // ----------------------------------------------------- //

}
