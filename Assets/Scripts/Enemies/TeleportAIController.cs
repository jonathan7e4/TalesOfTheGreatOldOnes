using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Teleport))]
[RequireComponent(typeof(Dash))]

public class TeleportAIController : MonoBehaviour
{
    Rigidbody2D rb;
    Teleport teleport;
    Dash dash;

    public float teleportCooldownTime;
    public float lastTeleportTime;

    float waitingTime = 0.5f;

    enum state
    {
        Dashing,
        Waiting,
        Teleporting
    }
    state currentState = state.Teleporting;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        teleport = GetComponent<Teleport>();
        dash = GetComponent<Dash>();

        teleport.InitBehaviourData();
        dash.InitBehaviourData();

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
            default:
                break;
        }
    }

    void TeleportUpdateLogic()
    {
        if (lastTeleportTime >= teleportCooldownTime)
        {
            teleport.StartBehaviour();
            lastTeleportTime = 0f;

            currentState = state.Waiting;            
        }
        else
        {
            lastTeleportTime += Time.deltaTime;
        }
    }

    void WaitingUpdateLogic() {
        if (waitingTime <= 0f)
        {
            dash.StartBehaviour();

            waitingTime = 0.25f;
            currentState = state.Dashing;
        }
        else
        {
            waitingTime -= Time.deltaTime;
        }
    }

    void DashingUpdateLogic()
    {
        if (rb.velocity.magnitude == 0f)
            currentState = state.Teleporting;
    }
}