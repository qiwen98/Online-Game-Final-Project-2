using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameState : MonoBehaviour
{
    float time = 0;
    GameObject[] players;
    Rigidbody rb;


    private void Awake()
    {

        if (players == null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }

       


    }

    // Start is called before the first frame update
    void Start()
    {

        foreach (GameObject player in players)
        {
            rb = player.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }



        time = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime * 1;

        //if time is 10 , then player stop movement 

        if(time>20)
         {
            // now freeze
            foreach (GameObject player in players)
            {
                rb = player.GetComponent<Rigidbody>();
                ObjMovementController playercontroller = player.GetComponent<ObjMovementController>();
                playercontroller.speed = 0;
                
            }
            Debug.Log("now post game");
        }

    }

    void freeze()
    {
        //now players cannot move
    }

}
