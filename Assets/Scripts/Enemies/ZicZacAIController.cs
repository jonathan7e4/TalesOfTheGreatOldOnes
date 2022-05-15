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

    enum State { DoingFlank, DoingZicZac }
    State currentState = State.DoingFlank;

    Flank flank;
    ZicZac zicZac;

    public float damage;


    void Start()
    {
        flank = GetComponent<Flank>();
        zicZac = GetComponent<ZicZac>();


        flank.InitBehaviourData();
        zicZac.InitBehaviourData();


        flank.StartBehaviour();
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


    private void DoFlank()
    {
        flank.UpdateBehaviour();

        timeCounterPostAttack += Time.deltaTime;

        if (timeCounterPostAttack >= timeToAttackAgain && zicZac.CanExecuteBehaviour())
        {
            currentState = State.DoingZicZac;
            flank.StopBehaviour();
            zicZac.StartBehaviour();
        }
    }


    void Update()
    {
        flank.UpdateDistanceToPlayer();

        switch ( currentState )
        {
            case State.DoingFlank:

                DoFlank();

                break;

            case State.DoingZicZac:

                DoZicZac();

                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Shake.instance.ShakeIt();
            PlayerController.instance.lifeSystem.TakeDamage(damage);
        }
    }
}