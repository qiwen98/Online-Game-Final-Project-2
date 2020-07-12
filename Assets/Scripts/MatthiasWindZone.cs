using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatthiasWindZone : MonoBehaviour
{

    List<Rigidbody> RigidbodiesInWindZoneList = new List<Rigidbody>();
    public Vector3 windDirection ;
    public bool switch_on = true;
    public float windStrength = 40;
    private float accelerateStrength = 1;


    private void Awake()
    {
       
    }

    private void OnTriggerEnter(Collider col)
    {
        Rigidbody objectRigid = col.gameObject.GetComponent<Rigidbody>();
        if (objectRigid != null)
            RigidbodiesInWindZoneList.Add(objectRigid);
    }

    private void OnTriggerExit(Collider col)
    {
        Rigidbody objectRigid = col.gameObject.GetComponent<Rigidbody>();
        if (objectRigid != null)
            RigidbodiesInWindZoneList.Remove(objectRigid);
    }

    private void FixedUpdate()
    {
        windDirection = this.transform.parent.up;
        

        if (RigidbodiesInWindZoneList.Count > 0)
        {
            //if switch on
            if(switch_on)
            {
                foreach (Rigidbody rigid in RigidbodiesInWindZoneList)
                {

                    //rigid.AddForce(windDirection * windStrength);
                    ApplyForceToReachVelocity(rigid, windDirection*windStrength,20);
                    
                    this.GetComponentInChildren<ParticleSystem>().Play();
                }
            }

            else
            {
                foreach (Rigidbody rigid in RigidbodiesInWindZoneList)
                {
                    //off
                    rigid.AddForce(windDirection * 1);
                    
                }

                this.GetComponentInChildren<ParticleSystem>().Stop();
            }


           
        }
    }


    public static void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

        //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedToTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
        }
    }
}