using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public int gameDay, gameHour, gameMinute, morningTime = 6, nightTime = 20; //when morning/night starts
    public float sunspeed;
    public Transform sun,clock, hourHand, minuteHand;
    public Light sunLight;
    public Text dateText;
    public GameManager gameManager;
    public float morningIntensity, dayIntensity, nightIntensity;
    public List<Color> colors; //morning, day,dusk, night

    // Start is called before the first frame update
    void Start()
    {
        SetSunAngle(new Vector3 (5,0,181));
    }

    // Update is called once per frame
    void Update()
    {
        

        RotateSunOverTime();
    }

    public void RotateSunOverTime()
    {
        LerpSunColor();

        if (GetHourOfDay() >= nightTime || GetHourOfDay() < morningTime )
        {

          
        }
        
        else 
        {
        

            float sunangle = 220 - (((GetHourOfDay() - morningTime) * 10) + (GetMinuteOfHour() * 0.6f));

            if (sun.transform.eulerAngles.z > sunangle)
            {
               
                sun.Rotate(0, 0, sunspeed * Time.deltaTime);
            }
        }
        

    }

    public void LerpSunColor()
    {
        if (GetHourOfDay() >= nightTime || GetHourOfDay() < morningTime)
        {
            //night: blue
            if (GetHourOfDay() < (morningTime * 0.5f) || GetHourOfDay() >= nightTime)
            {
                sunLight.color = Color.Lerp(sunLight.color, colors[3], Time.deltaTime); 
                sunLight.intensity = Mathf.Lerp(sunLight.intensity, nightIntensity,Time.deltaTime);
                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, colors[3], Time.deltaTime);

            }
            else
            //morning greyblue
            { 
                sunLight.color = Color.Lerp(sunLight.color, colors[0], Time.deltaTime);
                sunLight.intensity = Mathf.Lerp(sunLight.intensity, morningIntensity, Time.deltaTime);

                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, colors[0], Time.deltaTime);
            }

        }
        else
        {
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, dayIntensity, Time.deltaTime);
            //dusk orange
            if (GetHourOfDay() >= nightTime - 2)
            { sunLight.color = Color.Lerp(sunLight.color, colors[2], Time.deltaTime);
                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, colors[2], Time.deltaTime);
            }
            else
            //day white
            { sunLight.color = Color.Lerp(sunLight.color, colors[1], Time.deltaTime);
                RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, colors[1], Time.deltaTime);
            }
 
        }
    }


    public void SetSunAngle(Vector3 newRot)
    {
        sun.eulerAngles = newRot;

    }

    public float GetMinuteOfHour()
    {
        //get the minutes of the current hour
        return (gameMinute % 60);

    }


    public float GetHourOfDay()
    { 
        //what time is it of the current day
        return (gameMinute / 60) % 24;
    
    }

    public float GetHour()
    {
        //get the total hours passed: for tracking total time elapsed rather than for each day as a unique element
        return (gameMinute / 60) ;

    }

    public int GetDay()
    {
        return Mathf.FloorToInt(GetHour() / 24);

    }


    public void AdvanceTime( int _minute)
    {
        if ((gameMinute + _minute) > 60 && GetHourOfDay() == 23)
        { gameManager.StartDay(); }
        gameMinute += _minute;

        gameHour = Mathf.FloorToInt(gameMinute / 60);

        gameDay = Mathf.FloorToInt(gameHour / 24);

        SetClockHands();
      
    }

    public void SetClockHands()
    {
        float hourAsRot =  GetHourOfDay() / 24.0f;
        float minuteAsRot = GetMinuteOfHour() / 60.0f;
        hourHand.localEulerAngles = new Vector3(0,0,  (360 * hourAsRot)  );
        minuteHand.localEulerAngles = new Vector3(0,0,  (360 * minuteAsRot)  );

        dateText.text = "D: " + GetDay().ToString() + " H: " + GetHourOfDay().ToString() + " M: " + GetMinuteOfHour().ToString();

    }


}
