using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomModule : MonoBehaviour
{
    public static RandomModule instance;
    // Start is called before the first frame update
     void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Vector3 Randomlocation(Transform ori_location)
    {
        int randz = UnityEngine.Random.Range(0, 5);
        Vector3 spawnlocation = new Vector3(ori_location.position.x,
           ori_location.position.y,
           ori_location.position.z + randz);

        return spawnlocation;
    }
}
