using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class PreBuildState : MonoBehaviourPunCallbacks,Istate 
{
   public Transform spawnLocation;
    public GameObject playerTransparentPrefab;
    public GameObject[] traps;
    public Action CallBack;
   
    
    public float buildStateTimeLimit ;
    private float timer;
   // private int currentround = 0;




   // public ExitGames.Client.Photon.Hashtable _customroomproperties = new ExitGames.Client.Photon.Hashtable();


    public void onStateEnter()
    {
        
        ////setting up all the room properties


        ////if the room properties not set yet

        //if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_destination_Var"))
        //{
        //    _customroomproperties.Add("Global_destination_Var", 0);
        //    _customroomproperties.Add("Round", 1);
        //    _customroomproperties.Add("Global_death_quantity",0);

        //    Debug.Log("room properties created");
        //    PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);

        //}
        //else
        //{
        //    // all the player needs to know current round
        //    currentround = (int)PhotonNetwork.CurrentRoom.CustomProperties["Round"];
        //    currentround += 1;

        //    if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //    {
        //        //only master client need to write the new round

        //        //reset the room properties to 0 because the round is renewed
        //        _customroomproperties["Global_destination_Var"] = 0;
        //        _customroomproperties["Round"] = currentround;
        //        _customroomproperties["Global_death_quantity"] = 0;

        //        //update the round
        //        PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);
                
        //    }


        //    //reset all the local player health to 3
        //    foreach (GameObject Player in GameManager.instance.Playerslist)
        //    {
        //        Player.GetComponent<PhotonView>().RPC("ResetHealth",
        //                   RpcTarget.AllBuffered,    
        //                   Player.GetComponent<PhotonView>().Owner);
        //       // Debug.Log("Health reseted");
        //    }



        //        Debug.Log("Round" + currentround);
        //}

        timer = 0;

     




    }
    
   

  


    public void onFixedUpdate()
    {
       
    }

    public void onStateUpdate()
    {
        timer += Time.deltaTime * 1;
        //if whithin 30s 
        //allow them move to certain location
        if (timer > buildStateTimeLimit)
        {

            


            // instruct all the object player to stop moving
            foreach (GameObject objectPly in GameManager.instance.objectPlayers)
            {
                //halt the move

               // objectPly.GetComponent<ObjMovementController>().speed = 0;
               if(objectPly!=null)
                {
                    //destoy the objmovement script
                    if (objectPly.GetComponent<ObjMovementController>() != null)
                    {
                        Destroy(objectPly.GetComponent<ObjMovementController>());
                    }

                    //stop the camera
                    objectPly.transform.GetChild(0).gameObject.SetActive(false);
                }
               

               
            }

            //if state finnished 
            //callback
            CallBack();

        }


       
    }

    public void onStateExit()
    {

       
      

    }

    //[RPC]
    //void InstantiateFlame()
    //{
    //    fire = (GameObject)Instantiate(flameBaby, transform.position + transform.forward, transform.rotation);

    //    //this is the part that is giving me trouble.
    //    fire.transform.parent = myGuy.transform;
    //}

}
