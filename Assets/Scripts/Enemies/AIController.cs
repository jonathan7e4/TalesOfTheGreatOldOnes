using UnityEngine;

//[ RequireComponent( typeof(Flanqueo) ) ]


public class AIController : MonoBehaviour
{
    //Flanqueo flanqueoBehaviour;
    Heal healBehaviour;


    void Start()
    {
        //flanqueoBehaviour = GetComponent<Flanqueo>();
        healBehaviour = GetComponent<Heal>();

        healBehaviour.HealEnemies();

    }


    void Update()
    {
        //if ( flanqueoBehaviour.AroundPlayer() ) flanqueoBehaviour.ShootLogicUpdate();
        //else flanqueoBehaviour.FollowPlayerLogicUpdate();        
    }
}