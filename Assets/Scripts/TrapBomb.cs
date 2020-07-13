using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TrapBomb : MonoBehaviour
{
    public static TrapBomb instance;
    public GameObject Bomb;
    public GameObject bombDes;
    public GameObject bombVFX;
    public GameObject redArea;
    public float maxDis;

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
        maxDis = 4f;
        //PreBuildBomb();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerDisplay.instance.PlaceYourTrap.activeInHierarchy)
        {
            PreBuildBomb();
        }
        else if(ScoreManager.instance.UICanvasInGame.activeInHierarchy)
        {
            InGameBomb();
        }
    }

    public void PreBuildBomb()
    {
        bombVFX.SetActive(false);
        redArea.SetActive(true);
    }

    public void InGameBomb()
    {
        redArea.SetActive(false);
        BombExplode();
        DistanceDetected();
        Invoke("DestroySelf", 3f);
    }

    void BombExplode()
    {
        bombVFX.SetActive(true);
        Destroy(bombDes);
    }

    [PunRPC]
    void DistanceDetected()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obj_Player"))
        {
            if ((Vector3.Distance(obj.transform.position, this.transform.position) < maxDis)&&(obj != this))
            {
                //PlayerBehaviour.instance.TransferOwnershipRequest(obj.gameObject.GetPhotonView(), PhotonNetwork.LocalPlayer);
                Destroy(obj);
            }
        }
    }

    [PunRPC]
    void DestroySelf()
    {
        //PhotonNetwork.Destroy(Bomb.gameObject.GetPhotonView());
        Destroy(Bomb);
    }
}
