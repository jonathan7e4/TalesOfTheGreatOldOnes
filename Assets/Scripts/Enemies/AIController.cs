using UnityEngine;

[ RequireComponent( typeof(Flanqueo) ) ]


public class AIController : MonoBehaviour
{
    Flanqueo flanqueoBehaviour;


    void Start()
    {
        flanqueoBehaviour = GetComponent<Flanqueo>();
    }


    void Update()
    {
        if ( flanqueoBehaviour.AroundPlayer() ) flanqueoBehaviour.ShootLogicUpdate();
        else flanqueoBehaviour.FollowPlayerLogicUpdate();
    }
}