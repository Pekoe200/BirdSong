using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class birdController : MonoBehaviour {
    private Rigidbody2D mRigidbody2D;
    public float maxStamina = 100f;
    public float landingCheckDistance = 5f;
    public float quickLandingSpeed = 50f;
    public float groundCheckDistance = 0.6f; // Distance for the proximity check
    public float groundProximityThreshold = 0.1f; // Threshold for proximity check
    public LayerMask groundLayerMask; // Layer mask for ground detection

    public bool isGrounded = false;
    public bool isGliding = false;
    public bool isStalling = false;
    public bool isFacingRight = true;
    public bool isCharging = false;
    public bool isFlipping = false;
    public float currentStamina;
    public float tiltAngle = 0f;
    public bool hasJumped = false;

    public Slider staminaSlider;

    public bool shouldStayGrounded = false;
    private birdWalking birdWalking;
    private birdJumping birdJumping;
    private birdGliding birdGliding;
    private birdQuickLanding birdQuickLanding;
    private Animator animator;

    private Coroutine groundCheckCoroutine;
    private const float groundCheckDelay = 0.2f; // Small delay for ground check

    void Start() {
        mRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        if (mRigidbody2D == null) {
            Debug.LogError("Rigidbody2D component missing from this game object");
        }

        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;

        birdWalking = GetComponent<birdWalking>();
        birdJumping = GetComponent<birdJumping>();
        birdGliding = GetComponent<birdGliding>();
        birdQuickLanding = GetComponent<birdQuickLanding>();

        // Initialize the groundLayerMask
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    void Update() {
        if (isGrounded && !hasJumped) {
            birdWalking.HandleWalking();
            birdJumping.HandleJumping();
        } else if (isGliding || isStalling) {
            birdGliding.HandleGliding();
        } else {
            birdGliding.CheckForGlide();
        }
        birdQuickLanding.HandleQuickLanding();
        UpdateStaminaUI();
    }

    public void IncreaseStamina(float amount) {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateStaminaUI();
    }

    public void UpdateStaminaUI() {
        staminaSlider.value = currentStamina;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("OnCollisionEnter2D with: " + collision.gameObject.name);
        shouldStayGrounded = true;
        mRigidbody2D.gravityScale = 1;
        if (!isGrounded) {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            isGliding = false;
            animator.SetBool("isGliding", false);
            isStalling = false;
            animator.SetBool("isStalling", false);
            mRigidbody2D.drag = 0;
            tiltAngle = 0f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Bird landed on ground.");
            hasJumped = false; // Only reset hasJumped when the bird truly lands
            animator.SetBool("hasJumped", false);
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        mRigidbody2D.gravityScale = 1;
        if (!hasJumped) {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
        isGliding = false;
        animator.SetBool("isGliding", false);
        isStalling = false;
        animator.SetBool("isStalling", false);
        Debug.Log("Staying on ground.");
    }

    void OnCollisionExit2D(Collision2D collision) {
        Debug.Log("OnCollisionExit2D with: " + collision.gameObject.name);
        if (groundCheckCoroutine != null) {
            StopCoroutine(groundCheckCoroutine);
            Debug.Log("Coroutine Stopped");
        }
        groundCheckCoroutine = StartCoroutine(CheckIfGroundedAfterDelay());
        Debug.Log("Coroutine Started");
    }

    private IEnumerator CheckIfGroundedAfterDelay() {
        yield return new WaitForSeconds(groundCheckDelay);

        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayerMask);
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.red);

        if (hit.collider != null && Mathf.Abs(hit.distance) < groundProximityThreshold && shouldStayGrounded) {
            // The bird is still near the ground, don't set isGrounded to false
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            Debug.Log("Re-grounded due to proximity check.");
        } else {
            // The bird is no longer near the ground
            isGrounded = false;
            animator.SetBool("isGrounded", false);
            Debug.Log("Left ground.");
        }
    }
}
