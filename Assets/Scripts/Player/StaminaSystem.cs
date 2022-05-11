using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{

    public static StaminaSystem instance;
    public float maxStamina;
    public float currentStamina;
    public float recovery;

    [HideInInspector]
    public bool canRecover;

    private void Start()
    {
        currentStamina = maxStamina;
        instance = this;
    }

    private void Update()
    {
        if(PlayerController.instance.currentState == PlayerController.state.Normal)
            currentStamina = Mathf.Min(maxStamina, currentStamina + recovery * Time.deltaTime);
    }
}
