using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class HealthManagerUI : MonoBehaviour
{
    public static HealthManagerUI instance;
    public Sprite[] HealthHeartImages;
    public Image HealthImage;
    private int healthNum=3;
    public Animator anim;
    public GameObject soundleft;
    public GameObject soundleft_other;
    private void Awake()
    {
        if (instance != null)
        {
            //Destroy(this.gameObject);
        }
        else
        {
            instance = this;
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

    public void UpdateHealthImage()
    {
        HealthImage.enabled = true;
        //healthNum = PlayerBehaviour.instance.health;
        if(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Health"))
        {
            int.TryParse(PhotonNetwork.LocalPlayer.CustomProperties["Health"].ToString(), out healthNum);
           // Debug.Log("I am Called"+healthNum);
        }
      
        //healthNum = (int)PhotonNetwork.LocalPlayer.CustomProperties["Health"];

        switch (healthNum )
        {
            case 0:
                {
                    HealthImage.sprite = HealthHeartImages[0];
                   
                    break;
                }
            case 1:
                {
                    HealthImage.sprite = HealthHeartImages[1];
                   
                    break;
                }
            case 2:
                {
                    HealthImage.sprite = HealthHeartImages[2];
                   
                    break;
                }
            case 3:
                {
                    HealthImage.sprite = HealthHeartImages[3];
                    break;
                }
        }
    }
}
