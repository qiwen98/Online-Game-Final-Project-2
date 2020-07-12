using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject gun;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    private int flag = 0;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float bulletSpeed;



    // Start is called before the first frame update
    void Start()
    {
        speed = 3f;
        bulletSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        //gun.transform.Rotate(Vector3.left * speed);
        if (ScoreManager.instance.UICanvasInGame.activeInHierarchy)
        {
            if (flag == 0)
            {
                InGameGun();
            }
            else if (flag == 1)
            {
                gun.transform.Rotate(Vector3.left * speed);
            }
        }
        else if(TimerDisplay.instance.PlaceYourTrap.activeInHierarchy || TimerDisplay.instance.SelectYourTrap.activeInHierarchy)
        {
            flag = 0;
            CancelInvoke("bulletSpawner");
        }
    }

    void bulletSpawner()
    {
        Vector3 pos = bulletSpawnPoint.transform.position;
        GameObject aBullet = Instantiate(bullet, pos, bulletSpawnPoint.transform.rotation);

        aBullet.transform.parent = gun.transform.parent;

        aBullet.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        Destroy(aBullet, 5.0f);
    }

    void InGameGun()
    {
        flag = 1;
        InvokeRepeating("bulletSpawner", 1f, 0.2f);
        gun.transform.Rotate(Vector3.left * speed);
    }
}
