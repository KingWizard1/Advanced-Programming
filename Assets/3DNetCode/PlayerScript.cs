using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : NetworkBehaviour /* Derives from MonoBehavior */
{

    public float movementSpeed = 10.0f;
    public float rotationSpeed = 5.0f;
    public float jumpHeight = 2.0f;

    // ----------------------------------------------------- //

    private bool isGrounded = false;
    private Rigidbody rigid;

    private const string remoteLayerName = "RemotePlayer";

    // ----------------------------------------------------- //

    // Use this for initialization
    void Start()
    {

        rigid = GetComponent<Rigidbody>();

        // Get Camera, and its Audio Listener (AL is a component of Camera)
        Camera camera = GetComponentInChildren<Camera>();
        AudioListener audioListener = GetComponentInChildren<AudioListener>();

        // If the current instance is the local player
        if (isLocalPlayer)
        {
            // Enable everything
            camera.enabled = true;
            audioListener.enabled = true;
        }
        else
        {
            // Disable everything
            camera.enabled = false;
            audioListener.enabled = false;

            // Assign remote layer
            AssignRemoteLayer();
        }

        // Register this player on the network
        RegisterPlayer();

    }

    // Update is called once per frame
    void Update()
    {

        // "This is important so that when other users connect to our game,
        // each player won't be able to control the other players."
        if (isLocalPlayer)
        {
            HandleInput();

        }

    }

    // ----------------------------------------------------- //

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    // ----------------------------------------------------- //

    private void HandleInput()
    {
        KeyCode[] keys = {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D,
            KeyCode.Space,
        };

        foreach (var key in keys)
            if (Input.GetKey(key))
                Move(key);
    }

    // ----------------------------------------------------- //

    private void Move(KeyCode key)
    {

        Vector3 position = rigid.position;
        Quaternion rotation = rigid.rotation;

        switch (key)
        {

            case KeyCode.W:
                position += transform.forward * movementSpeed * Time.deltaTime;
                break;

            case KeyCode.S:
                position += -transform.forward * movementSpeed * Time.deltaTime;
                break;

            case KeyCode.A:
                //rotation *= Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
                position += -transform.right * movementSpeed * Time.deltaTime;
                break;

            case KeyCode.D:
                //rotation *= Quaternion.AngleAxis(rotationSpeed, Vector3.up);
                position += transform.right * movementSpeed * Time.deltaTime;
                break;

            case KeyCode.Space:
                if (isGrounded)
                {
                    rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
                    isGrounded = false;
                }
                break;

        }

        rigid.MovePosition(position);
        rigid.MoveRotation(rotation);

    }

    // ----------------------------------------------------- //

    /// <summary>Register player's ID on the network.</summary>
    private void RegisterPlayer()
    {
        // Get the id from the network identity component
        string id = "Player " + GetComponent<NetworkIdentity>().netId;
        this.name = id;
    }

    // Assign remote layer to current gameObject (if it is not local player)
    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

}
