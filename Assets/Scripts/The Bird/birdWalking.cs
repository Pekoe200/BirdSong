using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdWalking : MonoBehaviour {
    private birdController birdController;
    private Rigidbody2D mRigidbody2D;
    private Animator animator; // Add reference to Animator

    public float walkSpeed = 5f;

    void Start()
    {
        birdController = GetComponent<birdController>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>(); // Initialize Animator reference using GetComponentInChildren
    }

    public void HandleWalking()
    {
        float move = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(move, 0);

        if (!birdController.isCharging)
        {
            mRigidbody2D.velocity = new Vector2(moveDirection.x * walkSpeed, mRigidbody2D.velocity.y);
        }

        // Update Animator parameter for xVelocity
        animator.SetFloat("xVelocity", Mathf.Abs(mRigidbody2D.velocity.x));

        if ((move > 0 && !birdController.isFacingRight) || (move < 0 && birdController.isFacingRight))
        {
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        birdController.isFacingRight = !birdController.isFacingRight;
    }
}
