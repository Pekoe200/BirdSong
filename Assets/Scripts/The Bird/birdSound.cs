using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdSound : MonoBehaviour
{
    private birdController birdController;
    private AudioSource audioSource;
    private AudioSource walkingAudioSource;
    private AudioSource flippingAudioSource;
    private Rigidbody2D mRigidBody2D;
    public AudioClip glidingSound;
    public AudioClip walkingSound;
    public AudioClip flippingSound;
    public float minSpeed = 0f;
    public float maxSpeed = 40f;
    public float volumeSmoothing = 2f; // Smoothing factor for volume adjustment
    public float walkingSoundDelay = 0.3f; // Delay before the walking sound starts

    private float targetVolume = 0f;
    private Coroutine walkingSoundCoroutine;
    private bool hasPlayedFlipSound = false;

    void Start()
    {
        birdController = GetComponent<birdController>();
        mRigidBody2D = GetComponent<Rigidbody2D>();

        // Set up the gliding sound AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = glidingSound;
        audioSource.loop = true;
        audioSource.volume = 0f;

        // Set up the walking sound AudioSource
        walkingAudioSource = gameObject.AddComponent<AudioSource>();
        walkingAudioSource.clip = walkingSound;
        walkingAudioSource.loop = true;
        walkingAudioSource.volume = 0f; // Start with the volume at 0

        // Set up the flipping sound AudioSource
        flippingAudioSource = gameObject.AddComponent<AudioSource>();
        flippingAudioSource.clip = flippingSound;
        flippingAudioSource.loop = false; // Play the flipping sound only once per flip
    }

    void Update()
    {
        HandleGlidingSound();
        HandleWalkingSound();
        HandleFlippingSound();
    }

    void HandleGlidingSound()
    {
        if (!birdController.isGrounded && mRigidBody2D.velocity.magnitude > 0 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (birdController.isGrounded)
        {
            targetVolume = 0f;
            if (audioSource.volume <= 0.01f) // Stop the sound when volume is almost zero
            {
                audioSource.Stop();
            }
        }

        if (!birdController.isGrounded && mRigidBody2D.velocity.magnitude > 0)
        {
            AdjustVolumeBasedOnSpeed();
        }

        // Smoothly transition the volume in all cases
        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, volumeSmoothing * Time.deltaTime);
    }

    void HandleWalkingSound()
    {
        if (birdController.isGrounded && Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            if (walkingSoundCoroutine == null)
            {
                walkingSoundCoroutine = StartCoroutine(StartWalkingSoundWithDelay());
            }
        }
        else
        {
            if (walkingSoundCoroutine != null)
            {
                StopCoroutine(walkingSoundCoroutine);
                walkingSoundCoroutine = null;
            }
            walkingAudioSource.Stop();
        }
    }

    IEnumerator StartWalkingSoundWithDelay()
    {
        yield return new WaitForSeconds(walkingSoundDelay);

        if (birdController.isGrounded && Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            walkingAudioSource.Play();
            walkingAudioSource.volume = 1f; // Set volume for walking sound
            walkingAudioSource.pitch = 1.25f;
        }
    }

    void HandleFlippingSound()
    {
        if (birdController.isFlipping && !hasPlayedFlipSound)
        {
            flippingAudioSource.Play();
            hasPlayedFlipSound = true;
        }
        else if (!birdController.isFlipping)
        {
            hasPlayedFlipSound = false; // Reset for the next flip
        }
    }

    void AdjustVolumeBasedOnSpeed()
    {
        float speed = mRigidBody2D.velocity.magnitude;
        float normalizedSpeed = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
        targetVolume = Mathf.Lerp(0f, 1f, normalizedSpeed);
    }
}
