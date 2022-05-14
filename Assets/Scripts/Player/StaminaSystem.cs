using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{

    public static StaminaSystem instance;
    public float maxStamina = 100f;
    public float currentStamina;
    public float recovery = 5f;

    public float recoverCooldown = 1f;
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
                currentStamina = Mathf.Min(maxStamina, currentStamina + recovery * Time.deltaTime);
            else
                recoverTimer = Mathf.Min(recoverCooldown, recoverTimer + Time.deltaTime);

        }
        else
            recoverTimer = 0f;

        if (currentStamina <= 0f)
        {
            currentStamina = 0f;
            PlayerController.instance.exhausted = true;
        }

        if (currentStamina == maxStamina)
            PlayerController.instance.exhausted = false;
    }
}
