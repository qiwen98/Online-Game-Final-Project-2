using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController: MonoBehaviourPunCallbacks
{
    public static MenuController instance;

    [Header("CreateOrJoin Panel")]
    public GameObject CreateOrJoinUIPanel;
   
    //public ExitGames.Client.Photon.Hashtable charSelectProperties = new ExitGames.Client.Photon.Hashtable();

    public ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() {
        { "gm", "l1" } };

    public Toggle[] toggles;
    public Button confirmButton;

    public Text[] whoSelect;

    public int level=0;
    public int gmNum;
    public Text GameModeText;
    public Image PanelBackground;
    public Sprite[] LevelBackground;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void start()
    {
        gmNum = 1;
    }

    void Update()
    {

    }

    public void OnClickCharacterPick(int whichCharacter)
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
            //button[whichCharacter].SetActive(false);
        }
    }

    public void OnClickConfirm()
    { 
        string PlayerName = PhotonNetwork.LocalPlayer.NickName;
        //PhotonNetwork.CurrentRoom.SetCustomProperties(charTypeProperties);

        this.GetComponent<PhotonView>().RPC("DisableButton", RpcTarget.All, PlayerInfo.PI.mySelectedCharacter,PlayerName);
        toggles[0].interactable = false;
        toggles[1].interactable = false;
        toggles[2].interactable = false;
        toggles[3].interactable = false;
    }

    

    [PunRPC]
    void DisableButton(int whichCharacter,string PlayerName)
    {
        whoSelect[whichCharacter].text = PlayerName;
        toggles[whichCharacter].interactable = false;
    }

    public void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("l1"))
            {
            //GameModeText.text = "Level 1";

                PhotonNetwork.LoadLevel("Level_Desert");
                level = 1;
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("l2"))
            {
                //GameModeText.text = "Level 2";
                PhotonNetwork.LoadLevel("Level_Pond");
                level = 2;
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("l3"))
            {
                //GameModeText.text = "Level 3";
                PhotonNetwork.LoadLevel("Level_ice");
                level = 3;
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("l4"))
            {
                //GameModeText.text = "Level 4";
                PhotonNetwork.LoadLevel("Level_OceanLand");
                level = 4;
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("l5"))
            {
                //GameModeText.text = "Level 5";
                PhotonNetwork.LoadLevel("Level_Volcano");
                level = 5;
            }
            else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("l6"))
            {
                //GameModeText.text = "Level 6";
                PhotonNetwork.LoadLevel("Level_Jungle");
                level = 6;
            }
            else
            {
                PhotonNetwork.LoadLevel("Level_ice");
            }

            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public void OnBackButtonClicked()
    {
        CreateOrJoinUIPanel.SetActive(true);
        toggles[0].interactable = true;
        toggles[1].interactable = true;
        toggles[2].interactable = true;
        toggles[3].interactable = true;
        this.GetComponent<PhotonView>().RPC("EnableButton", RpcTarget.All, PlayerInfo.PI.mySelectedCharacter);
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    void EnableButton(int whichCharacter)
    {
        Debug.Log("1");
        whoSelect[whichCharacter].text = " ";
        //toggles[whichCharacter].interactable = true;
    }

    //level selection
    public void SetGameMode()
    {
        gmNum++;
        this.GetComponent<PhotonView>().RPC("ChangeLevel", RpcTarget.All,gmNum);

    }

    public void SetGameModeBack()
    {
        if(gmNum > 0)
        {
            gmNum--;
        }
        else
        {
            gmNum = 6 + gmNum;
        }
        this.GetComponent<PhotonView>().RPC("ChangeLevel", RpcTarget.All, gmNum);
    }

    [PunRPC]
    void ChangeLevel(int gmNum)
    {
        PlayerPrefs.SetInt("MyGameMode", gmNum);
        Debug.Log(gmNum);
        PanelBackground.color = Color.white;
        
        if (gmNum % 6 == 1)
        {
            customRoomProperties["gm"] = "l1";
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            PanelBackground.sprite = LevelBackground[0];
        }
        else if (gmNum % 6 == 2)
        {
            customRoomProperties["gm"] = "l2";
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            PanelBackground.sprite = LevelBackground[1];
        }
        else if (gmNum % 6 == 3)
        {
            customRoomProperties["gm"] = "l3";
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            PanelBackground.sprite = LevelBackground[2];
        }
        else if (gmNum % 6 == 4)
        {
            customRoomProperties["gm"] = "l4";
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            PanelBackground.sprite = LevelBackground[3];
        }
        else if (gmNum % 6 == 5)
        {
            customRoomProperties["gm"] = "l5";
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            PanelBackground.sprite = LevelBackground[4];
        }
        else if (gmNum % 6 == 0)
        {
            customRoomProperties["gm"] = "l6";
            PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
            PanelBackground.sprite = LevelBackground[5];
        }
    }

}
