using UnityEngine;
using UnityEngine.UI;

public class TimeIndicator : MonoBehaviour
{
    [SerializeField] private Image grayscaleImage; // The grayscale background image
    [SerializeField] private Image fullColorImage; // The full color foreground image
    [SerializeField] private float slowTimeDuration = 3f; // Duration of the slow time effect
    [SerializeField] private float rechargeTime = 5f; // Time for the ability to recharge

    private float rechargeTimer;
    private bool isTimeAbilityAvailable = true;
    private bool isTimeSlowed = false;
    public float timeScale = .5f;

    void Start()
    {
        if (grayscaleImage == null || fullColorImage == null)
        {
            Debug.LogError("Time Indicator images not assigned!");
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
