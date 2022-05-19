using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flank))]
public class SkeletonAIController : MonoBehaviour
{
    enum State { DoingFlank, Attacking }
    State currentState = State.DoingFlank;

    public GameObject animatedObject;

    Rigidbody2D rb;
    Animator animator;

    Flank flank;
    AIMeleeAttack melee;

    public float damage = 10;

    public float attackCooldown= 1f;
    public float attackCooldownCounter;

    void Start()
    {
        melee = GetComponent<AIMeleeAttack>();
        flank = GetComponent<Flank>();
        rb = GetComponent<Rigidbody2D>();

        flank.InitBehaviourData();

        flank.maxDistToPlayer = melee.attackDistance + GetComponent<CircleCollider2D>().radius * transform.localScale.x;
    }

    void Update()
    {
        flank.UpdateDistanceToPlayer();

        switch (currentState)
        {
            case State.DoingFlank:
                DoFlank();
                break;
            case State.Attacking:
                Attack();
                break;
        }
    }

    void DoFlank()
    {
        flank.UpdateBehaviour();

        if (PositionUtils.AroundPlayer(flank.distanceToPlayer, flank.maxDistToPlayer, flank.minDistToPlayer))
        {
            currentState = State.Attacking;

            flank.StopBehaviour();
            melee.StartBehaviour();
        }
    }

    void Attack()
    {
        attackCooldownCounter += Time.deltaTime;

        if (attackCooldownCounter >= attackCooldown)
        {
            melee.StartBehaviour();
            attackCooldownCounter = 0f;
        }

        if (!PositionUtils.AroundPlayer(flank.distanceToPlayer, flank.maxDistToPlayer, flank.minDistToPlayer))
        {
            currentState = State.DoingFlank;

            flank.StartBehaviour();
        }

    }
}
