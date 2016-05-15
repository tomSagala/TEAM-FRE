using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Clock : MonoBehaviour
{
    public Text timeText;
    public PlayState playState;

	void Start ()
    {
        SetTimeTextFromFloat(playState.MaxTimer);
	}
	
	void Update ()
    {
        SetTimeTextFromFloat(playState.MaxTimer - playState.GameTimer);
	}

    private void SetTimeTextFromFloat(float currentTime)
    {
        int minutes = (int) currentTime / 60;
        int seconds = (int) currentTime % 60;

        if (minutes < 0)
        {
            timeText.text = "0:00";
        }
        else
        {
            if (seconds < 10)
            {
                timeText.text = "" + minutes + ":" + seconds + "0";
            }
            else
            {
                timeText.text = "" + minutes + ":" + seconds;
            }
        }
    }
}
