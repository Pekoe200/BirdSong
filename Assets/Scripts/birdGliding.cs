using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdGliding : MonoBehaviour {
    private birdController birdController;
    private birdWalking birdWalking;
    private Rigidbody2D mRigidbody2D;
    private Animator animator;

    public float initialGlideSpeed = 15;
    public float tiltDownSpeedIncrease = 30f;
    public float tiltUpSpeedDecrease = 10f;
    public float stallSpeed = 10f;
    public float maxTiltAngle = 30f;
    public float stallForwardSpeed = 5f;
    public float boostStamina = 10f;

    private float forwardSpeed; // Make forwardSpeed a class-level variable
    private float boostFactor = 1f;

    void Start() {
        birdController = GetComponent<birdController>();
        birdWalking = GetComponent<birdWalking>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        forwardSpeed = initialGlideSpeed; // Initialize forwardSpeed
    }

    public void CheckForGlide() {
        if (mRigidbody2D.velocity.y <= 0 && !birdController.isGliding && !birdController.isStalling) {
            if (birdController.hasJumped) {
                birdController.isGliding = true;
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
                forwardSpeed = mRigidbody2D.velocity.magnitude;
                if (birdController.isFacingRight) {
                    
                    forwardSpeed = mRigidbody2D.velocity.magnitude;
                    if (birdController.tiltAngle > 8) {
                        birdController.hasJumped = false;
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
                    if (birdController.tiltAngle <= -8) {
                        birdController.hasJumped = false;
                        forwardSpeed += (birdController.tiltAngle / -maxTiltAngle) * (tiltDownSpeedIncrease - initialGlideSpeed) * Time.deltaTime;
                        forwardSpeed = Mathf.Min(forwardSpeed, 40f);
                    } else if (birdController.tiltAngle >= -8) {
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
                mRigidbody2D.velocity = transform.right * forwardSpeed * boostFactor * glideDirection;
                // Reset boostFactor to 1 after applying
                boostFactor = 1f;
            }
        }
    }

    public void ApplyBoost(float boostAmount) {
        boostFactor += boostAmount / forwardSpeed; // Adjust boostFactor based on boostAmount

    }

    void HandleStalling() {
        if ((birdController.tiltAngle >= 8 && birdController.isFacingRight) || (birdController.tiltAngle <= -8 && !birdController.isFacingRight)) {
            birdController.isStalling = false;
            animator.SetBool("isStalling", false);
            birdController.isGliding = true;
            mRigidbody2D.gravityScale = 0;
            animator.SetBool("isGliding", true);
        } else {
            if (!birdController.hasJumped) {
                mRigidbody2D.velocity = new Vector2(birdController.isFacingRight ? stallForwardSpeed : -stallForwardSpeed, mRigidbody2D.velocity.y);
            } else {
                float move = Input.GetAxis("Horizontal");
                Vector2 moveDirection = new Vector2(move, 0);
                mRigidbody2D.velocity = new Vector2(moveDirection.x * 3f, mRigidbody2D.velocity.y);
                if ((move > 0 && !birdController.isFacingRight) || (move < 0 && birdController.isFacingRight))
                {
                    birdWalking.Flip();
                }
            }
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

    public float GetForwardSpeed() {
        return forwardSpeed;
    }
}
