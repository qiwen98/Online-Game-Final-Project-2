using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stone_player : MonoBehaviour
{
    public PlayerMovementController Player;
    public float jumpForce = 1800;
    public float timeBeforeNextJump = 1.8f;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovementController.Stone(jumpForce, timeBeforeNextJump);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerMovementController.Stone(jumpForce, timeBeforeNextJump);

        }
    }
}
