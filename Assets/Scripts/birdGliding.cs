using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdGliding : MonoBehaviour {
    private birdController birdController;
    private Rigidbody2D mRigidbody2D;
    private Animator animator;

    public float initialGlideSpeed = 15;
    public float tiltDownSpeedIncrease = 30f;
    public float tiltUpSpeedDecrease = 10f;
    public float stallSpeed = 10f;
    public float maxTiltAngle = 30f;
    public float stallForwardSpeed = 5f;

    private float forwardSpeed; // Make forwardSpeed a class-level variable

    void Start() {
        birdController = GetComponent<birdController>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        forwardSpeed = initialGlideSpeed; // Initialize forwardSpeed
    }

    public void CheckForGlide() {
        if (mRigidbody2D.velocity.y <= 0 && !birdController.isGliding && !birdController.isStalling) {
            if (birdController.hasJumped) {
                birdController.isGliding = true;
                forwardSpeed = initialGlideSpeed; // Set the initial glide speed
                mRigidbody2D.velocity = transform.right * forwardSpeed;
                Debug.Log("Entering glide state with initial speed.");
                animator.SetBool("isGliding", true);
            } else {
                EnterStall();
            }
        }
    }

    public void HandleGliding() {
        float tiltInput = -Input.GetAxis("Vertical");

        if (birdController.isFacingRight) {
            birdController.tiltAngle += tiltInput * Time.deltaTime * 150f;
        } else {
            birdController.tiltAngle -= tiltInput * Time.deltaTime * 150f;
        }

        birdController.tiltAngle = Mathf.Clamp(birdController.tiltAngle, -maxTiltAngle, maxTiltAngle);
        transform.rotation = Quaternion.Euler(0, 0, -birdController.tiltAngle);
        animator.SetFloat("tiltAngle", birdController.tiltAngle);

        if (!birdController.isGrounded) {
            mRigidbody2D.drag = 0;

            if (birdController.isStalling) {
                HandleStalling();
            } else {
                if (birdController.isFacingRight) {
                    if (birdController.tiltAngle > 8) {
                        forwardSpeed += (birdController.tiltAngle / maxTiltAngle) * (tiltDownSpeedIncrease - initialGlideSpeed) * Time.deltaTime;
                        forwardSpeed = Mathf.Min(forwardSpeed, 40f);
                    } else if (birdController.tiltAngle < 8) {
                        forwardSpeed -= (birdController.tiltAngle / -maxTiltAngle) * (initialGlideSpeed - (initialGlideSpeed / 2)) * 2 * Time.deltaTime;
                        if (forwardSpeed <= initialGlideSpeed / 2) {
                            EnterStall();
                            return;
                        }
                    } else {
                        forwardSpeed = Mathf.Lerp(forwardSpeed, initialGlideSpeed, Time.deltaTime);
                    }
                } else {
                    if (birdController.tiltAngle < -4) {
                        forwardSpeed += (birdController.tiltAngle / -maxTiltAngle) * (tiltDownSpeedIncrease - initialGlideSpeed) * Time.deltaTime;
                        forwardSpeed = Mathf.Min(forwardSpeed, 40f);
                    } else if (birdController.tiltAngle > -8) {
                        forwardSpeed -= (birdController.tiltAngle / maxTiltAngle) * (initialGlideSpeed - (initialGlideSpeed / 2)) * 2 * Time.deltaTime;
                        if (forwardSpeed <= initialGlideSpeed / 2) {
                            EnterStall();
                            return;
                        }
                    } else {
                        forwardSpeed = Mathf.Lerp(forwardSpeed, initialGlideSpeed, Time.deltaTime);
                    }
                }

                float glideDirection = birdController.isFacingRight ? 1 : -1;
                mRigidbody2D.velocity = transform.right * forwardSpeed * glideDirection;
            }
        }
    }

    public void AddToForwardSpeed(float amount) {
        forwardSpeed += amount;
        forwardSpeed = Mathf.Max(forwardSpeed, 0); // Ensure forwardSpeed doesn't go below 0
        forwardSpeed = Mathf.Min(forwardSpeed, 40f); // Ensure it doesn't go above 40 eithers
    }

    void HandleStalling() {
        if ((birdController.tiltAngle >= 8 && birdController.isFacingRight) || (birdController.tiltAngle <= -8 && !birdController.isFacingRight)) {
            birdController.isStalling = false;
            animator.SetBool("isStalling", false);
            birdController.isGliding = true;
            mRigidbody2D.gravityScale = 0;
            animator.SetBool("isGliding", true);
        } else {
            mRigidbody2D.velocity = new Vector2(birdController.isFacingRight ? stallForwardSpeed : -stallForwardSpeed, mRigidbody2D.velocity.y);
        }
    }

    void EnterStall() {
        if(!birdController.isFlipping) {
            birdController.isStalling = true;
            mRigidbody2D.gravityScale = 1;
            mRigidbody2D.drag = 60;
            animator.SetBool("isStalling", true);
            animator.SetBool("isGliding", false);
        }
    }
}
