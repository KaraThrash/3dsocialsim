using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //settings
    public float sunspeed;
    public float morningIntensity, dayIntensity, nightIntensity;

    public int hoursPerDay = 9; //an 'hour' is the incremented time per action e.g. can perform 9 story actions in a 9 hour day

    public List<Color> colors; //morning, day,dusk, night
    public Gradient dayGradient,nightGradient;

    private int gameDay, gameHour, gameMinute, morningTime = 6, nightTime = 20; //when morning/night starts

    public int currentHour;

    private Vector3 BASESUNANGLE_DAY = new Vector3(0 ,360,0);
    private Vector3 SUNROTATION_DAY = new Vector3(5,-25,0);

    private Vector3 BASESUNANGLE_NIGHT = new Vector3(5, 360, 0);
    private Vector3 SUNROTATION_NIGHT = new Vector3(0, -15, 0);

    public bool isNight;
    

    public Transform sun,clock, hourHand, minuteHand;

    public Light sunLight;
    public Text dateText;
    public Text dayOneColumnText;
    public Text dayTenColumnText;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        RotateSunOverTime();
        LerpSunColor();


    }

    public void RotateSunOverTime()
    {
        

        if (IsNight())
        {
            sun.eulerAngles = Vector3.Lerp(sun.eulerAngles, BASESUNANGLE_NIGHT + (SUNROTATION_NIGHT * currentHour), Time.deltaTime * sunspeed);
        }
        else 
        {
            sun.eulerAngles = Vector3.Lerp(sun.eulerAngles, BASESUNANGLE_DAY + (SUNROTATION_DAY * currentHour), Time.deltaTime * sunspeed);

        }


    }

    public void LerpSunColor()
    {

        Color newColor = sunLight.color;
        float newIntensity = 1;

        if (IsNight())
        {
            if (hoursPerDay > 0 && currentHour <= hoursPerDay)
            {
                newColor = nightGradient.Evaluate((float)currentHour / (float)hoursPerDay);
            }
            newIntensity = nightIntensity;
        }
        else
        {
            if (hoursPerDay > 0 && currentHour <= hoursPerDay)
            {
                newColor = dayGradient.Evaluate((float)currentHour / (float)hoursPerDay);
            }
            newIntensity = dayIntensity;
        }

        sunLight.intensity = Mathf.Lerp(sunLight.intensity, newIntensity, Time.deltaTime * sunspeed);
        sunLight.color = Color.Lerp(sunLight.color, newColor, Time.deltaTime * sunspeed);
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
        currentHour++;

        SetClockHands();
        SetDateText();
    }

    public void SetClockHands()
    {
        float hourAsRot =  GetHourOfDay() / 24.0f;
        float minuteAsRot = GetMinuteOfHour() / 60.0f;
        hourHand.localEulerAngles = new Vector3(0,0,  (360 * hourAsRot)  );
        minuteHand.localEulerAngles = new Vector3(0,0,  (360 * minuteAsRot)  );

        dateText.text = "D: " + GetDay().ToString() + " H: " + GetHourOfDay().ToString() + " M: " + GetMinuteOfHour().ToString();

        

    }

    public void SetDateText()
    {
        if (dayTenColumnText != null && dayOneColumnText != null)
        {
            dayTenColumnText.text = (GetDay() % 10).ToString();
            dayOneColumnText.text = (GetDay() - ((GetDay() % 10) * 10)).ToString();
        }
    }


    public bool IsNight() { return isNight; }
    public void IsNight(bool _night) { isNight = _night; }

}
