using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Platformer.Mechanics;

public class PlatformerJumpPad : MonoBehaviour
{
    public float verticalVelocity;

    void OnTriggerEnter (Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb == null) return;
        //var player = rb.GetComponent<PlayerController>();
        var player = rb.GetComponent<PlayerMovementController>();
        if (player == null) return;
        

        player.rb.velocity += new Vector3(0, 50, 0);
    }

    
}
