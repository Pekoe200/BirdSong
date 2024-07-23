using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruitCollision : MonoBehaviour {
    public float staminaRestoreAmount = 20f; // Define the amount of stamina to restore

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {

            birdController bird = collision.GetComponent<birdController>();

            if (bird != null) {
                bird.IncreaseStamina(staminaRestoreAmount);
            }

            // Destroy the fruit object
            Destroy(gameObject);
        }
    }
}