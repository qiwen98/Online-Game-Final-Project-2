using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BombTrap : MonoBehaviour
{
    public static BombTrap instance;
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
        PreBuildBomb();
        Invoke("InGameBomb",2f);
    }

    // Update is called once per frame
    void Update()
    {

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
    }

    void BombExplode()
    {
        Destroy(bombDes);
        bombVFX.SetActive(true);
    }

    void DistanceDetected()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Trap"))
        {
            if(Vector3.Distance(obj.transform.position, this.transform.position) < maxDis)
            {
                Destroy(obj);
            }
        }
    }

}
  