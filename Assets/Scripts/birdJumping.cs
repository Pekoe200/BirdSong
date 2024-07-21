using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdJumping : MonoBehaviour {
    private birdController birdController;
    private Rigidbody2D mRigidbody2D;
    public float jumpForce = 5f;
    public float maxJumpForce = 30f;
    public float jumpChargeRate = 15f;

    private float currentJumpForce;

    void Start()
    {
        birdController = GetComponent<birdController>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        currentJumpForce = jumpForce;
    }

    public void HandleJumping()
    {
        if (Input.GetKey(KeyCode.Space) && birdController.currentStamina > 0)
        {
            if (currentJumpForce < maxJumpForce)
            {
                birdController.isCharging = true;
                mRigidbody2D.velocity = new Vector2(0, 0);
                float forceIncrease = jumpChargeRate * Time.deltaTime;
                currentJumpForce += forceIncrease;
                currentJumpForce = Mathf.Clamp(currentJumpForce, jumpForce, maxJumpForce);

                float staminaToDeplete = forceIncrease;
                birdController.currentStamina -= staminaToDeplete;
                birdController.currentStamina = Mathf.Clamp(birdController.currentStamina, 0, birdController.maxStamina);

                modCamera.Instance.ShakeCamera(currentJumpForce / maxJumpForce);
                modCamera.Instance.ZoomCamera(currentJumpForce / maxJumpForce);
            }

            if (birdController.currentStamina == 0)
            {
                ApplyJumpForce();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && currentJumpForce > jumpForce)
        {
            ApplyJumpForce();
        }

        if (Input.GetKey(KeyCode.Space) && birdController.currentStamina <= 0)
        {
            Debug.Log("Cannot initiate jump. Stamina depleted.");
        }
    }

    void ApplyJumpForce()
    {
        birdController.isCharging = false;
        birdController.isGrounded = false;

        modCamera.Instance.ShakeCamera(0f);
        modCamera.Instance.ZoomCamera(0f);

        mRigidbody2D.velocity = new Vector2(mRigidbody2D.velocity.x, currentJumpForce);
        currentJumpForce = jumpForce;
    }
}