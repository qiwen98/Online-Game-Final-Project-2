using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LFZ_CloudGenerator : MonoBehaviourPunCallbacks
{
    public GameObject platform;
    public Transform position;
    public static LFZ_CloudGenerator instance;
    private float timelimit = 5;
    private float timer ;


    // Start is called before the first frame update
    void Start()
    {

        timer = 0;
        //随机

       

    }


    void SpawnPlatform()
    {
        PhotonNetwork.Instantiate(platform.name, this.transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (ScoreManager.instance.UICanvasInGame.activeInHierarchy)
        {
            position.position = platform.transform.position;
            timer += Time.deltaTime * 1;
            if (timer > timelimit)
            {
                timer = 0;
                SpawnPlatform();
            }
        }
    }


}