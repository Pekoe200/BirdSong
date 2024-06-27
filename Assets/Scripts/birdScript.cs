using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdScript : MonoBehaviour
{
    private Animator mAnimator;
    private Rigidbody mRigidbody;
    private bool isGrounded = false;
    private bool isDiving = false;
    public float raycastDistance = 5.0f; // Distance to check for ground

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mRigidbody = GetComponent<Rigidbody>();
        mRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Debug.Log("birdScript initialized. Animator and Rigidbody components assigned.");
    }

    void Update()
    {
        if (isGrounded)
        {
            HandleWalking();
        }

        if (isDiving)
        {
            CheckForGroundProximity();
        }
    }

    public void StartDive()
    {
        mAnimator.SetTrigger("StartDive");
        mRigidbody.useGravity = true; // Enable gravity when diving
        isDiving = true;
        Debug.Log("StartDive triggered. Gravity enabled.");
    }

    public void Diving()
    {
        mAnimator.SetTrigger("Diving");
        Debug.Log("Diving triggered.");
    }

    public void Swoop()
    {
        mAnimator.SetTrigger("Swoop");
        mRigidbody.useGravity = false; // Disable gravity when swooping
        isDiving = false;
        Debug.Log("Swoop triggered. Gravity disabled.");
    }

    public void Ascend()
    {
        mAnimator.SetTrigger("Ascend");
        Debug.Log("Ascend triggered.");
    }

    public void EndSwoop()
    {
        mAnimator.SetTrigger("EndSwoop");
        Debug.Log("EndSwoop triggered.");
    }

    public void EndFlight()
    {
        mAnimator.SetTrigger("New State");
        Debug.Log("EndFlight triggered.");
    }

    void HandleWalking()
    {
        float move = Input.GetAxis("Horizontal");
        Vector3 moveDirection = new Vector3(move, 0, 0);
        transform.Translate(moveDirection * Time.deltaTime * 5); // Adjust speed as needed
        Debug.Log("Walking. Move direction: " + moveDirection);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isGrounded = false;
            mRigidbody.useGravity = false;
            mAnimator.SetTrigger("StartFlight");
            Debug.Log("StartFlight triggered. Gravity disabled.");
        }
    }

    void CheckForGroundProximity()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                Debug.Log("Ground detected close. Initiating landing sequence.");
                InitiateLandingSequence(hit.point);
            }
        }
    }

    void InitiateLandingSequence(Vector3 groundPoint)
    {
        isDiving = false;
        mRigidbody.velocity = Vector3.zero;
        mRigidbody.useGravity = true;
        transform.position = new Vector3(transform.position.x, groundPoint.y, transform.position.z);
        mAnimator.SetTrigger("Land");
        Debug.Log("Landing sequence initiated. Position set to ground, gravity enabled, velocity set to zero.");
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            mRigidbody.useGravity = true;
            mRigidbody.velocity = Vector3.zero;
            mAnimator.SetTrigger("Land");
            Debug.Log("Landed on ground. Gravity enabled, velocity set to zero.");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            mRigidbody.useGravity = true;
            mRigidbody.velocity = Vector3.zero;
            mAnimator.SetTrigger("Land");
            Debug.Log("Staying on ground. Gravity enabled, velocity set to zero.");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            mAnimator.SetTrigger("StartFlight");
            Debug.Log("Left ground. StartFlight triggered.");
        }
    }
}