using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Odometer : MonoBehaviour
{
    private birdController birdController;

    public float minRotation = 119f;
    public float maxRotation = -119f;
    public float maxSpeed = 40f;
    public float lerpSpeed = 5f; // Speed of the lerp

    private float currentRotation;

    void Start()
    {
        // Find the birdController component
        GameObject bird = GameObject.FindWithTag("Player");
        if (bird != null)
        {
            birdController = bird.GetComponent<birdController>();
        }

        // Initialize current rotation
        currentRotation = minRotation;
    }

    void Update()
    {
        if (birdController != null)
        {
            // Get the bird's forward speed
            float speed = birdController.GetForwardSpeed();
            // Map the speed to the target rotation
            float targetRotation = Mathf.Lerp(minRotation, maxRotation, speed / maxSpeed);
            // Smoothly interpolate the current rotation to the target rotation
            currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * lerpSpeed);
            // Apply the rotation to the dial
            transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
        }
    }
}
