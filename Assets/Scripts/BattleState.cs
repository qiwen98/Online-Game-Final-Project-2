using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class BattleState : MonoBehaviourPunCallbacks,Istate
{
    public Transform BattleStateStartingPoint;
    public Transform BattleStateDestination;
    public float BattleState_Timer;
    public float BattleState_TimeLimit;
    private const byte RECORD_SEQUENCE_EVENT=0;
    public float roundlimit;
   


    public Action WinCallBack;
    public Action LostCallBack;
    

    public ExitGames.Client.Photon.Hashtable _customroomproperties = new ExitGames.Client.Photon.Hashtable();

    public void onStateEnter()
    {
        BattleState_Timer = 0;

      //prepare this state assets
        PrepareBattleAssets(RandomPlayer());

        // means first rounf and not set the room properties yet
        /*
       if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_destination_Var"))
        {
            _customroomproperties.Add("Global_destination_Var", 0);
            _customroomproperties.Add("Round", 1);
           
            Debug.Log("room properties created");
            PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);

        }
       else
        {
            // all the player needs to know current round
            currentround = (int)PhotonNetwork.CurrentRoom.CustomProperties["Round"];
            currentround += 1;

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                //only master client need to ammend

                //reset the room properties to 0 because the round is renewed
                _customroomproperties["Global_destination_Var"] = 0;
                _customroomproperties["Round"] = currentround;
            //update the round
            PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);
            Debug.Log("Round"+currentround);
            }
           

        }
       */

    }

    private void PrepareBattleAssets(int whichprefab)
    {

        
        //if never spawn any player
        if (!GameManager.instance.player_spawned)
        {

            // spawn the new proper players
            //for (int i = 0; i < 4; i++)
            //{
            //    Playerslist.Add(GameObject.Instantiate(RealplayerPrefab, BattleStateStartingPoint.position, Quaternion.identity) as GameObject);

            GameManager.instance.Playerslist.Add(PhotonNetwork.Instantiate(GameManager.instance.playerselection[whichprefab].name, BattleStateStartingPoint.position, Quaternion.identity));
            //    Playerslist[i].name = "player_" + i;
            //    // Playerslist[0].setActive(false); //This sets the first healthpack inactive. And works on either arrays or List.
            //}

            //player spawned
            GameManager.instance.player_spawned = true;
        }
        else
        { //else no need spawn alrd but need to regain control

            foreach (GameObject Player in GameManager.instance.Playerslist)
            {
                int randz = UnityEngine.Random.Range(0, 10);
                Vector3 spawnlocation = new Vector3(BattleStateStartingPoint.position.x+(5 * PhotonNetwork.LocalPlayer.ActorNumber),
                    BattleStateStartingPoint.position.y+randz,
                    BattleStateStartingPoint.position.z );

                //revoke the move
                Player.transform.position = spawnlocation;
               // Player.GetComponent<PlayerMovementController>().speed=8;
                Player.GetComponent<PlayerMovementController>().enabled = true;
                Player.GetComponent<PlayerBehaviour>().enabled = true;

                //reset the reach destination
                Player.GetComponent<PlayerBehaviour>().reach_destination = false;
                //enable the camera
                Player.transform.GetChild(0).gameObject.SetActive(true);
            }

           
        }

       





    }

   

    public void onStateUpdate()
    {
        BattleState_Timer += Time.deltaTime * 1;
        TimerDisplay.instance.battleSTimer = BattleState_Timer;
        //if within timer


        // if time limit exceed
        if (BattleState_Timer>BattleState_TimeLimit)
        {

            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_destination_Var"))
            {
                if (((int)PhotonNetwork.CurrentRoom.CustomProperties["Round"] >= roundlimit))

                {
                    WinCallBack();
                    BattleState_Timer = 0;
                }

                else
                {

                    LostCallBack();
                }

            }

            //foreach (GameObject Player in GameManager.instance.Playerslist)
            //{

            //    //halt the move
            //    if (Player.GetComponent<PlayerMovementController>()!=null)
            //    {
            //        // Player.GetComponent<PlayerMovementController>().speed = 0;
            //        Player.GetComponent<PlayerMovementController>().enabled = false;
            //    }


            //    //stop the camera
            //    Player.transform.GetChild(0).gameObject.SetActive(false);
            //}


            //exit this state and prepare go to lose state





        }
        else
        {
            //else 
            

            foreach (GameObject Player in GameManager.instance.Playerslist)
            {
                //if one of players reach destination
                if ((Player.transform.position - BattleStateDestination.position).magnitude < 2f)
                {

                    Debug.Log("reach destination");
                    //record player reached destination
                    if(Player.GetComponent<PlayerBehaviour>().reach_destination==false && Player.GetComponent<PhotonView>().IsMine)
                    {

                        Player.GetComponent<PlayerBehaviour>().reach_destination = true;

                        //determine the player sequence
                        int current_sequence=(int) PhotonNetwork.CurrentRoom.CustomProperties["Global_destination_Var"];
                        current_sequence += 1;
                        _customroomproperties["Global_destination_Var"] = current_sequence;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);
                        Debug.Log("current_sequence"+current_sequence);


                        //add the reach destination score
                        float destination_obj_score = ScoreManager.instance.destination_objective_point;
                        //add the sequence score
                        float destination_seq_score = ScoreManager.instance.OnDestinationReachedScore(current_sequence);
                        Player.GetComponent<PhotonView>().RPC("AddScore", 
                            RpcTarget.AllBuffered,
                            destination_obj_score+ destination_seq_score,
                            Player.GetComponent<PhotonView>().Owner);


                        // set the player  reach destination and  global sequence to player's own sequence variable
                        Player.GetComponent<PhotonView>().RPC("SetreachDestination", 
                            RpcTarget.AllBuffered,
                            true ,
                            (float) current_sequence ,
                            Player.GetComponent<PhotonView>().Owner);

                        
                       
                        //if(currentround>=roundlimit&&current_sequence== )
                        //{
                        //    //tell other ppl, all the player reach the destination // and go to result state 
                        //    _customroomproperties["Finished"] = true;
                        //    //update the round
                        //    PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);

                        //}

                    }

                   
                }
                //if currentround== round limit && all player reached
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_destination_Var"))
                {           // when everyone reached
                    if (
                        ((int)PhotonNetwork.CurrentRoom.CustomProperties["Global_destination_Var"] == PhotonNetwork.PlayerList.Length)
                       &&
                        ((int)PhotonNetwork.CurrentRoom.CustomProperties["Round"] >= roundlimit)
                    
                       )

                    {
                      WinCallBack();
                        BattleState_Timer = 0;
                    }

                    if (
                       ((int)PhotonNetwork.CurrentRoom.CustomProperties["Global_destination_Var"] == PhotonNetwork.PlayerList.Length)
                       &&
                       ((int)PhotonNetwork.CurrentRoom.CustomProperties["Round"] < roundlimit)

                      )

                    {
                        LostCallBack();
                        BattleState_Timer = 0;
                    }

                    int deathpeople=(int)PhotonNetwork.CurrentRoom.CustomProperties["Global_death_quantity"];
                    int reachdestination=(int)PhotonNetwork.CurrentRoom.CustomProperties["Global_destination_Var"];

                    if (deathpeople+reachdestination>= PhotonNetwork.PlayerList.Length&&
                        (int)PhotonNetwork.CurrentRoom.CustomProperties["Round"] < roundlimit)
                    {
                        LostCallBack();
                        BattleState_Timer = 0;
                    }

                    if (deathpeople + reachdestination >= PhotonNetwork.PlayerList.Length &&
                       (int)PhotonNetwork.CurrentRoom.CustomProperties["Round"] >= roundlimit)
                    {
                        WinCallBack();
                        BattleState_Timer = 0;
                    }
                }
                
                
                // if all people die
                if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Global_death_quantity"))
                {
                    if(
                        ((int)PhotonNetwork.CurrentRoom.CustomProperties["Global_death_quantity"]== PhotonNetwork.PlayerList.Length)
                        &&
                       ((int)PhotonNetwork.CurrentRoom.CustomProperties["Round"] < roundlimit)
                       )
                    {
                        LostCallBack();
                    }
                }


                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Score"))
                {
                    //string currentscore=;


                    if (int.Parse(PhotonNetwork.LocalPlayer.CustomProperties["Score"].ToString()) >= ScoreManager.instance.scoreLimit)


                    {

                        
                        _customroomproperties["Global_destination_Var"] =PhotonNetwork.PlayerList.Length  ;
                       
                        _customroomproperties["Round"] = 10;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);
                      
                        Debug.Log("Finished");
                    }



                }




                }



            }




    }

    public void onFixedUpdate()
    {
    }

    public void onStateExit()
    {
       // CallBack();

    }

    public int RandomPlayer()
    {
        int Randomindex = UnityEngine.Random.Range(0, GameManager.instance.traps.Length);
       // Debug.Log(Randomindex);
        return Randomindex;
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        Debug.Log("the room prop >>" + propertiesThatChanged + ">>has changed");
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private  void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
       if(obj.Code==RECORD_SEQUENCE_EVENT)
        {
            object[] data = (object[])obj.CustomData;
            float id = (float)data[0];
            
        }
    }
}
// pass the id to the rank
// object[] id_data = new object[] { id };
// PhotonNetwork.RaiseEvent(RECORD_SEQUENCE_EVENT, id_data, RaiseEventOptions.Default, SendOptions.SendReliable);