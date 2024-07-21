using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdQuickLanding : MonoBehaviour {
    private birdController birdController;
    private Rigidbody2D mRigidbody2D;
    public float smoothSpeed = 2f;

    void Start()
    {
        birdController = GetComponent<birdController>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void HandleQuickLanding()
    {
        if (Input.GetKey(KeyCode.LeftShift) && IsGroundBelow())
        {
            QuickLand();
        }
    }

    bool IsGroundBelow()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, birdController.landingCheckDistance, birdController.groundLayerMask);
        Debug.Log("Raycast hit: " + hit.collider);
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            Debug.Log("Ground detected below.");
            return true;
        }
        Debug.Log("No ground detected.");
        return false;
    }

    void QuickLand()
    {
        mRigidbody2D.velocity = new Vector2(0, Mathf.Lerp(-2, -birdController.quickLandingSpeed, smoothSpeed * Time.deltaTime));
        Debug.Log("Quick landing.");
    }
}
