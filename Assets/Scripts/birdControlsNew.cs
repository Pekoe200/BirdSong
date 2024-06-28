using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class birdControlsNew : MonoBehaviour
{
    private Rigidbody mRigidbody;
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
    private float tiltAngle = 0f; // Current tilt angle

    public Slider staminaSlider; // Reference to the UI Slider for stamina

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        if (mRigidbody == null)
        {
            Debug.LogError("Rigidbody component missing from this game object");
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
        mRigidbody.useGravity = true; // Ensure gravity is on
        float move = Input.GetAxis("Horizontal"); // Get input from A/D keys or Left/Right Arrow keys
        Vector3 moveDirection = new Vector3(move, 0, 0); // Move in the x direction
        mRigidbody.velocity = new Vector3(moveDirection.x * walkSpeed, mRigidbody.velocity.y, mRigidbody.velocity.z);
        Debug.Log("Walking. Move direction: " + moveDirection);
    }

    void HandleJumping()
    {
        // Check if the jump key is being held down and stamina is available
        if (Input.GetKey(KeyCode.Space) && currentStamina > 0)
        {
            // Increase the jump force while holding down the jump key, but stop if max jump force is reached
            if (currentJumpForce < maxJumpForce)
            {
                float forceIncrease = jumpChargeRate * Time.deltaTime;
                currentJumpForce += forceIncrease;
                currentJumpForce = Mathf.Clamp(currentJumpForce, jumpForce, maxJumpForce);

                // Deplete stamina proportionally to the increase in jump force
                float staminaToDeplete = forceIncrease;
                currentStamina -= staminaToDeplete;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

                Debug.Log("Charging jump. Current jump force: " + currentJumpForce + ", Stamina: " + currentStamina);
            }

            // If stamina depletes to zero during the charge, apply the jump force immediately
            if (currentStamina == 0)
            {
                ApplyJumpForce();
                Debug.Log("Stamina depleted. Jumping with force: " + currentJumpForce);
            }
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
        mRigidbody.velocity = new Vector3(mRigidbody.velocity.x, currentJumpForce, mRigidbody.velocity.z);
        isGrounded = false;
        currentJumpForce = jumpForce; // Reset the jump force
    }

    void CheckForGlide()
    {
        // Check if the bird is descending (peak height reached) and not already gliding or stalling
        if (mRigidbody.velocity.y <= 0 && !isGliding && !isStalling)
        {
            isGliding = true;
            mRigidbody.velocity = transform.forward * initialGlideSpeed; // Start gliding with initial speed
            Debug.Log("Entering glide state with initial speed.");
        }
    }

    void HandleGliding()
    {
        float tiltInput = Input.GetAxis("Horizontal"); // Get input from A/D keys
        tiltAngle += tiltInput * Time.deltaTime * 100f; // Adjust tilt angle based on input
        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle); // Clamp tilt angle
        mRigidbody.drag = 0;

        // Rotate the bird based on tilt angle
        transform.rotation = Quaternion.Euler(0, 0, -tiltAngle);

        if (isStalling)
        {
            // If stalling, check if the tilt angle is greater than or equal to 0 to resume gliding
            if (tiltAngle >= 0)
            {
                isStalling = false;
                isGliding = true;
                mRigidbody.useGravity = false; // Turn off gravity
                Debug.Log("Resuming glide state.");
            }
            else
            {
                // Continue falling with gravity
                mRigidbody.velocity = new Vector3(stallForwardSpeed, mRigidbody.velocity.y, 0);
            }
        }
        else
        {
            float forwardSpeed = mRigidbody.velocity.magnitude; // Current forward speed

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
                // NOTE: An arbitrary 2 is added to the mult here as a deceleration factor.
                forwardSpeed -= (tiltAngle / -maxTiltAngle) * (initialGlideSpeed - (initialGlideSpeed / 2)) * 2 * Time.deltaTime;

                // Trigger stall if speed drops to half of the default flight speed
                if (forwardSpeed <= initialGlideSpeed / 2)
                {
                    isGliding = false;
                    isStalling = true;
                    mRigidbody.useGravity = true; // Turn on gravity
                    mRigidbody.drag = 60; // Added high drag to simulate immediate fall
                    Debug.Log("Stalled. Falling.");
                    return;
                }
            }
            else
            {
                // Maintain default glide speed if angle is near parallel
                forwardSpeed = Mathf.Lerp(forwardSpeed, initialGlideSpeed, Time.deltaTime);
            }

            // Set the velocity based on calculated forward speed
            mRigidbody.velocity = transform.right * forwardSpeed;

            Debug.Log("Gliding. Tilt angle: " + tiltAngle + ", Speed: " + forwardSpeed);
        }
    }



    void UpdateStaminaUI()
    {
        staminaSlider.value = currentStamina; // Update the stamina slider value
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isGliding = false;
            isStalling = false; // Reset isStalling when landing
            mRigidbody.drag = 0;
            tiltAngle = 0f; // Reset tilt angle when landing
            transform.rotation = Quaternion.Euler(0, 0, 0); // Reset rotation when landing
            Debug.Log("Landed on ground.");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            isGliding = false;
            isStalling = false; // Reset isStalling when staying on ground
            Debug.Log("Staying on ground.");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            Debug.Log("Left ground.");
        }
    }
}
