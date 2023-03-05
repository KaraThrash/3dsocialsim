using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public ClockUI clockUi;

    //settings
    public float sunspeed;

    public float morningIntensity, dayIntensity, nightIntensity;

    public int hoursPerDay = 9; //an 'hour' is the incremented time per action e.g. can perform 9 story actions in a 9 hour day
    public int actionCountTracker; //current number of actions performed on the current day

    public List<Color> colors; //morning, day,dusk, night
    public List<Material> grassMaterials;
    public Gradient dayGradient, nightGradient, grassGradient;

    private int gameDay, gameHour, gameMinute, morningTime = 6, nightTime = 20; //when morning/night starts

    public int currentHour;

    private Vector3 BASESUNANGLE_DAY = new Vector3(15, 45, 0);
    private Vector3 SUNROTATION_DAY = new Vector3(1, 5, 0);

    private Vector3 BASESUNANGLE_NIGHT = new Vector3(15, 135, 0);
    private Vector3 SUNROTATION_NIGHT = new Vector3(1, -5, 0);

    public bool isNight;

    public Transform sun, clock, hourHand, minuteHand;

    public Light sunLight;
    public Text dateText;
    public Text dayOneColumnText;
    public Text dayTenColumnText;
    public GameManager gameManager;

    private string grass_baseColor = "Color_796B3B6";

    // Start is
    // called before
    // the first
    // frame update
    private void Start()
    {
        GameManager.instance.TimeAdvance().AddListener(TimeAdvance);
    }

    public void Init()
    {
    }

    // Update is
    // called once
    // per frame
    private void FixedUpdate()
    {
        RotateSunOverTime();
        LerpSunColor();
    }

    public void RotateSunOverTime()
    {
        if (IsNight())
        {
            sun.eulerAngles = Vector3.Lerp(sun.eulerAngles, BASESUNANGLE_NIGHT + (SUNROTATION_NIGHT * GetHour()), Time.deltaTime * sunspeed);
        }
        else
        {
            Vector3 newrot = new Vector3(SUNROTATION_DAY.x, SUNROTATION_DAY.y * GetHour(), SUNROTATION_DAY.z);

            sun.eulerAngles = Vector3.Lerp(sun.eulerAngles, new Vector3(BASESUNANGLE_DAY.x, SUNROTATION_DAY.y * GetHour(), BASESUNANGLE_DAY.z), Time.deltaTime * sunspeed);
        }
    }

    public void LerpSunColor()
    {
        Color newColor = sunLight.color;
        float newIntensity = 1;

        if (IsNight())
        {
            newColor = dayGradient.Evaluate(GetHourAsPercent());
            newIntensity = nightIntensity;
        }
        else
        {
            newColor = dayGradient.Evaluate(GetHourAsPercent());
            newIntensity = dayIntensity;
        }

        if (sunLight.color != newColor)
        {
            sunLight.intensity = Mathf.Lerp(sunLight.intensity, newIntensity, Time.deltaTime * sunspeed);
            sunLight.color = Color.Lerp(sunLight.color, newColor, Time.deltaTime * sunspeed);
            foreach (Material el in grassMaterials)
            {
                el.SetColor(grass_baseColor, grassGradient.Evaluate(GetHourAsPercent()));
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
        return actionCountTracker;
    }

    public float GetHourAsPercent()
    {
        //get the total hours passed: for tracking total time elapsed rather than for each day as a unique element
        return (float)actionCountTracker / (float)(Constants.ACTIONS_PER_NIGHT + Constants.ACTIONS_PER_DAY);
    }

    public int GetDay()
    {
        return gameDay;
    }

    public void TimeAdvance()
    {
        actionCountTracker++;

        if (IsNight())
        {
            if (actionCountTracker >= Constants.ACTIONS_PER_NIGHT + Constants.ACTIONS_PER_DAY)
            {
                IsNight(false);
                actionCountTracker = 0;
                //NOTE: advancing the day is only hear for an update video
                gameDay++;
            }
        }
        else
        {
            if (actionCountTracker >= Constants.ACTIONS_PER_DAY)
            {
                IsNight(true);
            }
        }
        GameManager.instance.UiManager().PostTimeAdvance();

        SetClockHands();
        SetDateText();
    }

    public void SetClockHands()
    {
        float hourAsRot = GetHourOfDay() / 24.0f;
        float minuteAsRot = GetMinuteOfHour() / 60.0f;
        hourHand.localEulerAngles = new Vector3(0, 0, (360 * hourAsRot));
        minuteHand.localEulerAngles = new Vector3(0, 0, (360 * minuteAsRot));

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

    public bool IsNight()
    { return isNight; }

    public void IsNight(bool _night)
    { isNight = _night; }

    public ClockUI ClockUI()
    {
        return clockUi;
    }
}