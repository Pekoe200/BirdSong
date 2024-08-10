using UnityEngine;
using UnityEngine.UI;

public class TimeIndicator : MonoBehaviour
{
    [SerializeField] private Image grayscaleImage; // The grayscale background image
    [SerializeField] private Image fullColorImage; // The full color foreground image
    [SerializeField] private float slowTimeDuration = 3f; // Duration of the slow time effect
    [SerializeField] private float rechargeTime = 5f; // Time for the ability to recharge
    [SerializeField] private AudioClip slowTimeSound; // Sound effect for slowing time
    [SerializeField] private AudioClip resumeTimeSound; // Sound effect for resuming time

    private float rechargeTimer;
    private bool isTimeAbilityAvailable = true;
    private bool isTimeSlowed = false;
    public float timeScale = .5f;
    private AudioSource audioSource; // Audio source to play sound effects

    void Start()
    {
        if (grayscaleImage == null || fullColorImage == null)
        {
            Debug.LogError("Time Indicator images not assigned!");
        }

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1.5f;
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject.");
        }

        ResetTimeIndicator();
    }

    void Update()
    {
        if (isTimeSlowed)
        {
            rechargeTimer += Time.unscaledDeltaTime;
            float fillAmount = 1f - Mathf.Clamp01(rechargeTimer / slowTimeDuration);
            fullColorImage.fillAmount = fillAmount;

            if (fillAmount <= 0f)
            {
                isTimeSlowed = false;
                Time.timeScale = 1f;
                rechargeTimer = 0f;

                // Play the resume time sound effect
                if (audioSource != null && resumeTimeSound != null)
                {
                    audioSource.PlayOneShot(resumeTimeSound);
                    
                }
            }
        }
        else if (!isTimeAbilityAvailable)
        {
            rechargeTimer += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(rechargeTimer / rechargeTime);
            fullColorImage.fillAmount = fillAmount;

            if (fillAmount >= 1f)
            {
                isTimeAbilityAvailable = true;
                fullColorImage.fillAmount = 1f;
            }
        }
    }

    public bool UseTimeAbility()
    {
        if (isTimeAbilityAvailable)
        {
            isTimeAbilityAvailable = false;
            isTimeSlowed = true;
            rechargeTimer = 0f;
            fullColorImage.fillAmount = 1f;
            Time.timeScale = timeScale; // Adjust as necessary for the slow motion effect

            // Play the slow time sound effect
            if (audioSource != null && slowTimeSound != null)
            {
                audioSource.PlayOneShot(slowTimeSound);
            }

            return true;
        }
        return false;
    }

    public bool IsTimeSlowed()
    {
        return isTimeSlowed;
    }

    private void ResetTimeIndicator()
    {
        fullColorImage.fillAmount = 1f;
    }
}
