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

    Rigidbody2D rb;
    Teleport teleport;
    Dash dash;
    Flank flank;

    float attackCooldown;
    public float attackCooldownTimer;

    int consecutiveAttacks;
    public int consecutiveAttacksCounter;

    [HideInInspector]
    public enum State
    {
        Dashing,
        Waiting,
        Attack,
        FollowingPlayer,
        Resting
    }
    public State currentState = State.FollowingPlayer;


    void Start()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
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

        if (flank.distanceToPlayer.magnitude <= flank.maxDistToPlayer) {
            flank.StopBehaviour();

            currentState = State.Resting;
        }
        else
            attackCooldownTimer -= Time.deltaTime;
    }

    void RestingUpdateLogic() {
        flank.UpdateDistanceToPlayer();

        if (flank.distanceToPlayer.magnitude > flank.maxDistToPlayer)
        {
            currentState = State.FollowingPlayer;
        }
        else if (attackCooldownTimer <= 0f)
        {
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

        teleport.StartBehaviour();
        dash.StartBehaviour();

        currentState = State.Dashing;
    }

    void DashingUpdateLogic()
    {
        if (rb.velocity.magnitude == 0f)
        {
            if (flank.distanceToPlayer.magnitude > flank.maxDistToPlayer)
                currentState = State.FollowingPlayer;
            else if (consecutiveAttacksCounter == 0)
                currentState = State.Resting;
            else
                currentState = State.Attack;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player") {
            //rb.velocity = Vector2.zero;
        }
    }
}