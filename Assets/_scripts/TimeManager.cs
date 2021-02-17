using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public int gameDay, gameHour, gameMinute;
    public Transform clock, hourHand, minuteHand;
    public Text dateText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AdvanceTime(5);
            SetClockHands();
        }
    }

    public void AdvanceTime( int _minute)
    {
        gameMinute += _minute;

        gameHour = Mathf.FloorToInt(gameMinute / 60);

        gameDay = Mathf.FloorToInt(gameHour / 24);
        
       
    }

    public void SetClockHands()
    {
        float hourAsRot = gameHour % 24 == 0 ? 0 : (gameHour % 24) / 24.0f;
        float minuteAsRot = gameMinute % 60 == 0 ? 0 :  (gameMinute % 60) / 60.0f;
        hourHand.localEulerAngles = new Vector3(0,0,  (-360 * hourAsRot)  );
        minuteHand.localEulerAngles = new Vector3(0,0,  (-360 * minuteAsRot)  );

        dateText.text = "D: " + gameDay.ToString() + " H: " + (gameHour % 24).ToString() + " M: " + (gameMinute % 60).ToString();

    }


}
