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


    void Update()
    {
        switch ( currentState )
        {
            case State.DoingFlank:

                flank.UpdateBehaviour();

                if ( flank.AroundPlayer() )
                {
                    currentState = State.Shooting;

                    flank.StopBehaviour();
                    shoot.StartBehaviour();
                }

                break;

            case State.Shooting:

                shoot.ShootLogicUpdate();

                if ( true) // !flank.AroundPlayer()
                {
                    currentState = State.DoingFlank;

                    shoot.StopBehaviour();
                    flank.StartBehaviour();
                }

                break;
        }
    }
}