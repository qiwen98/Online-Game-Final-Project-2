using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TimerDisplay : MonoBehaviour
{
    public static TimerDisplay instance;

    public GameObject battleTimeCounter;
    public GameObject roundDisplayCounter;
    public Text roundCounter;
    public Text roundTimer;
    public Text battleStateTimer;
    public float roundTimeCounter;
    public float battleSTimer;
    public float battleStateTimeCounter;
    public int round;
    public GameObject SelectYourTrap;
    public GameObject PlaceYourTrap;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (MenuController.instance.level == 1 || MenuController.instance.level == 5)
            SelectYourTrap.SetActive(true);
        else
            SelectYourTrap.SetActive(false);
        round = 1;
        battleTimeCounter.SetActive(false);
        roundDisplayCounter.SetActive(false);
        
        PlaceYourTrap.SetActive(false);
        roundTimeCounter = GameManager.instance.ResultState_TimerLimit;
        battleStateTimeCounter = GameManager.instance.BattleState_TimerLimit;
    }

    // Update is called once per frame
    void Update()
    {
       // Invoke("RoundUpdate", 1f);

        if (ScoreManager.instance.UICanvasForEveryRoundResult.activeInHierarchy == true)
        {
            roundTimeCounter -= Time.deltaTime * 1;
            int intime = (int)roundTimeCounter;
            if (intime <= 5)
            {
                roundTimer.color = Color.red; 
            }
            else
            {
                roundTimer.color = Color.black;
            }
            roundTimer.text = intime.ToString();
        }
        else
        {
            roundTimeCounter = GameManager.instance.ResultState_TimerLimit;
        }

        if (battleSTimer > 0)
        {
            int battime = (int)battleStateTimeCounter - (int)battleSTimer;

            if (battime == 20)
            {
                Debug.Log("countdown");
                TimeAppear();
                battleStateTimer.text = "20s Left";
                Invoke("TimeDisappear", 2f);
            }
            else if (battime == 5)
            {
                TimeAppear();
                battleStateTimer.text = "5s Left";
                Invoke("TimeDisappear", 2f);
            }
            else if (battime == 0)
            {
                battleStateTimeCounter = GameManager.instance.BattleState_TimerLimit;
            }
        }
           
    }

    public void TimeAppear()
    {
        battleTimeCounter.SetActive(true);
    }

    public void TimeDisappear()
    {
        battleTimeCounter.SetActive(false);
    }

    public void RoundDisplay(int currentround)
    {
        //RoundUpdate();
        roundDisplayCounter.SetActive(true);
        
        roundCounter.text = "Round " + currentround;
        //round++;
        Invoke("RoundDisapear", 3f);
    }

    public void RoundDisapear()
    {
        roundDisplayCounter.SetActive(false);
    }

    public void RoundUpdate()
    {

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("Round"))
        {
           // round = (int)PhotonNetwork.CurrentRoom.CustomProperties["Round"] + 1;
            int.TryParse(PhotonNetwork.CurrentRoom.CustomProperties["Round"].ToString(), out round);
            Debug.Log("round in UI"+round);
        }
    }
}

