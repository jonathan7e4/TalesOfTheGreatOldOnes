using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealerFlank))]
[RequireComponent(typeof(Heal))]
[RequireComponent(typeof(AILifeSystem))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class HealerAIController : MonoBehaviour
{
    Heal heal;
    HealerFlank healerFlank;

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
        healerFlank = GetComponent<HealerFlank>();

        heal.InitBehaviourData();
        healerFlank.InitBehaviourData();

        currentState = State.FollowingEnemy;
        healerFlank.StartBehaviour();
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
        healerFlank.UpdateBehaviour();

        if (healerFlank.followingPath && healerFlank.onRange())
        {
            healerFlank.StopBehaviour();

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
        healerFlank.UpdateDistances();

        if (!healerFlank.onRange())
        {
            currentState = State.FollowingEnemy;
            healerFlank.StartBehaviour();
        }

        if (lastHealingTime >= healingCooldown)
            currentState = State.Healing;
        else
            lastHealingTime += Time.deltaTime;
    }
}
