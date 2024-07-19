using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carMoveLeft : MonoBehaviour
{
    public float carSpeed; // Speed of the car

    void Update()
    {
        // Move the car to the left
        transform.Translate(Vector3.left * carSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LeftBoundary"))
        {
            Destroy(gameObject);
        }
    }
}
