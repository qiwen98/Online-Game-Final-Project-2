using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving : MonoBehaviour
{
    private Transform pos;
    [Header("【需要跟随的物体】")]
    public Transform Pos;
    public float Speed = 20f;
    public bool X = false;
    public bool Y = false;
    public bool Z = false;

    // Use this for initialization
    void Awake()
    {
        pos = transform;
    }
    // Update is called once per frame
    void Update()
    {
        //只勾选一个轴的
        if (X == true && Y == false && Z == false)
        {
            float x = Mathf.Lerp(pos.position.x, Pos.position.x, Time.deltaTime * Speed);
            pos.position = new Vector3(x, pos.position.y, pos.position.z);
        }
        else if (X == false && Y == true && Z == false)
        {
            float y = Mathf.Lerp(pos.position.y, Pos.position.y, Time.deltaTime * Speed);
            pos.position = new Vector3(pos.position.x, y, pos.position.z);
        }
        else if (X == false && Y == false && Z == true)
        {
            float z = Mathf.Lerp(pos.position.z, Pos.position.z, Time.deltaTime * Speed);
            pos.position = new Vector3(pos.position.x, pos.position.y, z);
        }
        //勾选其中两个的
        else if (X == true && Y == true && Z == false)
        {
            float x1 = Mathf.Lerp(pos.position.x, Pos.position.x, Time.deltaTime * Speed);
            float y1 = Mathf.Lerp(pos.position.y, Pos.position.y, Time.deltaTime * Speed);
            pos.position = new Vector3(x1, y1, pos.position.z);
        }
        else if (X == true && Y == false && Z == true)
        {
            float x1 = Mathf.Lerp(pos.position.x, Pos.position.x, Time.deltaTime * Speed);
            float z1 = Mathf.Lerp(pos.position.z, Pos.position.z, Time.deltaTime * Speed);
            pos.position = new Vector3(x1, pos.position.y, z1);
        }
        else if (X == false && Y == true && Z == true)
        {
            float y1 = Mathf.Lerp(pos.position.y, Pos.position.y, Time.deltaTime * Speed);
            float z1 = Mathf.Lerp(pos.position.z, Pos.position.z, Time.deltaTime * Speed);
            pos.position = new Vector3(pos.position.x, y1, z1);
        }
        //三个全部勾上
        else if (X == true && Y == true && Z == true)
        {
            float x1 = Mathf.Lerp(pos.position.x, Pos.position.x, Time.deltaTime * Speed);
            float y1 = Mathf.Lerp(pos.position.y, Pos.position.y, Time.deltaTime * Speed);
            float z1 = Mathf.Lerp(pos.position.z, Pos.position.z, Time.deltaTime * Speed);
            pos.position = new Vector3(x1, y1, z1);
        }
    }
}
