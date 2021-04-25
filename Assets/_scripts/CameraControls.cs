using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform player;
    public float followSpeed, maxPlayerDistance, maxPlayerDistanceInside, cameraAngle,lowAngle = 45.0f, highAngle = 75.0f, conversationAngle = 25.0f,insideAngle=50.0f;
    public float camAdjustSpeed = 5,followSpeedAdjustment=1;//followspeed adjustment for controlling how cloesly the camera follows
    public Vector3 camOffset,highCamOffset,lowCamOffSet,conversationOffset,insideOffset;
    // Start is called before the first frame update
    void Start()
    {
        camOffset = lowCamOffSet;
        cameraAngle = lowAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player>().IsInside())
        {
            TrackPlayerInside();
        }
        else 
        {
            TrackPlayer();
        }
        

        if (Input.GetKeyDown(KeyCode.I)) { camOffset = highCamOffset;  }

        if (Input.GetKeyDown(KeyCode.O)) { camOffset = lowCamOffSet;   }
        if (Input.GetKeyDown(KeyCode.P)) { cameraAngle = lowAngle; }

        if (Input.GetKeyDown(KeyCode.U)) { cameraAngle = highAngle; }
        if (Input.GetKeyDown(KeyCode.Y)) { cameraAngle = conversationAngle; }
        if (Input.GetKeyDown(KeyCode.T)) { camOffset = conversationOffset; }

        if (InputControls.PickUpButton())
        {

            if (camOffset == highCamOffset)
            {
                camOffset = lowCamOffSet;
                cameraAngle = lowAngle;
            }
           else if (camOffset == lowCamOffSet)
            {
                camOffset = highCamOffset;
                cameraAngle = highAngle;
            }
            else 
            {
                camOffset = lowCamOffSet;
                cameraAngle = lowAngle;
            }

        }

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(cameraAngle, 0, 0), Time.deltaTime * camAdjustSpeed);

    }

    public void TrackPlayer()
    {
        Vector3 playerXYPos = camOffset + player.position;
        if (Vector3.Distance(playerXYPos, transform.position) > maxPlayerDistance )
        {
            transform.position = Vector3.MoveTowards(transform.position, playerXYPos,Time.deltaTime * followSpeed * followSpeedAdjustment);
        }
    }

    public void TrackPlayerInside()
    {
        Vector3 playerXYPos = camOffset + player.position;
        if (Vector3.Distance(playerXYPos, transform.position) > maxPlayerDistanceInside)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerXYPos, Time.deltaTime * followSpeed * followSpeedAdjustment);
        }
    }


    public void SetCameraTrackingOffset(string _offsetType)
    {
        if (_offsetType.Equals("conversation"))
        { 
            cameraAngle = conversationAngle;
            camOffset = conversationOffset;
            followSpeedAdjustment = 1;
        }
        else if (_offsetType.Equals("inside"))
        {
            cameraAngle = insideAngle;
            camOffset = insideOffset;
            followSpeedAdjustment = 1;
        }
        else if (_offsetType.Equals("showoff"))
        {
            cameraAngle = conversationAngle;
            camOffset = conversationOffset;
            followSpeedAdjustment = 1;
        }
        else if (_offsetType.Equals("high"))
        {
            cameraAngle = highAngle;
            camOffset = highCamOffset;
            followSpeedAdjustment = 1;
        }
        else if (_offsetType.Equals("low"))
        {
            cameraAngle = lowAngle;
            camOffset = lowCamOffSet;
            followSpeedAdjustment = 1;
        }
        else if (_offsetType.Equals("lostwoods"))
        {
            cameraAngle = lowAngle;
            camOffset = lowCamOffSet;
            followSpeedAdjustment = 1;
        }
        else 
        {
            cameraAngle = lowAngle;
            camOffset = lowCamOffSet;
            followSpeedAdjustment = 1;
        }

    }



    public void ConversationToggle(bool _on)
    {
        cameraAngle = _on ? conversationAngle : lowAngle ;
        camOffset = _on ? conversationOffset : lowCamOffSet;
    }

    public void SetLocation(Vector3 _pos)
    {
        transform.position = _pos + camOffset;
    
    }

}
