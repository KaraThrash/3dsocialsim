using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    public float clockHandSpeed;
    public Text dayNumberText,dayNumberTensText;

    public Transform hourHand,backgroundImage;

    private float hourHand_z_Rotation;

    // Start is called before the first frame update
    void Start()
    {
        hourHand_z_Rotation = 330;
        hourHand.localEulerAngles = new Vector3(0, 0, 330);
       // GameManager.instance.TimeAdvance().AddListener(TimeAdvance);
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateClockFace();
    }


    public void TimeAdvance()
    {
       // hourHand_z_Rotation = 360 - (30 * GameManager.instance.TimeManager().currentHour);

    }

    public void SetHour(float _hour)
    {
        hourHand_z_Rotation = _hour;

        if (_hour != 0)
        {
            hourHand_z_Rotation = 360 - (_hour * 360.0f);
        }

    }


    public void UpdateClockFace()
    {

        float rot = 360 - (30 * GameManager.instance.TimeManager().currentHour);

        if (hourHand != null && hourHand.localEulerAngles.z != hourHand_z_Rotation)
        { 
            hourHand.localEulerAngles = Vector3.Slerp(hourHand.localEulerAngles, new Vector3(0, 0, hourHand_z_Rotation), Time.deltaTime * clockHandSpeed);
            backgroundImage.localEulerAngles = Vector3.Slerp(backgroundImage.localEulerAngles, new Vector3(0, 0, hourHand_z_Rotation), Time.deltaTime * clockHandSpeed);
        }
    
    }

    public void SetDayNumber(int _day)
    {
        if (dayNumberText == null || dayNumberTensText == null) { return; }

        if (_day < 10)
        {
            dayNumberTensText.text = "0";
        }
        else 
        {
            dayNumberTensText.text = Mathf.FloorToInt(_day / 10).ToString();
        }
        dayNumberText.text = (_day % 10).ToString();
    }


}
