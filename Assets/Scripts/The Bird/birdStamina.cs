using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdStamina : MonoBehaviour
{
    private birdController birdController;
    private birdGliding birdGliding;
    private Animator animator;

    void Start()
    {
        birdController = GetComponent<birdController>();
        birdGliding = GetComponent<birdGliding>();
        animator = GetComponentInChildren<Animator>();
        if (birdController == null)
        {
            Debug.LogError("birdController component missing from this game object");
        }
    }

    public void ApplyDamage(int damage)
    {
        float staminaLoss = -damage; // Convert damage to negative value for reducing stamina
        birdGliding.EnterStall();
        
        IncreaseStamina(staminaLoss);
    }

    public void IncreaseStamina(float amount)
    {
        birdController.currentStamina += amount;
        birdController.currentStamina = Mathf.Clamp(birdController.currentStamina, 0, birdController.maxStamina);
        birdController.UpdateStaminaUI();
    }

    public void DecreaseStamina(float amount)
    {
        IncreaseStamina(-amount);
    }
}
