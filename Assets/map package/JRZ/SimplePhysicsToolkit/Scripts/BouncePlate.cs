using UnityEngine;
using System.Collections;

/* Simple Physics Toolkit - Bounce Plate
 * Description: A varient on Fan Controller - applies impulse force, using world space (UP)
 * Required Components: Collider (All Types Supported)
 * Author: Dylan Anthony Auty
 * Version: 1.0
 * Last Change: 2016-01-16
*/
public class BouncePlate : MonoBehaviour {

	public float bounce = 10.0f;

	void Start () {
		if (GetComponent<Collider> ()) {
			GetComponent<Collider>().isTrigger = true;
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.GetComponent<Rigidbody> ()) {
			other.GetComponent<Rigidbody>().AddForce(bounce * Vector3.up, ForceMode.Impulse);
		}
	}

	void OnTriggerStay(Collider other){
		if (other.GetComponent<Rigidbody> ()) {
			other.GetComponent<Rigidbody>().AddForce(bounce * Vector3.up, ForceMode.Impulse);
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.cyan;
		if (GetComponent<BoxCollider> ()) {
			BoxCollider c = GetComponent<BoxCollider> ();
			Gizmos.DrawWireCube (transform.position, c.bounds.size);
		}
	}
}
