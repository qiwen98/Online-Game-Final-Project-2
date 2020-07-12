using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{
    protected PlayerMovementController player;
    protected Vector3 RemotePlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
       player=GetComponent<PlayerMovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
            return;

        var LagDistance = RemotePlayerPosition - transform.position;

        //High distance => sync is to much off => send to position
        if (LagDistance.magnitude > 5f)
        {
            transform.position = RemotePlayerPosition;
            LagDistance = Vector3.zero;
        }

        //ignore the y distance
        LagDistance.y = 0;

        if (LagDistance.magnitude < 0.11f)
        {
            //Player is nearly at the point
            player._xMovement = 0;
            player._zMovement = 0;
        }
        else
        {
            //Player has to go to the point
            player._xMovement = LagDistance.normalized.x;
            player._zMovement = LagDistance.normalized.z;
        }

       

    }

    public void OnDrawGizmos()
    {


        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(RemotePlayerPosition, 1f);
        //Debug.Log(RemotePlayerPosition.x);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
           

        }
        else
        {
            RemotePlayerPosition = (Vector3)stream.ReceiveNext();
            
        }
    }
}
