using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flank))]
[RequireComponent(typeof(Heal))]

public class HealerAIController : MonoBehaviour
{
    Rigidbody2D rb;

    Heal heal;
    Flank flank;

    public float healingCooldown = 5f;
    [HideInInspector]
    public float lastHealingTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        heal = GetComponent<Heal>();
        flank = GetComponent<Flank>();

        heal.InitBehaviourData();
        flank.InitBehaviourData();
    }

    
    void Update()
    {
        HealingUpdateLogic();
    }

    void HealingUpdateLogic() {
        if (lastHealingTime >= healingCooldown)
        {
            heal.StartBehaviour();
            lastHealingTime = 0f;
        }

        lastHealingTime += Time.deltaTime;
    }


}
