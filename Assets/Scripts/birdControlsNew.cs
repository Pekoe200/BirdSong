using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class birdControlsNew : MonoBehaviour
{
    private Rigidbody2D mRigidbody2D;
    public float walkSpeed = 5f;
    public float jumpForce = 5f;
    public float maxJumpForce = 30f;
    public float jumpChargeRate = 15f;
    public float maxStamina = 100f; // Maximum stamina value
    public float initialGlideSpeed = 15; // Initial speed when starting to glide
    public float tiltDownSpeedIncrease = 30f; // Maximum speed when tilting down
    public float tiltUpSpeedDecrease = 10f; // Minimum speed when tilting up
    public float tiltUpHeightGain = 5f; // Altitude gained when tilting up
    public float stallSpeed = 10f; // Speed threshold below which the bird stalls
    public float maxTiltAngle = 30f; // Maximum tilt angle before stalling
    public float stallForwardSpeed = 5f; // Forward speed during stall
    private float currentStamina;
    private float currentJumpForce;
    private bool isGrounded = true;
    private bool isGliding = false;
    private bool isStalling = false; // Added isStalling variable
    private bool isFacingRight = true;
    private bool isCharging = false;
    private float tiltAngle = 0f; // Current tilt angle

    public Slider staminaSlider; // Reference to the UI Slider for stamina

    void Start()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();
        if (mRigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D component missing from this game object");
        }

        currentStamina = maxStamina; // Initialize current stamina to max stamina
        staminaSlider.maxValue = maxStamina; // Set the max value of the slider
        staminaSlider.value = currentStamina; // Set the initial value of the slider
    }

    void Update()
    {
        if (isGrounded)
        {
            HandleWalking();
            HandleJumping();
        }
        else if (isGliding || isStalling)
        {
            HandleGliding();
        }
        else
        {
            CheckForGlide();
        }
        UpdateStaminaUI();
    }

    void HandleWalking()
    {
        mRigidbody2D.gravityScale = 1; // Ensure gravity is on
        float move = Input.GetAxis("Horizontal"); // Get input from A/D keys or Left/Right Arrow keys
        Vector2 moveDirection = new Vector2(move, 0); // Move in the x direction

        if (!isCharging)
        {
            mRigidbody2D.velocity = new Vector2(moveDirection.x * walkSpeed, mRigidbody2D.velocity.y);
        }
        // Flip character based on walking direction
        if (move > 0 && !isFacingRight)
        {
            Flip();
        }
        if (move < 0 && isFacingRight)
        {
            Flip();
        }

        Debug.Log("Walking. Move direction: " + move);
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }

    void HandleJumping()
    {
        // Check if the jump key is being held down and stamina is available
        if (Input.GetKey(KeyCode.Space) && currentStamina > 0)
        {
            // Increase the jump force while holding down the jump key, but stop if max jump force is reached
            if (currentJumpForce < maxJumpForce)
            {
                isCharging = !isCharging;
                mRigidbody2D.velocity = new Vector2(0, 0);
                float forceIncrease = jumpChargeRate * Time.deltaTime;
                currentJumpForce += forceIncrease;
                currentJumpForce = Mathf.Clamp(currentJumpForce, jumpForce, maxJumpForce);

                // Deplete stamina proportionally to the increase in jump force
                float staminaToDeplete = forceIncrease;
                currentStamina -= staminaToDeplete;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

                // Shake the camera with intensity proportional to the jump force
                modCamera.Instance.ShakeCamera(currentJumpForce / maxJumpForce);

                // Change the FOV with intensity proportional to the jump force
                modCamera.Instance.ZoomCamera(currentJumpForce / maxJumpForce);

                Debug.Log("Charging jump. Current jump force: " + currentJumpForce + ", Stamina: " + currentStamina);
            }

            // If stamina depletes to zero during the charge, apply the jump force immediately
            if (currentStamina == 0)
            {
                ApplyJumpForce();            
                Debug.Log("Stamina depleted. Jumping with force: " + currentJumpForce);
            }

            // If bird attempts to move, cancel the jump
        }

        // Check if the jump key is released and there is some stamina left
        if (Input.GetKeyUp(KeyCode.Space) && currentJumpForce > jumpForce)
        {
            ApplyJumpForce();
            Debug.Log("Jumping with force: " + currentJumpForce);
        }

        // Ensure jump cannot be initiated if stamina is zero at the start
        if (Input.GetKey(KeyCode.Space) && currentStamina <= 0)
        {
            Debug.Log("Cannot initiate jump. Stamina depleted.");
        }
    }

    void ApplyJumpForce()
    {
        modCamera.Instance.ShakeCamera(0f);
        modCamera.Instance.ZoomCamera(0f);
        isCharging = !isCharging;
        mRigidbody2D.velocity = new Vector2(mRigidbody2D.velocity.x, currentJumpForce);
        isGrounded = false;
        currentJumpForce = jumpForce; // Reset the jump force
    }

    void CheckForGlide()
    {
        // Check if the bird is descending (peak height reached) and not already gliding or stalling
        if (mRigidbody2D.velocity.y <= 0 && !isGliding && !isStalling)
        {
            isGliding = true;
            mRigidbody2D.velocity = transform.right * initialGlideSpeed; // Start gliding with initial speed
            Debug.Log("Entering glide state with initial speed.");
        }
    }

    void HandleGliding()
    {
        float tiltInput = Input.GetAxis("Horizontal"); // Get input from A/D keys or Left/Right Arrow keys
        tiltAngle += tiltInput * Time.deltaTime * 100f; // Adjust tilt angle based on input
        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle); // Clamp tilt angle
        mRigidbody2D.drag = 0;

        // Rotate the bird based on tilt angle
        transform.rotation = Quaternion.Euler(0, 0, -tiltAngle);

        if (isStalling)
        {
            // If stalling, check if the tilt angle is greater than or equal to 0 to resume gliding
            if ((tiltAngle >= 0 && isFacingRight) || (tiltAngle <= 0 && !isFacingRight))
            {
                isStalling = false;
                isGliding = true;
                mRigidbody2D.gravityScale = 0; // Turn off gravity
                Debug.Log("Resuming glide state.");
            }
            else
            {
                // Continue falling with gravity
                mRigidbody2D.velocity = new Vector2(stallForwardSpeed, mRigidbody2D.velocity.y);
            }
        }
        else
        {
            float forwardSpeed = mRigidbody2D.velocity.magnitude; // Current forward speed

            if (isFacingRight)
            {
                if (tiltAngle > 0)
                {
                    // Accelerate while diving
                    forwardSpeed += (tiltAngle / maxTiltAngle) * (tiltDownSpeedIncrease - initialGlideSpeed) * Time.deltaTime;
                    // Clamp forward speed to the maximum value
                    forwardSpeed = Mathf.Min(forwardSpeed, 40f);
                }
                else if (tiltAngle < 0)
                {
                    // Decelerate while climbing
                    forwardSpeed -= (tiltAngle / -maxTiltAngle) * (initialGlideSpeed - (initialGlideSpeed / 2)) * 2 * Time.deltaTime;

                    // Trigger stall if speed drops to half of the default flight speed
                    if (forwardSpeed <= initialGlideSpeed / 2)
                    {
                        isGliding = false;
                        isStalling = true;
                        mRigidbody2D.gravityScale = 1; // Turn on gravity
                        mRigidbody2D.drag = 60; // Added high drag to simulate immediate fall
                        Debug.Log("Stalled. Falling.");
                        return;
                    }
                }
                else
                {
                    // Maintain default glide speed if angle is near parallel
                    forwardSpeed = Mathf.Lerp(forwardSpeed, initialGlideSpeed, Time.deltaTime);
                }
            }
            else
            {
                if (tiltAngle < 0)
                {
                    // Accelerate while diving (left)
                    forwardSpeed += (tiltAngle / -maxTiltAngle) * (tiltDownSpeedIncrease - initialGlideSpeed) * Time.deltaTime;
                    // Clamp forward speed to the maximum value
                    forwardSpeed = Mathf.Min(forwardSpeed, 40f);
                }
                else if (tiltAngle > 0)
                {
                    // Decelerate while climbing (left)
                    forwardSpeed -= (tiltAngle / maxTiltAngle) * (initialGlideSpeed - (initialGlideSpeed / 2)) * 2 * Time.deltaTime;

                    // Trigger stall if speed drops to half of the default flight speed
                    if (forwardSpeed <= initialGlideSpeed / 2)
                    {
                        isGliding = false;
                        isStalling = true;
                        mRigidbody2D.gravityScale = 1; // Turn on gravity
                        mRigidbody2D.drag = 60; // Added high drag to simulate immediate fall
                        Debug.Log("Stalled. Falling.");
                        return;
                    }
                }
                else
                {
                    // Maintain default glide speed if angle is near parallel
                    forwardSpeed = Mathf.Lerp(forwardSpeed, initialGlideSpeed, Time.deltaTime);
                }
            }

            // Determine the direction of gliding based on the bird's facing direction
            float glideDirection = isFacingRight ? 1 : -1;

            // Set the velocity based on calculated forward speed and direction
            mRigidbody2D.velocity = transform.right * forwardSpeed * glideDirection;

            Debug.Log("Gliding. Tilt angle: " + tiltAngle + ", Speed: " + forwardSpeed);
        }
    }   

    void UpdateStaminaUI()
    {
        staminaSlider.value = currentStamina; // Update the stamina slider value
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isGliding = false;
            isStalling = false; // Reset isStalling when landing
            mRigidbody2D.drag = 0;
            tiltAngle = 0f; // Reset tilt angle when landing
            transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation when landing
            Debug.Log("Landed on ground.");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Debug.Log("OnCollisionStay2D with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            isGliding = false;
            isStalling = false; // Reset isStalling when staying on ground
            Debug.Log("Staying on ground.");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("OnCollisionExit2D with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Left ground.");
        }
    }
}
