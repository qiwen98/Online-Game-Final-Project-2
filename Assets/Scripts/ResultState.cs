using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class ResultState : MonoBehaviourPunCallbacks, Istate
{
    public float resultStateTimeLimit;
    private float timer;
    public Action CallBack;

    public void onStateEnter()
    {
        timer = 0;
        /* UI operation
        foreach (Player p in PhotonNetwork.PlayerList)
        {
           string Score=(string) p.CustomProperties["Score"];
        }
        */
        foreach (GameObject Player in GameManager.instance.Playerslist)
        {

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


        ScoreManager.instance.UICanvasForEveryRoundResult.SetActive(true);
        ScoreManager.instance.UICanvasInGame.SetActive(false);
        ScoreManager.instance.GraphicDisplayResult();
    }

    public void onStateExit()
    {
       
    }

    public void onStateUpdate()
    {
        timer += Time.deltaTime * 1;

        if (timer > resultStateTimeLimit)
        {

             ScoreManager.instance.UICanvasForEveryRoundResult.SetActive(false);

            CallBack();
            //go back to game manager
        }
    }

   public void onFixedUpdate()
    {

    }


}
