using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCrack : MonoBehaviour
{
    public GameObject Cracked_child;
    Animator anim;
    bool cracked=false;
    
    // Start is called before the first frame update
    void Start()
    {
        anim =Cracked_child.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&!cracked)
        {
            anim.SetBool("cracked",true);
            Invoke("disable", 0.75f);
            Invoke("RestoreOri",5f);
            cracked = true;
        }
    }

    void disable()
    {
        Cracked_child.SetActive(false);
    }

    private void RestoreOri()
    {
        Cracked_child.SetActive(true);
        anim.SetBool("cracked", false);
        cracked = false;
        
    }
}
