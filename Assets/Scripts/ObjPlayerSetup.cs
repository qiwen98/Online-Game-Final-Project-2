using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System;

public class ObjPlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject FPSCamera;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    public int prefab_value;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<ObjMovementController>().enabled = true;
            FPSCamera.GetComponent<Camera>().enabled = true;
        }
        else
        {
            transform.GetComponent<ObjMovementController>().enabled = false;
            FPSCamera.GetComponent<Camera>().enabled = false;
        }

        SetPlayerUI();
        SetPlayerAssets();
    }

    private void SetPlayerAssets()
    {
        if (photonView.IsMine)
        {
           
            photonView.RPC("AddcustomChild", RpcTarget.AllBuffered, GameManager.instance.assignned_child_trap);//1
        }
    }

    [PunRPC]
    private void AddcustomChild(int whichprefab)
    {
       // prefab_value = whichprefab;
        Debug.Log("the prefab "+whichprefab+"should be spawn");
        if(whichprefab==-1)
        {
            whichprefab = UnityEngine.Random.Range(0, GameManager.instance.traps_available_for_player.Length);
        }

        GameObject trap_1 = GameObject.Instantiate(GameManager.instance.traps_available_for_player[whichprefab],transform.position, Quaternion.identity,this.transform);
        this.transform.name="ObjPlayer_"+ photonView.Owner.NickName;
        // GameObject trap_1 = GameObject.Instantiate(traps[whichprefab],transform.position, Quaternion.identity);

        //objectPlayers_trap_belongings.Add(PhotonNetwork.Instantiate(trap.name, transform.position, Quaternion.identity));


        //Debug.Log("is here");


        //trap_1.transform.parent = objectPlayers[objectPlayers_count - 1].transform;
    }

    void SetPlayerUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = photonView.Owner.NickName;
        }
    }
}
