using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carMoveRight : MonoBehaviour
{
    public float carSpeedRight; // Speed of the car

    void Update()
    {
        // Move the car to the left
        transform.Translate(Vector3.right * carSpeedRight * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RightBoundary"))
        {
            Destroy(gameObject);
        }
    }

}
