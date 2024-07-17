using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdWalking : MonoBehaviour {
    private birdController birdController;
    private Rigidbody2D mRigidbody2D;
    public float walkSpeed = 5f;

    void Start()
    {
        birdController = GetComponent<birdController>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void HandleWalking()
    {
        mRigidbody2D.gravityScale = 1;
        float move = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(move, 0);

        if (!birdController.isCharging)
        {
            mRigidbody2D.velocity = new Vector2(moveDirection.x * walkSpeed, mRigidbody2D.velocity.y);
        }

        if ((move > 0 && !birdController.isFacingRight) || (move < 0 && birdController.isFacingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        birdController.isFacingRight = !birdController.isFacingRight;
    }
}