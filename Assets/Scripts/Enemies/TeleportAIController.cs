using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Teleport))]
[RequireComponent(typeof(Dash))]
[RequireComponent(typeof(Flank))]

public class TeleportAIController : MonoBehaviour
{
    public static TeleportAIController instance;

    Teleport teleport;
    Dash dash;
    Flank flank;

    public float damage = 30f;
    public float attackCooldown = 2f;
    public float attackCooldownTimer;

    public int consecutiveAttacks = 4;
    public int consecutiveAttacksCounter;

    public bool bonked;

    [HideInInspector]
    public enum State
    {
        Dashing,
        Waiting,
        Attack,
        FollowingPlayer,
        Resting
    }
    public State currentState = State.Resting;


    void Start()
    {
        instance = this;

        teleport = GetComponent<Teleport>();
        dash = GetComponent<Dash>();
        flank = GetComponent<Flank>();

        teleport.InitBehaviourData();
        dash.InitBehaviourData();
        flank.InitBehaviourData();

        attackCooldownTimer = 0f;
        consecutiveAttacksCounter = consecutiveAttacks;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Dashing:
                DashingUpdateLogic();
                break;
            case State.Resting:
                RestingUpdateLogic();
                break;
            case State.Attack:
                Attack();
                break;
            case State.FollowingPlayer:
                FollowPlayer();
                break;
            default:
                break;
        }
    }

    private void FollowPlayer()
    {
        flank.UpdateBehaviour();

        if (attackCooldownTimer <= 0f && flank.onRange()) {
            flank.StopBehaviour();

            currentState = State.Resting;
        }
        else
            attackCooldownTimer -= Time.deltaTime;
    }

    void RestingUpdateLogic() {
        flank.UpdateDistanceToPlayer();

        if (!flank.onRange() && !bonked)
        {
            currentState = State.FollowingPlayer;
        }
        else if (attackCooldownTimer <= 0f)
        {
            bonked = false;

            attackCooldownTimer = attackCooldown;
            consecutiveAttacksCounter = consecutiveAttacks;
            currentState = State.Attack;
        }
        else
            attackCooldownTimer -= Time.deltaTime;
    }

    void Attack()
    {
        consecutiveAttacksCounter--;

        Vector2 initialPosition = transform.position;

        teleport.StartBehaviour();

        if ((Vector2)transform.position != initialPosition)
            dash.StartBehaviour();

        currentState = State.Dashing;
    }

    void DashingUpdateLogic()
    {
        if (!dash.dashing)
        {
            if (flank.distanceToPlayer.magnitude > flank.maxDistToPlayer)
                currentState = State.FollowingPlayer;
            else if (consecutiveAttacksCounter == 0)
                currentState = State.Resting;
            else
                currentState = State.Attack;
        }
    }

    public void InterruptDash()
    {
        dash.StopBehaviour();

        attackCooldownTimer = 0.5f;
        consecutiveAttacksCounter = 0;

        currentState = State.Resting;

        bonked = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && dash.dashing) {
            PlayerController.instance.lifeSystem.TakeDamage(damage);

            Shake.instance.ShakeIt();

            dash.dashing = false;
            dash.StopBehaviour();
        }

        if (collision.gameObject.layer == 3 && dash.dashing) {
            InterruptDash();
        }
    }
}