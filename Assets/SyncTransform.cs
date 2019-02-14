using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
public class SyncTransform : NetworkBehaviour /* Derives from MonoBehavior */ {

    // Speed of lerping rotation and position
    public float lerpRate = 15;

    // Threshold for when to send commands, so we don't spam the server
    public float positionThreshold = 0.5f;
    public float rotationThreshold = 5.0f;

    // The previous position and rotation that was sent to the server
    private Vector3 lastPosition;
    private Quaternion lastRotation;

    // ----------------------------------------------------- //

    // Vars to be synced across the network
    [SyncVar] private Vector3 syncPosition;
    [SyncVar] private Quaternion syncRotation;

    private Rigidbody rigid;

    // ----------------------------------------------------- //

    // Use this for initialization
    void Start () {

        rigid = GetComponent<Rigidbody>();

	}

    // ----------------------------------------------------- //

    void FixedUpdate () {

        TransmitPosition();
        LerpPosition();

        TransmitRotation();
        LerpRotation();

	}

    // ----------------------------------------------------- //

    [ClientCallback]
    private void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(rigid.position, lastPosition) > positionThreshold)
        {
            CmdSendPositionToServer(rigid.position);
            lastPosition = rigid.position;
        }
    }

    [ClientCallback]
    private void TransmitRotation()
    {
        if (isLocalPlayer && Quaternion.Angle(rigid.rotation, lastRotation) > rotationThreshold)
        {
            CmdSendRotationToServer(rigid.rotation);
            lastRotation = rigid.rotation;
        }
    }

    // ----------------------------------------------------- //

    [Command]
    private void CmdSendPositionToServer(Vector3 position)
    {
        syncPosition = position;
        //Debug.Log("Position command");
    }

    [Command]
    private void CmdSendRotationToServer(Quaternion rotation)
    {
        syncRotation = rotation;
        //Debug.Log("Rotation command");
    }

    // ----------------------------------------------------- //


    private void LerpPosition()
    {

        // If the current instance is not the local player
        if (!isLocalPlayer)
        {
            // Lerp position of fall other connected clients
            rigid.position = Vector3.Lerp(rigid.position, syncPosition, Time.deltaTime * lerpRate);
        }

    }

    private void LerpRotation()
    {

        // If the current instance is not the local player
        if (!isLocalPlayer)
        {
            // Lerp rotation of fall other connected clients
            rigid.rotation = Quaternion.Lerp(rigid.rotation, syncRotation, Time.deltaTime * lerpRate);
        }

    }

}
