using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setactivefalse : MonoBehaviour
{
    public GameObject[] GameObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < GameObject.Length; i++)
            {
                /*if (GameObject[i].activeSelf == true) 
                {
                    GameObject[i].SetActive(false);
                }
                else
                {
                    GameObject[i].SetActive(true);
                }*/
                GameObject[i].SetActive(false);
                Invoke("setact",30f);
            }
        }
        
    }
    void setact()
    {
        for (int i = 0; i < GameObject.Length; i++)
        {

            GameObject[i].SetActive(true);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
