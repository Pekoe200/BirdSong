using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public GameObject bird;
    private Vector3 offset;
    private Camera cam;

    public float minFOV = 60f; // Minimum field of view
    public float midFOV = 80f; // Mid-level field of view
    public float maxFOV = 90f; // Maximum field of view

    public float lowSpeedThreshold = 5f; // Speed threshold for mid-level FOV
    public float highSpeedThreshold = 20f; // Speed threshold for maximum FOV

    public float smoothSpeed = .5f; // Smoothing factor for FOV transition

    void Start()
    {
        offset = transform.position - bird.transform.position;
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        transform.position = bird.transform.position + offset;

        float birdSpeed = bird.GetComponent<Rigidbody2D>().velocity.magnitude;
        float targetFOV = minFOV;

        if (birdSpeed > highSpeedThreshold)
        {
            targetFOV = maxFOV;
        }
        else if (birdSpeed > lowSpeedThreshold)
        {
            targetFOV = midFOV;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, smoothSpeed * Time.deltaTime);
    }
}
