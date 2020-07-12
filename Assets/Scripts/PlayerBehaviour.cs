using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;

public class PlayerBehaviour : MonoBehaviourPunCallbacks
{
    public float health;
    public float startHealth = 3;
    public float currentscore;
    public bool reach_destination;
    public float destination_sequence;
    bool isfalling = false;
    public Rigidbody rb;
    float currentplayertimer = 0f;
    float timeout = 0f;
    public bool ChoosenTrap = false;
    public float scoretoadd;
    public bool getpushed = false;

    public static PlayerBehaviour instance;

    public ExitGames.Client.Photon.Hashtable _customproperties= new ExitGames.Client.Photon.Hashtable();
    public ExitGames.Client.Photon.Hashtable _customroomproperties = new ExitGames.Client.Photon.Hashtable();

    private void Awake()
    {
        if (instance != null)
        {
           // Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = startHealth;
        currentscore = 0;   // or System assingned score;
        destination_sequence = 0; // 0 represent havent reach, large than one means the sequence
        reach_destination = false;
        scoretoadd = 0;

        // initialise key for each players
        _customproperties.Add("Health",health);
        _customproperties.Add("Score", currentscore);
        _customproperties.Add("Des_sequence", destination_sequence);
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(_customproperties);
        Debug.Log(" first nitialise score success");

    }

    // Update is called once per frame
    void Update()
    {

        currentplayertimer += 1 * Time.deltaTime;
        //always refresh the death ppl quantity
        


    }
    void mute()
    {
        HealthManagerUI.instance.soundleft.SetActive(false);
    }
    private void FixedUpdate()
    {
         float DisstanceToTheGround = GetComponent<Collider>().bounds.extents.y;

        RaycastHit ray;

         bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, out ray, DisstanceToTheGround + 0.1f);

         //Debug.DrawRay(transform.position, Vector3.down * DisstanceToTheGround , Color.red);
         //Debug.Log(DisstanceToTheGround);


         if (IsGrounded)
         {
            // Debug.Log("is gorunded");
             isfalling = false;
             // Anim.SetBool("in_the_air", false);
         }
         else
         {
           //  Debug.Log("is not gorunded");
             isfalling = true;
             // Anim.SetBool("in_the_air", true);
             //rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
         }


         //condition 1 , if is falling and distance to the ground >100, then means below have nothings die

         //if (ray.distance > 20)
         //{
         //    photonView.RPC("TakeDamage",
         //                    RpcTarget.AllBuffered,
         //                   1f);


         //}
          

        //condition 2, get squeeze down by other player and is falling== true, then also die  
    }

    [PunRPC]
    public void TakeDamage(float _damage, Player target)
    {
        health = (float)target.CustomProperties["Health"];
        health -= _damage;
        HealthManagerUI.instance.soundleft.SetActive(true);
        Invoke("mute", 2f);
        if (target.IsLocal)
        {
            HealthManagerUI.instance.anim.SetTrigger("LowHealth");
        }

        //HealthManagerUI.instance.soundleft.SetActive(true);
        //Invoke("mute", 2f);
        if (health <= 0f)
        {
            //Die
            Die();
            health = 0;
            _customproperties["Health"] = health;
            target.SetCustomProperties(_customproperties);
        }

        else
        {

            Invoke("Respawn",1.3f);
            _customproperties["Health"] = health;
           target.SetCustomProperties(_customproperties);
        }

        
    }

    [PunRPC]
    public void RecoverHealth(float _heart, Player target)
    {
        health = (float)target.CustomProperties["Health"];
        health += _heart;

        if (health <= 3f)
        {
            _customproperties["Health"] = health;
            target.SetCustomProperties(_customproperties);
        }
    }

    public void Respawn()
    {
        Debug.Log("get hurt and wait for going b to starting point");

        Vector3 spawnpoint = new Vector3(GameManager.instance.BattleStateStartingPoint.position.x + (5 * PhotonNetwork.LocalPlayer.ActorNumber),
                                 GameManager.instance.BattleStateStartingPoint.position.y, GameManager.instance.BattleStateStartingPoint.position.z);
        transform.position =spawnpoint ;
    }

    [PunRPC]
    public void AddScore(float _score, Player Targetplayerscore )
    {
        currentscore = (float)Targetplayerscore.CustomProperties["Score"];
        ScoreManager.instance.HoverScoreUI(Targetplayerscore.ActorNumber,_score);
        currentscore += _score;
        _customproperties["Score"] = currentscore;

       

        Targetplayerscore.SetCustomProperties(_customproperties);
        
       // Debug.Log(currentscore);


    }

    [PunRPC]
    public void SetreachDestination(bool yes, float sequence_number, Player Targetplayerscore)
    {
        reach_destination = yes;
        _customproperties["Des_sequence"] = sequence_number;
        Targetplayerscore.SetCustomProperties(_customproperties);

    }

    [PunRPC]
    public void ResetHealth()
    {
        
            if(photonView.IsMine)
            {
               float playerhealth = (float)PhotonNetwork.LocalPlayer.CustomProperties["Health"];
                playerhealth = startHealth;
                _customproperties["Health"] = playerhealth;
              PhotonNetwork.LocalPlayer.SetCustomProperties(_customproperties);
            }
           
        
        
       
    }

    void Die()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Health"))
        {
            
                int current_quantity_of_deathPlayer = (int)PhotonNetwork.CurrentRoom.CustomProperties["Global_death_quantity"];
                current_quantity_of_deathPlayer += 1;
                _customroomproperties["Global_death_quantity"] = current_quantity_of_deathPlayer;
                PhotonNetwork.CurrentRoom.SetCustomProperties(_customroomproperties);
            
        }

        if (photonView.IsMine)
        {
            // PixelGunGameManager.instance.LeaveRoom();
            foreach (GameObject Player in GameManager.instance.Playerslist)
            {

                //halt the move
                if (Player.GetComponent<PlayerMovementController>() != null)
                {
                    // Player.GetComponent<PlayerMovementController>().speed = 0;
                    Player.GetComponent<PlayerMovementController>().enabled = false;
                }


                //stop the camera
                Player.transform.GetChild(0).gameObject.SetActive(false);
            }
            Debug.Log("you re dead");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("collided" + other.gameObject.tag +"and with name" +other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "TrapForChoose":

                if(!ChoosenTrap)
                {
                    GameManager.instance.assignned_child_trap = (other.gameObject.GetPhotonView().ViewID % GameManager.instance.traps_available_for_player.Length) - 1;
                    //transfer the ownership to me first, because only owner of object can destroy
                    TransferOwnershipRequest(other.gameObject.GetPhotonView(), PhotonNetwork.LocalPlayer);

                    //if owenership is mine then destroy
                    if (other.gameObject.GetPhotonView().IsMine)
                    {
                        PhotonNetwork.Destroy(other.gameObject.GetPhotonView());
                       //  Debug.Log("transfer sucess,can destroy");
                    }
                    ChoosenTrap = true;
                }
                break;

            case "damage":
                if (photonView.IsMine && !getpushed)
                {
                    // Debug.Log("damage is true");

                    photonView.RPC("TakeDamage",
                                    RpcTarget.AllBuffered,
                                   1f, PhotonNetwork.LocalPlayer);

                }


                break;

            case "bullet":

                break;
        }

        switch (other.gameObject.name)
        {
            case "Vornado_VFAN(Clone)":
                //Debug.Log("collidedfan");
                if(other.gameObject.transform.parent!=null)
                {
                   // Debug.Log("updatescore");
                    UpdateScore(other.gameObject, ScoreManager.instance.bigfanScore);
                }
            
                break;

            case "Bullet(Clone)":
                //Debug.Log("collidedbullet");
                if (other.gameObject.transform.parent != null)
                {
                    // Debug.Log("updatescore");
                    UpdateScore(other.gameObject, ScoreManager.instance.gunScore);
                }

                if (photonView.IsMine)
                {
                    //Debug.Log("Bullet is true");
                    // UpdateScore(other.gameObject, ScoreManager.instance.gunScore);


                    photonView.RPC("TakeDamage",
                                    RpcTarget.AllBuffered,
                                    1f, PhotonNetwork.LocalPlayer);

                }

                break;
        }
       
    }

    public void AddHealth()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RecoverHealth",
                                RpcTarget.AllBuffered,
                                1f, PhotonNetwork.LocalPlayer);
        }
    }

    private void TransferOwnershipRequest(PhotonView targetView,Player requestingPlayer)
    {
        targetView.TransferOwnership(requestingPlayer);

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {

            //  Invoke("checkfallingdamage", 2f);
            StartCoroutine(checkfallingdamage(collision.collider, 1.8f));
            getpushed = true;

            
        }

        /*test for health manager*/
        if (collision.gameObject.tag == "damage")
        {
            if (photonView.IsMine&&!getpushed)
            {
               // Debug.Log("damage is true");

                photonView.RPC("TakeDamage",
                                RpcTarget.AllBuffered,
                               1f, PhotonNetwork.LocalPlayer);

               
            }
        }

    }

    IEnumerator checkfallingdamage(Collider target,float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (isfalling && rb.velocity.y < -9.5f &&getpushed==true)
        {

            Debug.Log("fall damage");
            if (photonView.IsMine)
            {
                photonView.RPC("TakeDamage",
                             RpcTarget.AllBuffered,
                            1f, PhotonNetwork.LocalPlayer);
                //  HealthManagerUI.instance.UpdateHealthImage();
               /* photonView.RPC("AddScore",
                               RpcTarget.AllBuffered,
                              ScoreManager.instance.pushscore,
                              target.gameObject.GetComponent<PhotonView>().Owner);
                              */
                //AddScore(ScoreManager.instance.pushscore, target.gameObject.GetComponent<PhotonView>().Owner);

            }



            getpushed = false;

        }
        getpushed = false;
    }

    void UpdateScore(GameObject trap, float trapType_score)
    {
        if (trap.gameObject.transform.parent.GetComponent<PhotonView>().Owner== PhotonNetwork.LocalPlayer)
        {
           
            Debug.Log("my trap"+trap.gameObject.transform.parent.name);
            
            

        }
        else if (trap.gameObject.transform.parent.GetComponent<PhotonView>().Owner.ActorNumber!=PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Debug.Log("is not mine");
            
            
           // if (trap.gameObject.GetComponentInParent<PhotonView>().Owner.CustomProperties["Score"] != null)
            //{
                // currentscore = (float)trap.gameObject.GetComponentInParent<PhotonView>().Owner.CustomProperties["Score"];

                // AddScore(trapType_score, trap.gameObject.GetComponentInParent<PhotonView>().Owner);
                //  Debug.Log(" second set score success");

                if(photonView.IsMine)
                {
                    photonView.RPC("AddScore",
                              RpcTarget.AllBuffered,
                             trapType_score,
                             trap.gameObject.transform.parent.GetComponent<PhotonView>().Owner);
                }
               
                
                

                Debug.Log(trap.gameObject.transform.parent.GetComponent<PhotonView>().Owner+"will be awarded score");
            //}
            //else
           // {
             //   Debug.LogError("Score key not initialise yet");
            //}


        }
    }

    public override void OnRoomUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnRoomUpdate(targetPlayer, changedProps);
        
        

        if (targetPlayer!=null )
        {
            Debug.Log(targetPlayer + "changed" + changedProps);

            if(photonView.IsMine)
            {
                HealthManagerUI.instance.UpdateHealthImage();
            }
            

            ScoreManager.instance.displayRanking();
          

        }
      
     
    }

    
}

