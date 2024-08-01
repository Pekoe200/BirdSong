using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class birdAbilities : MonoBehaviour {
    private birdController birdController;
    private Rigidbody2D mRigidbody2D;
    private Animator animator;
    private float fixedDeltaTime;
    private Vector2 storedVelocity;

    void Start() {
        birdController = GetComponentInParent<birdController>();
        mRigidbody2D = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (!animator.GetBool("isFlipping")) {
                StartCoroutine(StartFlip());
            }
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            ToggleTimeScale();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            RestartScene();
        }
    }

    private IEnumerator StartFlip() {
        Debug.Log("Bird Flipping");
        storedVelocity = mRigidbody2D.velocity;
        mRigidbody2D.velocity = Vector2.zero;
        mRigidbody2D.gravityScale = 0;
        birdController.isFlipping = true;
        animator.SetBool("isFlipping", true);

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

    private void ToggleTimeScale() {
        if (Time.timeScale == 1.0f) {
            Time.timeScale = 0.4f;
        } else {
            Time.timeScale = 1.0f;
        }
        // Adjust fixed delta time according to timescale
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }

    private void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
