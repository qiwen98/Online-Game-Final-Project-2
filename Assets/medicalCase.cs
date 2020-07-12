using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class medicalCase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && ScoreManager.instance.UICanvasInGame.activeInHierarchy)
        {
            PlayerBehaviour.instance.AddHealth();
            Destroy(this.gameObject);
        }
    }

}
