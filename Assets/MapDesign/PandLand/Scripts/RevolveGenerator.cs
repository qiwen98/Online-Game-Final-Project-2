using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolveGenerator : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    Vector3 m_EulerAngleVelocity;
    int i ;
    

    [SerializeField]
    float Speed;
    void Start()
    {
        //Set the axis the Rigidbody rotates in (100 in the y axis)
        m_EulerAngleVelocity = new Vector3(0, Speed, 0);

        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = GetComponent<Rigidbody>();

        i = 0;
    }

    /*private void Update()
    {
        i++;
        if (i == 100)
        {
            Speed = Speed * Random.Range(1, 10);
            Debug.Log("Speed now = " + Speed);
            i = 0;
        }
    }*/

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.deltaTime);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
     
    }
}
