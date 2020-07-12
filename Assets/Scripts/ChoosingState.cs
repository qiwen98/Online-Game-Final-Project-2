using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;


public class ChoosingState : MonoBehaviourPunCallbacks, Istate
{
    public Transform ChooseState_Traps_spawnLocation;
    public Transform ChooseState_Players_spawnLocation;
    public GameObject[] traps_available_for_player;
    public float ChooseStateTimeLimit ;
    public Action CallBack;
    

    private float xdistance = 5f;
    private float timer;
    private GameObject[] remaining_traps;
    public int currentround = 0;

    public ExitGames.Client.Photon.Hashtable _customroomproperties = new ExitGames.Client.Photon.Hashtable();
    

    public void onStateEnter()
    {
        timer = 0;
        PrepareBattleAssets(PlayerPrefs.GetInt("MyCharacter"));

        //setting up all the room properties


        //if the room properties not set yet

        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_destination_Var"))
        {
            _customroomproperties.Add("Global_destination_Var", 0);
            _customroomproperties.Add("Round", 1);
            _customroomproperties.Add("Global_death_quantity", 0);

            Debug.Log("room properties created");
            PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);
            currentround = 1;

        }
        else
        {
            // all the player needs to know current round
            currentround = (int)PhotonNetwork.CurrentRoom.CustomProperties["Round"];
            currentround += 1;

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //only master client need to write the new round

                //reset the room properties to 0 because the round is renewed
                _customroomproperties["Global_destination_Var"] = 0;
                _customroomproperties["Round"] = currentround;
                _customroomproperties["Global_death_quantity"] = 0;


                //update the round
                PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);
                
                  
            }

            ////reset all the local player health to 3
            foreach (GameObject Player in GameManager.instance.Playerslist)
            {
                /* Player.GetComponent<PhotonView>().RPC("ResetHealth",
                           RpcTarget.AllBuffered,
                           Player.GetComponent<PhotonView>().Owner);
                          */
                Debug.Log("#Health reseted");

                Player.GetComponent<PhotonView>().RPC("ResetHealth",
                     RpcTarget.AllBuffered
                    );

            }





        }
        //display round
        TimerDisplay.instance.RoundDisplay(currentround);
        Debug.Log("Round" + currentround);


        //if master then spawn the traps
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            int number_of_trap_per_round=4;
             int current_round_trap_index =(currentround-1)*number_of_trap_per_round;

            for (int i=0;i< number_of_trap_per_round; i++)
            {
                PhotonNetwork.InstantiateSceneObject(traps_available_for_player[i+current_round_trap_index].name,
                   new Vector3(ChooseState_Traps_spawnLocation.position.x+(xdistance*i), ChooseState_Traps_spawnLocation.position.y,ChooseState_Traps_spawnLocation.position.z),
                    Quaternion.identity);
            }

           
        }

        



    }

    public void onStateUpdate()
    {
        timer += Time.deltaTime * 1;

        if(timer>=ChooseStateTimeLimit-2)
        {
            remaining_traps = GameObject.FindGameObjectsWithTag("TrapForChoose");

            //if player did not trigger with any, assign him random ramaining trap
            if (!PlayerBehaviour.instance.ChoosenTrap)
            {
                //for each gameobject with nametag trapassets, randomly pick one view id, 
                //destroy and assign it to gamemanager.instance.assigned prefab
                Debug.Log("assign random trap");

                int random_index = UnityEngine.Random.Range(0, remaining_traps.Length);
                if (remaining_traps == null)
                {
                    random_index = 1;
                }
                


                GameManager.instance.assignned_child_trap = (remaining_traps[random_index].GetPhotonView().ViewID % GameManager.instance.traps_available_for_player.Length) - 1;
                PlayerBehaviour.instance.ChoosenTrap = true;
                Debug.Log("assign random trap"+ GameManager.instance.assignned_child_trap);
            }

        }


        if (timer > ChooseStateTimeLimit)
        {
           
            

            foreach (GameObject Player in GameManager.instance.Playerslist)
            {
                Player.GetComponent<PlayerMovementController>().jungle_sound.SetActive(false);
                Player.GetComponent<PlayerMovementController>().desert_sound.SetActive(false);
                Player.GetComponent<PlayerMovementController>().ocean_sound.SetActive(false);
                Player.GetComponent<PlayerMovementController>().pond_sound.SetActive(false);
                Player.GetComponent<PlayerMovementController>().volcano_sound.SetActive(false);
                Player.GetComponent<PlayerMovementController>().ice_sound.SetActive(false);
                //halt the move
                if (Player.GetComponent<PlayerMovementController>() != null)
                {
                    // Player.GetComponent<PlayerMovementController>().speed = 0;
                    Player.GetComponent<PlayerMovementController>().enabled = false;
                    Player.GetComponent<PlayerBehaviour>().enabled = false;
                }


                //stop the camera
                Player.transform.GetChild(0).gameObject.SetActive(false);
            }

            //destroy all the prefab with tag.... //allow the delay of 1s
            if(PhotonNetwork.IsMasterClient)
            {
                foreach (GameObject remainingtraps in remaining_traps)
                {
                    if(remainingtraps.GetPhotonView().ViewID!=0)
                    {
                        PhotonNetwork.Destroy(remainingtraps.GetPhotonView());
                    }
                    
                    
                }

                   
            }
            CallBack();
        }

        else
        {
            //if player trigger with certain trap, record the view id down and spawn it later
           
            //make sure the player marked as choosen to avoid 2 choose
            
        //find this part in player behaviour.cs

           
       
        }

    }

    public void onFixedUpdate()
    {
        
    }
    
    public void onStateExit()
    {
       
    }

    private void PrepareBattleAssets(int whichprefab)
    {


        //if never spawn any player
        if (!GameManager.instance.player_spawned)
        {
            int randz = UnityEngine.Random.Range(0, 10);
            Vector3 spawnlocation = new Vector3(ChooseState_Players_spawnLocation.position.x + (xdistance * PhotonNetwork.LocalPlayer.ActorNumber),
                ChooseState_Players_spawnLocation.position.y+randz, 
                ChooseState_Players_spawnLocation.position.z);
            
           if (GameManager.instance.playerselection != null)
           {
              GameManager.instance.Playerslist.Add(PhotonNetwork.Instantiate(GameManager.instance.playerselection[whichprefab].name, spawnlocation, Quaternion.identity));

            }
           // //player spawned
           GameManager.instance.player_spawned = true;
        }
        else
        { //else no need spawn alrd but need to regain control

            foreach (GameObject Player in GameManager.instance.Playerslist)
            {
                //revoke the move
                Player.transform.position = ChooseState_Players_spawnLocation.position;
                // Player.GetComponent<PlayerMovementController>().speed=8;
                Player.GetComponent<PlayerMovementController>().enabled = true;
                Player.GetComponent<PlayerBehaviour>().enabled = true;

                //reset the reach destination
                Player.GetComponent<PlayerBehaviour>().reach_destination = false;
                //enable the camera
                Player.transform.GetChild(0).gameObject.SetActive(true);
                //mark player havent choose trap
                PlayerBehaviour.instance.ChoosenTrap = false;
            }


        }







    }

    public int RandomPlayer()
    {
        int Randomindex = UnityEngine.Random.Range(0, GameManager.instance.traps.Length);
        // Debug.Log(Randomindex);
        return Randomindex;
    }


}
