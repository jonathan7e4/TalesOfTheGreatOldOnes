using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepitoAIController : MonoBehaviour
{

    public enum State
    {
        ATTACKING,
        FLANQUEANDO,
        QUIETO
    }

    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void AttackingUpdate() { }
    void FlanqueandoUpdate() {

        //GetComponent<Flanqueo>().ExecuteUpdate();

        //if (GetComponent<ZicZac>().CanExecuteBehaviour())
        //{
        //    currentState = State.ATTACKING;
            
        //}

    
    }
    void QuietoUpdate() { }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case State.ATTACKING:
                AttackingUpdate();
                break;
            case State.FLANQUEANDO:
                FlanqueandoUpdate();
                break;
            case State.QUIETO:
                QuietoUpdate();
                break;
            default:
                break;
        }

    }
}
