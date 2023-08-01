using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewParallex : MonoBehaviour
{
    public Transform virtualCamera;
    public float parallaxStrength = 0.5f;

    private Vector3 initialPosition;
    private Vector3 previousCameraPosition;

    private void Start()
    {
        initialPosition = transform.position;
        previousCameraPosition = virtualCamera.position;
    }

    private void LateUpdate()
    {
        // Calculate the movement of the virtual camera
        Vector3 cameraMovement = virtualCamera.position - previousCameraPosition;

        // Calculate the parallax effect for this background layer
        Vector3 parallaxMovement = new Vector3(cameraMovement.x * parallaxStrength, 0f, 0f);

        // Apply the parallax effect to the background layer
        transform.position += parallaxMovement;

        previousCameraPosition = virtualCamera.position;
    }
}
