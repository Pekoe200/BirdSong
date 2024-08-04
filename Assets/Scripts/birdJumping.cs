using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdJumping : MonoBehaviour {
    private birdController birdController;
    private Rigidbody2D mRigidbody2D;
    private birdStamina birdStamina;
    public float jumpForce = 5f;
    public float maxJumpForce = 30f;
    public float jumpChargeRate = 15f;

    private float currentJumpForce;
    private Animator animator;
    private bool chargeStarted = false;

    void Start() {
        birdController = GetComponent<birdController>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        birdStamina = GetComponent<birdStamina>();
        currentJumpForce = jumpForce;
        animator = GetComponentInChildren<Animator>();
    }

    public void HandleJumping() {
        if (Input.GetKeyDown(KeyCode.Space) && birdController.currentStamina > 0 && !chargeStarted) {
            birdController.isCharging = true;
            animator.SetBool("isCharging", true);
            birdController.shouldStayGrounded = false;
            chargeStarted = true;
        }

        if (Input.GetKey(KeyCode.Space) && birdController.currentStamina > 0 && chargeStarted) {
            if (currentJumpForce < maxJumpForce) {
                mRigidbody2D.velocity = new Vector2(0, 0);
                float forceIncrease = jumpChargeRate * Time.deltaTime;
                currentJumpForce += forceIncrease;
                currentJumpForce = Mathf.Clamp(currentJumpForce, jumpForce, maxJumpForce);

                float staminaToDeplete = forceIncrease;
                birdStamina.DecreaseStamina(staminaToDeplete);

                modCamera.Instance.ShakeCamera(currentJumpForce / maxJumpForce);
                modCamera.Instance.ZoomCamera(currentJumpForce / maxJumpForce);
            }

            if (birdController.currentStamina == 0) {
                ApplyJumpForce();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && chargeStarted) {
            ApplyJumpForce();
        }

        if (!Input.GetKey(KeyCode.Space)) {
            chargeStarted = false;
        }
    }

    void ApplyJumpForce() {
        birdController.isGrounded = false;
        birdController.shouldStayGrounded = false;
        birdController.isCharging = false;
        birdController.hasJumped = true;
        mRigidbody2D.gravityScale = 1;

        animator.SetBool("hasJumped", true);
        animator.SetBool("isCharging", false);
        animator.SetBool("isGrounded", false);

        modCamera.Instance.ShakeCamera(0f);
        modCamera.Instance.ZoomCamera(0f);

        mRigidbody2D.velocity = new Vector2(mRigidbody2D.velocity.x, currentJumpForce);
        currentJumpForce = jumpForce;
        chargeStarted = false;
    }
}
