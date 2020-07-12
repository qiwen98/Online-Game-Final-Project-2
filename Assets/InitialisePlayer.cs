using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class InitialisePlayer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        SetPlayerAssets();
    }

    private void SetPlayerAssets()
    {
        if (photonView.IsMine)
        {

            photonView.RPC("AddcustomChild", RpcTarget.AllBuffered, 1);//1
        }
    }

    [PunRPC]
    private void AddcustomChild(int whichprefab)
    {



        GameObject Player_1 =PhotonNetwork.Instantiate(GameManager.instance.playerselection[whichprefab].name, 
            GameManager.instance.ChooseState_Players_spawnLocation.position, Quaternion.identity);

        GameManager.instance.Playerslist.Add(Player_1);
        Player_1.transform.name = "Player_" + PhotonNetwork.LocalPlayer.NickName;

        Debug.Log("destroy this object");
        //PhotonNetwork.Destroy(photonView);
    }


}
