using UnityEngine;

[ RequireComponent( typeof(Flank) ) ]
[ RequireComponent( typeof(Shoot) ) ]


public class ArcherAIController : MonoBehaviour
{
    enum State { DoingFlank, Shooting }
    State currentState = State.DoingFlank;

    Flank flank;
    Shoot shoot;


    void Start()
    {
        flank = GetComponent<Flank>();
        shoot = GetComponent<Shoot>();


        flank.InitBehaviourData();
        shoot.InitBehaviourData();


        flank.StartBehaviour();
    }


    void Shoot()
    {
        shoot.UpdateBehaviour();

        if ( !PositionUtils.AroundPlayer( flank.distanceToPlayer, flank.maxDistToPlayer, flank.minDistToPlayer ) )
        {
            currentState = State.DoingFlank;

            shoot.StopBehaviour();
            flank.StartBehaviour();
        }
    }


    void DoFlank()
    {
        flank.UpdateBehaviour();

        if ( PositionUtils.AroundPlayer( flank.distanceToPlayer, flank.maxDistToPlayer, flank.minDistToPlayer ) )
        {
            currentState = State.Shooting;

            flank.StopBehaviour();
            shoot.StartBehaviour();
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

            case State.Shooting:

                Shoot();

                break;
        }
    }
}