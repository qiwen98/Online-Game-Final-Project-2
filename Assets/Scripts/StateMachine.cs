using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Istate currentState;

    public void changeState(Istate _state)
    {
        if(currentState!=null)
        {
            currentState.onStateExit();
        }

        currentState = _state;
        currentState.onStateEnter();
    }

    public void UpdateState()
    {
        currentState.onStateUpdate();
    }

    public void FixedUpdateState()
    {
        currentState.onFixedUpdate();
    }
}
