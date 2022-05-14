using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{

    public static StaminaSystem instance;
    public float maxStamina;
    public float currentStamina;
    public float recovery;

    public float recoverCooldown;
    public float recoverTimer;

    [HideInInspector]
    public float staminaDebuff;

    private void Start()
    {
        currentStamina = maxStamina;
        instance = this;
        staminaDebuff = 1f;
    }

    private void Update()
    {
        if (PlayerController.instance.currentState == PlayerController.state.Normal)
        {
            if (recoverTimer >= recoverCooldown)
            {
                currentStamina = Mathf.Min(maxStamina, currentStamina + recovery * Time.deltaTime);
            }
            else
            {
                recoverTimer = Mathf.Min(recoverCooldown, recoverTimer + Time.deltaTime);
            }

        }
        else {
            recoverTimer = 0f;
        }

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            staminaDebuff = 0.5f;
        }

        if (currentStamina == maxStamina)
            staminaDebuff = 1f;
    }
}
