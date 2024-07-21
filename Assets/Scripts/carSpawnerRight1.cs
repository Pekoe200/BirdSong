using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carSpawnerRight1 : MonoBehaviour
{
    public GameObject carPrefab2; // The car prefab to spawn
    public float spawnInterval; // Time interval between spawns
    public Vector3 spawnPosition = new Vector3(10, 0, 0); // Position to spawn cars

    private float timer = 0.0f;

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn a new car
        if (timer >= spawnInterval)
        {
            // Reset the timer
            timer = 0.0f;

            // Instantiate a new car at the spawn position
            Instantiate(carPrefab2, spawnPosition, Quaternion.identity);
        }
    }
}
