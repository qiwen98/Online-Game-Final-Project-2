using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Istate 
{
    void onStateEnter();
    void onStateUpdate();
    void onFixedUpdate();
    void onStateExit();
}
