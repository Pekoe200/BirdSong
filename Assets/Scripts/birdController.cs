using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class birdController : MonoBehaviour {
    private Rigidbody2D mRigidbody2D;
    public float maxStamina = 100f;
    public float landingCheckDistance = 5f;
    public float quickLandingSpeed = 50f;
    public float groundCheckDistance = 0.2f; // Distance for the proximity check
    public float groundProximityThreshold = 0.1f; // Threshold for proximity check
    public LayerMask groundLayerMask; // Layer mask for ground detection

     
    public bool isGrounded = true;
     
    public bool isGliding = false;
     
    public bool isStalling = false;
     
    public bool isFacingRight = true;
     
    public bool isCharging = false;
     
    public float currentStamina;
     
    public float tiltAngle = 0f;
     
    public bool hasJumped = false;

    public Slider staminaSlider;

    private birdWalking birdWalking;
    private birdJumping birdJumping;
    private birdGliding birdGliding;
    private birdQuickLanding birdQuickLanding;
    private Animator animator;

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
//        CheckGroundProximity();
    }

/*    void CheckGroundProximity() {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayerMask);
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.red);
        
        if (hit.collider != null) {
            Debug.Log("Ground proximity check hit: " + hit.collider.name + " at distance: " + hit.distance);
            if (Mathf.Abs(hit.distance) < groundProximityThreshold) {
                isGrounded = true;
                animator.SetBool("isGrounded", true);
                isGliding = false;
                isStalling = false;
                mRigidbody2D.drag = 0;
                tiltAngle = 0f;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log("Re-grounded due to proximity check.");
            }
        } else {
            Debug.Log("Ground proximity check did not hit any ground.");
        }
    }

*/

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
        if (collision.gameObject.CompareTag("Ground")) {
            Vector2 contactNormal = collision.contacts[0].normal;
            if (contactNormal.y > 0) {
                // Bird is landing from above
                if (!isGrounded || mRigidbody2D.velocity.y <= 0) {
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
                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            Vector2 contactNormal = collision.contacts[0].normal;
            if (contactNormal.y > 0 && !isGrounded) {
                // Bird is staying on ground from above
                isGrounded = true;
                animator.SetBool("isGrounded", true);
                isGliding = false;
                animator.SetBool("isGliding", false);
                isStalling = false;
                animator.SetBool("isStalling", false);
                Debug.Log("Staying on ground.");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        Debug.Log("OnCollisionExit2D with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
            Debug.Log("Left ground.");
        }
    }

}
