using UnityEngine;
using System.Collections;

public class WaveGenerator : MonoBehaviour {

	public float amplitude = 2;
	public float frequency = 2;
	public float startMoving = 0;
	private Rigidbody rb;
	


	public char dimension1 = 'x';
	public char dimension2 ;
	float angle = 0;
	
	
	void Start(){
		rb = GetComponent<Rigidbody>();
	
	}
	
	// Update is called once per frame
	void Update () {
		Invoke("move", startMoving);
	}

	void move()
	{
		angle += Time.deltaTime * frequency;
		Vector3 pos = transform.position;



		if (dimension1 == 'x' && dimension2 == 'y')
		{ 
			rb.velocity = new Vector3(0, Mathf.Sin(angle) * amplitude, Mathf.Cos(angle) * amplitude);
			//Debug.Log("Time.deltaTime="+Time.deltaTime);
		//Debug.Log(Mathf.Cos(angle) + "*" + amplitude + "=" + Mathf.Cos(angle) * amplitude);
		}
		if (dimension1 == 'x' && dimension2 == 'z')
		{
			rb.velocity = new Vector3(Mathf.Sin(angle) * amplitude, 0,  Mathf.Cos(angle) * amplitude);
			//Debug.Log("Time.deltaTime=" + Time.deltaTime);
			//Debug.Log(Mathf.Cos(angle) + "*" + amplitude + "=" + Mathf.Cos(angle) * amplitude);
		}
		else if (dimension1 == 'x')
			rb.velocity = new Vector3(Mathf.Sin(angle) * amplitude, 0, 0);
		else if (dimension1 == 'y')
			rb.velocity = new Vector3( 0, Mathf.Sin(angle) * amplitude, 0);
		else if  (dimension1 == 'z')
			rb.velocity = new Vector3( 0, 0, Mathf.Sin(angle) * amplitude);
		transform.position = pos;
	}
}
