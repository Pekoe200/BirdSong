using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdGliding : MonoBehaviour {
    private birdController birdController;
    private Rigidbody2D mRigidbody2D;
    private Animator animator; // Add reference to Animator

    public float initialGlideSpeed = 15;
    public float tiltDownSpeedIncrease = 30f;
    public float tiltUpSpeedDecrease = 10f;
    public float stallSpeed = 10f;
    public float maxTiltAngle = 30f;
    public float stallForwardSpeed = 5f;

    void Start()
    {
        birdController = GetComponent<birdController>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>(); // Initialize Animator reference using GetComponentInChildren
    }

    public void CheckForGlide()
    {
        if (mRigidbody2D.velocity.y <= 0 && !birdController.isGliding && !birdController.isStalling)
        {
            birdController.isGliding = true;
            mRigidbody2D.velocity = transform.right * initialGlideSpeed;
            Debug.Log("Entering glide state with initial speed.");
            animator.SetBool("isGliding", true); // Update Animator parameter
        }
    }

    public void HandleGliding()
    {
        float tiltInput = Input.GetAxis("Horizontal");
        birdController.tiltAngle += tiltInput * Time.deltaTime * 100f;
        birdController.tiltAngle = Mathf.Clamp(birdController.tiltAngle, -maxTiltAngle, maxTiltAngle);
        mRigidbody2D.drag = 0;

        transform.rotation = Quaternion.Euler(0, 0, -birdController.tiltAngle);

        // Update Animator parameter for tiltAngle
        animator.SetFloat("tiltAngle", birdController.tiltAngle);

        if (birdController.isStalling)
        {
            if ((birdController.tiltAngle >= 8 && birdController.isFacingRight) || (birdController.tiltAngle <= -8 && !birdController.isFacingRight))
            {
                birdController.isStalling = false;
                animator.SetBool("isStalling", false);
                birdController.isGliding = true;
                mRigidbody2D.gravityScale = 0;
                Debug.Log("Resuming glide state.");
                animator.SetBool("isGliding", true); // Update Animator parameter
            }
            else
            {
                mRigidbody2D.velocity = new Vector2(birdController.isFacingRight ? stallForwardSpeed : -stallForwardSpeed, mRigidbody2D.velocity.y);
            }
        }
        else
        {
            float forwardSpeed = mRigidbody2D.velocity.magnitude;

            if (birdController.isFacingRight)
            {
                if (birdController.tiltAngle > 8)
                {
                    forwardSpeed += (birdController.tiltAngle / maxTiltAngle) * (tiltDownSpeedIncrease - initialGlideSpeed) * Time.deltaTime;
                    forwardSpeed = Mathf.Min(forwardSpeed, 40f);
                }
                else if (birdController.tiltAngle < 8)
                {
                    forwardSpeed -= (birdController.tiltAngle / -maxTiltAngle) * (initialGlideSpeed - (initialGlideSpeed / 2)) * 2 * Time.deltaTime;

                    if (forwardSpeed <= initialGlideSpeed / 2)
                    {
                        birdController.isGliding = false;
                        birdController.isStalling = true;
                        animator.SetBool("isStalling", true);
                        mRigidbody2D.gravityScale = 1;
                        mRigidbody2D.drag = 60;
                        Debug.Log("Stalled. Falling.");
                        animator.SetBool("isGliding", false); // Update Animator parameter
                        return;
                    }
                }
                else
                {
                    forwardSpeed = Mathf.Lerp(forwardSpeed, initialGlideSpeed, Time.deltaTime);
                }
            }
            else
            {
                if (birdController.tiltAngle < -4)
                {
                    forwardSpeed += (birdController.tiltAngle / -maxTiltAngle) * (tiltDownSpeedIncrease - initialGlideSpeed) * Time.deltaTime;
                    forwardSpeed = Mathf.Min(forwardSpeed, 40f);
                }
                else if (birdController.tiltAngle > -8)
                {
                    forwardSpeed -= (birdController.tiltAngle / maxTiltAngle) * (initialGlideSpeed - (initialGlideSpeed / 2)) * 2 * Time.deltaTime;

                    if (forwardSpeed <= initialGlideSpeed / 2)
                    {
                        birdController.isGliding = false;
                        birdController.isStalling = true;
                        animator.SetBool("isStalling", true);
                        mRigidbody2D.gravityScale = 1;
                        mRigidbody2D.drag = 60;
                        Debug.Log("Stalled. Falling.");
                        animator.SetBool("isGliding", false); // Update Animator parameter
                        return;
                    }
                }
                else
                {
                    forwardSpeed = Mathf.Lerp(forwardSpeed, initialGlideSpeed, Time.deltaTime);
                }
            }

            float glideDirection = birdController.isFacingRight ? 1 : -1;
            mRigidbody2D.velocity = transform.right * forwardSpeed * glideDirection;

            Debug.Log("Gliding. Tilt angle: " + birdController.tiltAngle + ", Speed: " + forwardSpeed);
        }
    }
}
