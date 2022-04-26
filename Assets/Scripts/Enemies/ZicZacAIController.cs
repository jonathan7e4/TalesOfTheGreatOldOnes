using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent( typeof(Flank) ) ]
[ RequireComponent( typeof(ZicZac) ) ]


public class ZicZacAIController : MonoBehaviour
{
    public const float MAX_TIME_TO_ATTACK_AGAIN = 10f;
    public const float MIN_TIME_TO_ATTACK_AGAIN = 3f;

    public float timeCounterPostAttack = 0f;
    public float timeToAttackAgain = 3f;

    enum State { DoingZicZac, DoingFlank }
    State currentState = State.DoingFlank;

    ZicZac zicZac;
    Flank flank;


    private void Start()
    {
        zicZac = GetComponent<ZicZac>();
        flank = GetComponent<Flank>();


        zicZac.InitBehaviourData();
        flank.InitBehaviourData();


        flank.StartBehaviour();
    }


    private void DoFlank()
    {
        flank.UpdateBehaviour();

        timeCounterPostAttack += Time.deltaTime;

        if ( timeCounterPostAttack >= timeToAttackAgain && zicZac.CanExecuteBehaviour() )
        {
            currentState = State.DoingZicZac;
            flank.StopBehaviour();
            zicZac.StartBehaviour();
        }
    }


    private void DoZicZac()
    {
        zicZac.UpdateBehaviour();

        if ( zicZac.Finished() )
        {
            timeToAttackAgain = Random.Range( MIN_TIME_TO_ATTACK_AGAIN, MAX_TIME_TO_ATTACK_AGAIN );
            timeCounterPostAttack = 0f;

            currentState = State.DoingFlank;
            zicZac.StopBehaviour();
            flank.StartBehaviour();
        }
    }


    private void Update()
    {
        switch ( currentState )
        {
            case State.DoingZicZac:

                DoZicZac();

                break;

            case State.DoingFlank:

                DoFlank();

                break;
        }
    }
}