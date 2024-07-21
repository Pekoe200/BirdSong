using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class birdController : MonoBehaviour {
    private Rigidbody2D mRigidbody2D;
    public float maxStamina = 100f;
    public float landingCheckDistance = 5f;
    public float quickLandingSpeed = 50f;
    public LayerMask groundLayerMask;

    [HideInInspector]
    public bool isGrounded = true;
    [HideInInspector]
    public bool isGliding = false;
    [HideInInspector]
    public bool isStalling = false;
    [HideInInspector]
    public bool isFacingRight = true;
    [HideInInspector]
    public bool isCharging = false;
    [HideInInspector]
    public float currentStamina;
    
    [HideInInspector]
    public float tiltAngle = 0f;

    public Slider staminaSlider;

    private birdWalking birdWalking;
    private birdJumping birdJumping;
    private birdGliding birdGliding;
    private birdQuickLanding birdQuickLanding;

    void Start()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();
        if (mRigidbody2D == null)
        {
            Debug.LogError("Rigidbody2D component missing from this game object");
        }

        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;

        birdWalking = GetComponent<birdWalking>();
        birdJumping = GetComponent<birdJumping>();
        birdGliding = GetComponent<birdGliding>();
        birdQuickLanding = GetComponent<birdQuickLanding>();
    }

    void Update()
    {
        if (isGrounded)
        {
            birdWalking.HandleWalking();
            birdJumping.HandleJumping();
        }
        else if (isGliding || isStalling)
        {
            birdGliding.HandleGliding();
        }
        else
        {
            birdGliding.CheckForGlide();
        }
        birdQuickLanding.HandleQuickLanding();
        UpdateStaminaUI();
    }

    public void UpdateStaminaUI()
    {
        staminaSlider.value = currentStamina;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isGliding = false;
            isStalling = false; 
            mRigidbody2D.drag = 0;
            tiltAngle = 0f; 
            transform.rotation = Quaternion.Euler(0, 0, 0); 
            Debug.Log("Landed on ground.");
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            isGliding = false;
            isStalling = false; 
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