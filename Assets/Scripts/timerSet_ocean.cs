using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerSet_ocean : MonoBehaviour
{
    //public Text state;
    public Text setTimer;
    public float maxTime;
    private float leftTime;

    public string  stateName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (stateName == "place")
        {
            maxTime = GameManager.instance.buildStateTimeLimit;

            if (leftTime <= 0)
            {
                leftTime = maxTime;
            }

            if (TimerDisplay.instance.PlaceYourTrap.activeInHierarchy)
            {
                leftTime -= Time.deltaTime * 1;
                int intime = (int)leftTime;
                if (intime <= 5)
                {
                    setTimer.color = Color.red;
                }
                else
                {
                    setTimer.color = Color.white;
                }
                setTimer.text = intime.ToString();
            }
        }
        else if(stateName == "select")
        {
            maxTime = GameManager.instance.ChooseStateTimeLimit;

            if (leftTime <= 0)
            {
                leftTime = maxTime;
            }

            if (TimerDisplay.instance.SelectYourTrap.activeInHierarchy)
            {
                leftTime -= Time.deltaTime * 1;
                int intime = (int)leftTime;
                if (intime <= 5)
                {
                    setTimer.color = Color.red;
                }
                else
                {
                    setTimer.color = Color.white;
                }
                setTimer.text = intime.ToString();
            }
        }
    }
}
