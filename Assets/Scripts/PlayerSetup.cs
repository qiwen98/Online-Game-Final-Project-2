using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject FPSCamera;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<PlayerMovementController>().enabled = true;
            transform.GetComponent<PlayerBehaviour>().enabled = true;
            transform.GetComponent<NetworkPlayer>().enabled = true;
            FPSCamera.GetComponent<Camera>().enabled = true;
        }
        else
        {
            transform.GetComponent<PlayerMovementController>().enabled = false;
            transform.GetComponent<PlayerBehaviour>().enabled = false;
            transform.GetComponent<NetworkPlayer>().enabled = false;
            FPSCamera.GetComponent<Camera>().enabled = false;
        }

        SetPlayerUI();
       // SetPlayerAssets();
    }

   

    void SetPlayerUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = photonView.Owner.NickName;
        }
    }
}
