using UnityEngine;
using UnityEngine.UI;

public class BoostIndicator : MonoBehaviour
{
    [SerializeField] private Image grayscaleImage; // The grayscale background image
    [SerializeField] private Image fullColorImage; // The full color foreground image
    [SerializeField] private float cooldownTime = 5f; // Time for the boost to recharge

    private float cooldownTimer;
    private bool isBoostAvailable = true;

    void Start()
    {
        if (grayscaleImage == null || fullColorImage == null)
        {
            Debug.LogError("Boost Indicator images not assigned!");
        }

        ResetBoostIndicator();
    }

    void Update()
    {
        if (!isBoostAvailable)
        {
            cooldownTimer += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(cooldownTimer / cooldownTime);
            fullColorImage.fillAmount = fillAmount;

            if (fillAmount >= 1f)
            {
                isBoostAvailable = true;
                fullColorImage.fillAmount = 1f;
            }
        }
    }

    public bool UseBoost()
    {
        if (isBoostAvailable)
        {
            isBoostAvailable = false;
            cooldownTimer = 0f;
            fullColorImage.fillAmount = 0f;
            // Add your boost logic here (e.g., Apply speed boost to the bird)
            return true;
        }
        return false;
    }

    private void ResetBoostIndicator()
    {
        fullColorImage.fillAmount = 1f;
    }
}
