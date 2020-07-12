using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChange : MonoBehaviour
{
    public static LevelChange LV;

    public int GameMode;


    private void OnEnable()
    {
        if (LevelChange.LV == null)
        {
            LevelChange.LV = this;
        }
        else
        {
            if (LevelChange.LV != this)
            {
                Destroy(LevelChange.LV.gameObject);
                LevelChange.LV = this;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MyGameMode"))
        {
            GameMode = PlayerPrefs.GetInt("MyGameMode");
        }
        else
        {
            GameMode = 1;
            PlayerPrefs.GetInt("MyGameMode", GameMode);
        }
    }



}
