using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fruitCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Destroy the fruit object
            Destroy(gameObject);
        }
    }
}
