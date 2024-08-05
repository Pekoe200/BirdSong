using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class birdAbilities : MonoBehaviour {
    private birdController birdController;
    private birdStamina birdStamina; // Updated to reference BirdStamina
    private birdGliding birdGliding; // Reference to birdGliding
    private Rigidbody2D mRigidbody2D;
    private Animator animator;
    private float fixedDeltaTime;
    private Vector2 storedVelocity;
    public float flipStamina = 3f;
    public float dashStamina = 5f;
    public float timeStamina = 10f;
    public float speedBoostAmount = 10f;

    private BoostIndicator boostIndicator;
    private TimeIndicator timeIndicator;

    void Start() {
        birdController = GetComponentInParent<birdController>();
        birdStamina = GetComponentInParent<birdStamina>(); // Updated to reference BirdStamina
        birdGliding = GetComponentInParent<birdGliding>(); // Reference to birdGliding
        mRigidbody2D = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boostIndicator = FindObjectOfType<BoostIndicator>(); // Find the BoostIndicator in the scene
        timeIndicator = FindObjectOfType<TimeIndicator>(); // Find the TimeIndicator in the scene
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update() {
        if (((Input.GetKeyDown(KeyCode.A) && birdController.isFacingRight) || (Input.GetKeyDown(KeyCode.D) && !birdController.isFacingRight)) 
                    && birdController.isGliding && !birdController.isStalling) {
            if (!animator.GetBool("isFlipping")) {
                StartCoroutine(StartFlip());
            }
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            ApplyTimeSlow();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            RestartScene();
        }

        if (Input.GetKeyDown(KeyCode.Space) && birdController.isGliding) {
            ApplySpeedBoost();
        }
    }

    private IEnumerator StartFlip() {
        Debug.Log("Bird Flipping");
        storedVelocity = mRigidbody2D.velocity;
        mRigidbody2D.velocity = Vector2.zero;
        mRigidbody2D.gravityScale = 0;
        birdController.isFlipping = true;
        animator.SetBool("isFlipping", true);

        // Penalize the bird for flipping
        birdStamina.DecreaseStamina(flipStamina);

        yield return null; // Ensures the flip animation starts
    }

    // This method should be called at the end of the flip animation using an Animation Event
    public void FinishFlip() {
        Debug.Log("Flip over");
        birdController.isFacingRight = !birdController.isFacingRight;
        birdController.isFlipping = false;
        animator.SetBool("isFlipping", false);

        // Flip the parent object
        Transform parentTransform = transform.parent;
        Vector3 currentScale = parentTransform.localScale;
        currentScale.x *= -1;
        parentTransform.localScale = currentScale;

        // Transfer the bird's velocity to the other direction
        storedVelocity.x *= -1;
        mRigidbody2D.velocity = storedVelocity;
    }

    private void ApplyTimeSlow() {
        if (timeIndicator != null && timeIndicator.UseTimeAbility())
        {
            birdStamina.DecreaseStamina(timeStamina);
        }
    }

    private void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ApplySpeedBoost() {
        if (boostIndicator != null && boostIndicator.UseBoost())
        {
            birdGliding.ApplyBoost(speedBoostAmount);
            birdStamina.DecreaseStamina(dashStamina);
        }
    }
}
