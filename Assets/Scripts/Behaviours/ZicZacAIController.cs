using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Flanqueo))]
[RequireComponent(typeof(ZicZac))]


public class ZicZacAIController : MonoBehaviour
{
   enum State
    {
        Attack,
        Flank
    }

    State currentState = State.Flank;


    ZicZac ziczac;
    Flanqueo flank;


    private void Start()
    {
        ziczac = GetComponent<ZicZac>();
        flank = GetComponent<Flanqueo>();


        ziczac.InitBehaviourData();
        flank.InitBehaviourData();


        flank.StartBehaviour();
    }


    private void Update()
    {
        switch ( currentState )
        {
            case State.Attack:
                ziczac.UpdateBehaviour();

                if (/* debo pasar de atacar a flanquear*/)
                {
                    currentState = State.Flank;
                    ziczac.StopBehaviour();
                    flank.StartBehaviour();
                }


                break;
            case State.Flank:
                flank.UpdateBehaviour();

                if (/* debo pasar de flanquear atacar */)
                {
                    currentState = State.Attack;
                    flank.StopBehaviour();
                    ziczac.StartBehaviour();
                }

                break;
        }
    }
}