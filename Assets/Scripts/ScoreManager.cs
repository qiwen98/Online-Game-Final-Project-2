using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ScoreManager : MonoBehaviourPunCallbacks
{
   
    public float destination_objective_point = 20;
    public float bigfanScore=5;
    public float gunScore=5;
    public float pushscore=5;
    public float current_sequence = 0;
    public int scoreLimit = 100;
    public GameObject UICanvasInGame;
    public GameObject UICanvasForLastResult;
    public GameObject UICanvasForEveryRoundResult;
    object hashTableValue;
    public Player[] pList;
    public float MaxScore;

    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Text roundNum;

    

    public GameObject[] spawnPoint, scoreUnit,scoreUI;
    public Text[] playerRankName, playerRankScore, playerRoundName, playerFinalName, RankNum;

    // public ExitGames.Client.Photon.Hashtable _customroomproperties = new ExitGames.Client.Photon.Hashtable();

    public static ScoreManager instance;

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
        MaxScore = 100f;
        playerName.text = PhotonNetwork.LocalPlayer.NickName;//right corner name
        UICanvasInGame.SetActive(false);
        UICanvasForLastResult.SetActive(false);
        UICanvasForEveryRoundResult.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverScoreUI(int viewid,float scoretoadd)
    {
        int i = 0;

        if (scoretoadd != 0)
        {
           // Debug.Log(i + "showscore");
            scoreUI[viewid].GetComponent<Text>().text = scoretoadd.ToString() + "pts";
            scoreUI[viewid].GetComponent<Animator>().SetTrigger("showScore");
        }


    }


    public void displayRanking()
    {

        
          
        


        sortScore();
       
    }

    public void sortScore()
    {
        Debug.Log("sorting score...");
       Player[] pList = PhotonNetwork.PlayerList;
        
      
            System.Array.Sort(pList, delegate (Player p1, Player p2) {

               

                string p1score = p1.CustomProperties["Score"].ToString();
                string p2score = p2.CustomProperties["Score"].ToString();
                return p1score.CompareTo(p2score);
                
               
            });
        
        
        //reverse the lost for descending order
        System.Array.Reverse(pList);
        // call the display resut
        displayresult(pList);
    }

    public void displayresult(Player[] pList)
    {
        Debug.Log(" after sort");
        foreach (Player p in pList)
        {
            // this is the current score for each player
            Debug.Log(p.NickName + "Score :" + p.CustomProperties["Score"]);
            // this is the current ranking for this round
            Debug.Log(p.NickName + "Reach Destination Ranking   :" + p.CustomProperties["Des_sequence"]);
        }

        /**************************** for UI *******************************************/
        /*
         * 
         
        //eg.
        // this Plist is alrd sorted, every time the score changed, the highest score will be at pList[0]
    
        for (int i=0; i<pList.Length;i++)
        {
            //assign the NickName to the Display name UI text
            
            **********************************************
            UI.displayname[i].text=pList[i].NickName
            ***********************************************
            

          ** if you want to diaplay name for each player, just use PhotonNetwork.LocalPlayer.NickName
       
        }
        */

        // pList[].NickName;
        // string score=(string)pList[0].CustomProperties["Score"];
        //string round = (string)PhotonNetwork.CurrentRoom.CustomProperties["Round"];

        //***************************************
        // NEW!!!!!!!!!!!!!!!! health UI
        //health=PhotonNetwork.LocalPlayer.CustomProperties["Health"];

        int i = 0;
        int num = 1;
        foreach (Player p in pList)
        {
            playerRankName[i].text = p.NickName;
            playerFinalName[i].text = p.NickName;        
            RankNum[i].text = num + ".";
            playerRankScore[i].text = p.CustomProperties["Score"].ToString();
            i++;
            num++;
        }

        for (int j = 0; j < PhotonNetwork.PlayerList.Length; j++)
        {
            playerRoundName[j].text = PhotonNetwork.PlayerList[j].NickName;
            //playerRoundScore[j].text = PhotonNetwork.PlayerList[j].CustomProperties["Score"].ToString();
        }
    }

    public void hoverUI()
    {
       
        
    }



    public float OnDestinationReachedScore(int sequence)

    {

        //if x=1 get 10; x=2 get 5; x=3 get=3.3
        float Score = 1 / sequence * 10;
        Debug.Log("rankscoreadded");
        return  Score; 
    }

    /*Every Round Result Display*/
    public void GraphicDisplayResult()
    {
        Invoke("ResetRoundResult", 15f);//ResultStateTimeLimit
        roundNum.text = PhotonNetwork.CurrentRoom.CustomProperties["Round"].ToString();
        int resultScore;
        int threshold = 0;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            int.TryParse(PhotonNetwork.PlayerList[i].CustomProperties["Score"].ToString(), out resultScore);
            Debug.Log(resultScore);
            int num = resultScore / 10;
            Debug.Log(num);
            float scaleScore = (resultScore % 10) / 10.0f;
            Debug.Log(scaleScore);
            Vector3 pos = spawnPoint[i].transform.position;
            for (int j = 0; j < num; j++)
            {
                if (threshold < MaxScore)
                {
                    if (j != 0)
                    {
                        pos += Vector3.up * 44;
                    }
                    threshold += 10;
                    GameObject a = Instantiate(scoreUnit[i], pos, scoreUnit[i].transform.rotation);
                    //a.transform.parent = UICanvasForEveryRoundResult.transform;
                    a.transform.SetParent(UICanvasForEveryRoundResult.transform);
                }
            }
            if (scaleScore != 0 && threshold < MaxScore)
            {
                GameObject topPiece = scoreUnit[i];
                topPiece.transform.localScale = new Vector3(topPiece.transform.localScale.x * scaleScore, topPiece.transform.localScale.y, topPiece.transform.localScale.z);
                if (num == 0)
                {
                    GameObject b = Instantiate(topPiece, pos - new Vector3(0, 22 * (1 - scaleScore), 0), topPiece.transform.rotation);
                    b.transform.parent = UICanvasForEveryRoundResult.transform;
                }
                else
                {
                    GameObject c = Instantiate(topPiece, pos + new Vector3(0, 22 * (1 + scaleScore), 0), topPiece.transform.rotation);
                    c.transform.parent = UICanvasForEveryRoundResult.transform;
                }
                topPiece.transform.localScale = new Vector3(topPiece.transform.localScale.x / scaleScore * 1.0f, topPiece.transform.localScale.y, topPiece.transform.localScale.z);
            }
        }
    }

    /*Reset*/
    public void ResetRoundResult()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("scorebar"))
        {
            Destroy(obj);
        }
    }
}
