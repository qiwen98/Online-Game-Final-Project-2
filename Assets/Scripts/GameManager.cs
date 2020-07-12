using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

    private StateMachine stateMachine;
    private ChoosingState choose = new ChoosingState();
    private PreBuildState build = new PreBuildState();
    private BattleState battle = new BattleState();
    private ResultState result = new ResultState();
    public List<GameObject> objectPlayers = new List<GameObject>();
    public List<GameObject> Playerslist = new List<GameObject>();
    public int assignned_child_trap=0;

    public bool player_spawned = false;

    #region Assets for ChoosingScene
  //  public GameObject GameplayerTransparentPrefab;
    public Transform ChoosespawnLocation;
    public Transform ChooseState_Players_spawnLocation;
    public GameObject[] traps_available_for_player;
    public float ChooseStateTimeLimit = 15f;
    #endregion

    #region Assets for PrebuildScene
    public Transform spawnLocation;
    public GameObject playerTransparentPrefab;
    public GameObject[] traps;
    public float buildStateTimeLimit = 15f;
    #endregion

    #region Assets for Battle
    public Transform BattleStateStartingPoint;
    public Transform BattleStateDestination;
   // public GameObject RealplayerPrefab;
    public GameObject[] playerselection;
    public float BattleState_TimerLimit=120f;
    public float roundlimit=2;
    #endregion

    #region Assets for Resultstate
    public float ResultState_TimerLimit = 120f;
    #endregion

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
   

    // Start is called before the first frame update
    void Start()
    {
        //for (int i = 1; i < 5; i++)
        //{
        //    traps[i] = Resources.Load("prefab_"+i) as GameObject;
        //}
        //playerTransparentPrefab = Resources.Load("Invi_player") as GameObject;

        stateMachine = GetComponent<StateMachine>();
        if (PhotonNetwork.IsConnected)
        {

             


            //if (playerTransparentPrefab != null)
            //{
            //    PhotonNetwork.Instantiate(GameplayerTransparentPrefab.name, ChooseState_Players_spawnLocation.position, Quaternion.identity);
            //    GameManager.instance.player_spawned = true;
            //}
            gotoChooseState();
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.UpdateState();
    }

    private void gotoChooseState()
    {
        Debug.Log("choosing state is start");

        
        stateMachine.changeState(Choose());
        
        TimerDisplay.instance.SelectYourTrap.SetActive(true);
    }

    private void ChooseStateCallBack()
    {
        Debug.Log("choosing state is end");
        if (playerTransparentPrefab != null)
        {
            objectPlayers.Add(PhotonNetwork.Instantiate(playerTransparentPrefab.name, spawnLocation.position, Quaternion.identity));
        }

        stateMachine.changeState(Build());
        TimerDisplay.instance.SelectYourTrap.SetActive(false);
        TimerDisplay.instance.PlaceYourTrap.SetActive(true);

        Debug.Log("prebuild start");
    }

    

    public void buildCallBack()
    {
        Debug.Log("build state is end");
        //gotobattle state
        stateMachine.changeState(Battle());
        TimerDisplay.instance.PlaceYourTrap.SetActive(false);
        ScoreManager.instance.UICanvasInGame.SetActive(true);

        Debug.Log("batttle state is start");
    }

    public void BattleWinCallBack()
    {
        Debug.Log("Battle state is end");
        
        Debug.Log("Battle state is win");

        /* UI operation
       
        */
        
        ScoreManager.instance.UICanvasForLastResult.SetActive(true);

        

        // stateMachine.changeState(Build());
        //go to win state

    }

    public void BattleLostCallBack()
    {
        Debug.Log("Battle state is end");
        Debug.Log("Battle state is lost");


        //go to lost state first

        //spwan obj again
        Debug.Log("Go to result state");
        stateMachine.changeState(Result());

    }

    public void resultStateCallBack()
    {
        Debug.Log("result state is end");

        

        //then only go choose again
        stateMachine.changeState(Choose());
       // TimerDisplay.instance.RoundDisplay();
        TimerDisplay.instance.SelectYourTrap.SetActive(true);
        ScoreManager.instance.UICanvasInGame.SetActive(false);
        Debug.Log("choosing state is start");
    }

    private ChoosingState Choose()
    {
        choose.ChooseState_Traps_spawnLocation = ChoosespawnLocation;
        choose.traps_available_for_player = traps_available_for_player;
        choose.ChooseState_Players_spawnLocation = ChooseState_Players_spawnLocation;
        choose.CallBack = ChooseStateCallBack;
       choose.ChooseStateTimeLimit = ChooseStateTimeLimit;
        
        return choose;
    }

    private PreBuildState Build()
    {
        build.spawnLocation = spawnLocation;
        build.playerTransparentPrefab=playerTransparentPrefab;
        build.traps=traps;
        build.CallBack = buildCallBack;
        build.buildStateTimeLimit = buildStateTimeLimit;

        return build;
    }

    private BattleState Battle()
    {
        //run all the scripts attached on the traps
        //foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Trap"))
        //{
        //    obj.transform.GetChild(0).gameObject.SetActive(true);
        //}

        battle.BattleStateStartingPoint=BattleStateStartingPoint;
        battle.BattleStateDestination= BattleStateDestination;
       // battle.RealplayerPrefab=RealplayerPrefab;
        battle.BattleState_TimeLimit= BattleState_TimerLimit;
        battle.WinCallBack = BattleWinCallBack;
        battle.LostCallBack = BattleLostCallBack;
        battle.roundlimit = roundlimit;

        return battle;
    }

    private ResultState Result()
    {
       
        
       
        result.CallBack = resultStateCallBack;
        result.resultStateTimeLimit = ResultState_TimerLimit;

        return result;
    }

   

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerLeftRoom(Player p)
    {
        Debug.Log(p.NickName + "has left");
     
    }

    

    public void onClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
       


    }

    public override void OnLeftRoom()
    {
    
        PhotonNetwork.LoadLevel(0);
        
        base.OnLeftRoom();
    }

}
