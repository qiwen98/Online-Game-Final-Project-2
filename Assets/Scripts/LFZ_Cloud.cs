using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LFZ_Cloud : MonoBehaviourPunCallbacks
{
    [SerializeField]//we gonna expose this variable in the editor 
    private float speed;
    Rigidbody rb;

    public float amplitude = 0;
    public float frequency = 0;
    public float startMoving = 0;

    public char dimension = 'z';
    float angle = 0;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        rb.velocity = new Vector3(0, 0, speed);
        Invoke("EnabledBC", 1f);
        Invoke("FallDestroy", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position += new Vector3(0, 0, 0.05f); 
        Invoke("move", startMoving);
    }

    void FallDestroy()
    {
        rb.useGravity = true;
        if(photonView.IsMine)
        {
            PhotonNetwork.Destroy(transform.gameObject);
        }
       
    }

    void EnabledBC()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }

    void move()
    {
        angle += Time.deltaTime * frequency;
        Vector3 pos = transform.position;


        if (dimension == 'x')
            rb.velocity = new Vector3(speed, 0, 0);
        else if (dimension == 'y')
            rb.velocity = new Vector3(0, speed, 0);
        else if (dimension == 'z')
            rb.velocity = new Vector3(0, 0, speed);
        transform.position = pos;
    }

}