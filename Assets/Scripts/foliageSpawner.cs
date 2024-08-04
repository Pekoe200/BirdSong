using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foliageSpawner : MonoBehaviour
{
    // Reference to the bees prefab
    public GameObject beesPrefab;

    // Chance to spawn bees, 0.1 represents 10%
    private float spawnChance = .1f;

    // This method is called when the collider other enters the trigger
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is the bird
        if (collision.gameObject.CompareTag("Player"))
        {
            // Generate a random float between 0 and 1
            float randomValue = Random.Range(0f, 1f);
            Debug.Log("is bird");

            // If the random value is less than the spawn chance, instantiate the bees prefab
            if (randomValue < spawnChance)
            {
                Instantiate(beesPrefab, transform.position, Quaternion.identity);
                Debug.Log("made bee");
            }
        }
    }
}