using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapSnow : MonoBehaviour
{
    public static TrapSnow instance;

    float speed = 1f;
    float angle = 0;
    public GameObject ice;
    public GameObject self;

    Vector3 pos;

    public bool iceblock;
    float timer;
    private int icecount;
    private GameObject generatedice;

    // Use this for initialization
    void Start()
    {
        iceblock = false;
        icecount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        angle = (angle + speed) % 360f;
        transform.localRotation = Quaternion.Euler(new Vector3(30, angle, 90));
    }

    void OnTriggerEnter(Collider col)
    {
        if (ScoreManager.instance.UICanvasInGame.activeInHierarchy)
        {
            if (col.tag == "Player" && iceblock == false)
            {
                Debug.Log("1");
                iceblock = true;
                pos = col.transform.position;
                pos.y += 1.45f;

                if (icecount < 1)
                {
                    generatedice = Instantiate(ice, pos, Quaternion.identity);
                    icecount++;
                    Invoke("iceMelt", 5f);
                }

                self.SetActive(false);
            }
        }
    }

    void iceMelt()
    {
        Invoke("snowGenerate", 5f);
        Destroy(generatedice);
    }

    void snowGenerate()
    {
        iceblock = false;
        icecount = 0;
        self.SetActive(true);
    }
}
