using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealerFlank))]
[RequireComponent(typeof(Heal))]

public class HealerAIController : MonoBehaviour
{
    Heal heal;
    HealerFlank flank;

    public float healingCooldown = 5f;
    public float lastHealingTime = 0f;

    [HideInInspector]
    public enum State
    {
        FollowingEnemy,
        Waiting,
        Healing,
    }
    public State currentState;

    void Start()
    {
        heal = GetComponent<Heal>();
        flank = GetComponent<HealerFlank>();

        heal.InitBehaviourData();
        flank.InitBehaviourData();

        currentState = State.FollowingEnemy;
        flank.StartBehaviour();
    }

    
    void Update()
    {
        switch (currentState)
        {
            case State.FollowingEnemy:
                FollowEnemyLogic();
                break;
            case State.Waiting:
                UpdateLogic();
                break;
            case State.Healing:
                Heal();
                break;
        }        
    }

    private void FollowEnemyLogic()
    {
        flank.UpdateBehaviour();

        if (flank.distanceToPlayer.magnitude <= flank.maxDistToPlayer || !flank.hasEnemiesAround)
        {
            flank.StopBehaviour();

            currentState = State.Waiting;
        }
        else
            lastHealingTime -= Time.deltaTime;
    }

    void Heal()
    {
        heal.StartBehaviour();
        lastHealingTime = 0f;

        currentState = State.Waiting;
    }

    void UpdateLogic()
    {
        flank.UpdateDistanceToPlayer();

        if (flank.distanceToPlayer.magnitude > flank.maxDistToPlayer && flank.hasEnemiesAround)
        {
            currentState = State.FollowingEnemy;
            flank.StartBehaviour();
        }
        
        if (lastHealingTime >= healingCooldown)
            currentState = State.Healing;
        else
            lastHealingTime += Time.deltaTime;
    }
}
