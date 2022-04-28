using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Teleport))]
[RequireComponent(typeof(Dash))]
[RequireComponent(typeof(Flank))]

public class TeleportAIController : MonoBehaviour
{
    Rigidbody2D rb;
    Teleport teleport;
    Dash dash;
    Flank flank;

    public float teleportCooldownTime;
    public float lastTeleportTime;

    float waitingTime = 0.5f;

    [HideInInspector]
    public enum state
    {
        Dashing,
        Waiting,
        Teleporting,
        FollowingPlayer,
        WaitToTeleport
    }
    public state currentState = state.FollowingPlayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        teleport = GetComponent<Teleport>();
        dash = GetComponent<Dash>();
        flank = GetComponent<Flank>();

        teleport.InitBehaviourData();
        dash.InitBehaviourData();
        flank.InitBehaviourData();

        lastTeleportTime = 0f;        
    }

    void Update()
    {
        switch (currentState)
        {
            case state.Dashing:
                DashingUpdateLogic();
                break;
            case state.Waiting:
                WaitingUpdateLogic();
                break;
            case state.Teleporting:
                TeleportUpdateLogic();
                break;
            case state.FollowingPlayer:
                FollowPlayer();
                break;
            default:
                break;
        }
    }

    private void FollowPlayer()
    {
        flank.UpdateBehaviour();
        rb.velocity = Vector2.zero;

        if (flank.distanceToPlayer.magnitude <= flank.maxDistToPlayer) {
            flank.StopBehaviour();

            currentState = state.Teleporting;
        }

        lastTeleportTime += Time.deltaTime;
    }

    void TeleportUpdateLogic()
    {
        rb.velocity = Vector2.zero;
        if (lastTeleportTime >= teleportCooldownTime)
        {
            teleport.StartBehaviour();
            lastTeleportTime = 0f;

            currentState = state.Waiting;            
        }
        else
            lastTeleportTime += Time.deltaTime;
    }

    void WaitingUpdateLogic() {
        if (waitingTime <= 0f)
        {
            dash.StartBehaviour();

            waitingTime = 0.25f;
            currentState = state.Dashing;
        }
        else
            waitingTime -= Time.deltaTime;
    }

    void DashingUpdateLogic()
    {
        if (rb.velocity.magnitude == 0f)
        {
            if (flank.distanceToPlayer.magnitude <= flank.maxDistToPlayer)
                currentState = state.Teleporting;
            else
                currentState = state.FollowingPlayer;
        }
    }
}