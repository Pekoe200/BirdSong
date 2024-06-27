using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class birdControlsNew : MonoBehaviour
{
    private Rigidbody mRigidbody;
    public float walkSpeed = 5f;
    public float jumpForce = 5f;
    public float maxJumpForce = 15f;
    public float jumpChargeRate = 10f;
    public float maxStamina = 100f; // Maximum stamina value
    public float staminaDepletionRate = 1f; // Stamina depleted per unit jump force
    public float initialGlideSpeed = 20f; // Initial speed when starting to glide
    public float tiltDownSpeedIncrease = 30f; // Maximum speed when tilting down
    public float tiltUpSpeedDecrease = 10f; // Minimum speed when tilting up
    public float tiltUpHeightGain = 5f; // Altitude gained when tilting up
    public float stallSpeed = 10f; // Speed threshold below which the bird stalls
    public float maxTiltAngle = 90f; // Maximum tilt angle before stalling
    public float stallTime = 2f; // Time before stalling when pitched up above 90 degrees
    public float stallForwardSpeed = 3f; // Forward speed during stall
    private float currentStamina;
    private float currentJumpForce;
    private bool isGrounded = true;
    private bool isGliding = false;
    private float tiltAngle = 0f; // Current tilt angle
    private float stallTimer = 0f; // Timer for stalling

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
        else if (isGliding)
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
        float move = Input.GetAxis("Horizontal"); // Get input from A/D keys or Left/Right Arrow keys
        Vector3 moveDirection = new Vector3(move, 0, 0); // Move in the x direction
        mRigidbody.velocity = new Vector3(moveDirection.x * walkSpeed, mRigidbody.velocity.y, mRigidbody.velocity.z);
        Debug.Log("Walking. Move direction: " + moveDirection);
    }

    void HandleJumping()
    {
        if (Input.GetKey(KeyCode.Space) && currentStamina > 0)
        {
            // Increase the jump force while holding down the jump key, but stop if max jump force is reached
            if (currentJumpForce < maxJumpForce)
            {
                currentJumpForce += jumpChargeRate * Time.deltaTime;
                currentJumpForce = Mathf.Clamp(currentJumpForce, jumpForce, maxJumpForce);

                // Deplete stamina correspondingly
                float staminaToDeplete = staminaDepletionRate * Time.deltaTime;
                currentStamina -= staminaToDeplete;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

                Debug.Log("Charging jump. Current jump force: " + currentJumpForce);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && currentStamina > 0)
        {
            // Apply the jump force when the jump key is released
            mRigidbody.velocity = new Vector3(mRigidbody.velocity.x, currentJumpForce, mRigidbody.velocity.z);
            isGrounded = false;
            currentJumpForce = jumpForce; // Reset the jump force
            Debug.Log("Jumping with force: " + currentJumpForce);
        }
    }

    void CheckForGlide()
    {
        // Check if the bird is descending (peak height reached)
        if (mRigidbody.velocity.y <= 0)
        {
            isGliding = true;
            mRigidbody.velocity = transform.forward * initialGlideSpeed; // Start gliding with initial speed
            Debug.Log("Entering glide state with initial speed.");
        }
    }

    void HandleGliding()
    {
        float tiltInput = Input.GetAxis("Horizontal"); // Get input from A/D keys
        tiltAngle += tiltInput * Time.deltaTime * 50f; // Adjust tilt angle based on input
        tiltAngle = Mathf.Clamp(tiltAngle, -maxTiltAngle, maxTiltAngle); // Clamp tilt angle

        // Rotate the bird based on tilt angle
        transform.rotation = Quaternion.Euler(0, 0, -tiltAngle);

        // Calculate speed based on tilt angle
        float forwardSpeed = Mathf.Lerp(tiltUpSpeedDecrease, tiltDownSpeedIncrease, (tiltAngle + maxTiltAngle) / (2 * maxTiltAngle));

        // Set the velocity based on tilt angle
        mRigidbody.velocity = transform.right * forwardSpeed;

        // Check for stalling
        if (tiltAngle < 0)
        {
            stallTimer += Time.deltaTime;
            if (stallTimer >= stallTime)
            {
                isGliding = false;
                mRigidbody.velocity = new Vector3(stallForwardSpeed, -Mathf.Abs(mRigidbody.velocity.y), 0); // Stall and start falling
                Debug.Log("Stalled. Falling.");
                stallTimer = 0f; // Reset stall timer
            }
        }
        else
        {
            stallTimer = 0f; // Reset stall timer if not tilting up
        }

        Debug.Log("Gliding. Tilt angle: " + tiltAngle + ", Speed: " + forwardSpeed);
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
