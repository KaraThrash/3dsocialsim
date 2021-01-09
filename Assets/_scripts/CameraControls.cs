using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform player;
    public float followSpeed, maxPlayerDistance, cameraAngle,lowAngle = 45.0f, highAngle = 75.0f, conversationAngle = 25.0f;
    public float camAdjustSpeed = 5;
    public Vector3 camOffset,highCamOffset,lowCamOffSet,conversationOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();

        if (Input.GetKeyDown(KeyCode.I)) { camOffset = highCamOffset;  }

        if (Input.GetKeyDown(KeyCode.O)) { camOffset = lowCamOffSet;   }
        if (Input.GetKeyDown(KeyCode.P)) { cameraAngle = lowAngle; }

        if (Input.GetKeyDown(KeyCode.U)) { cameraAngle = highAngle; }
        if (Input.GetKeyDown(KeyCode.Y)) { cameraAngle = conversationAngle; }
        if (Input.GetKeyDown(KeyCode.T)) { camOffset = conversationOffset; }

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(cameraAngle, 0, 0), Time.deltaTime * camAdjustSpeed);

    }

    public void TrackPlayer()
    {
        Vector3 playerXYPos = camOffset + player.position;
        if (Vector3.Distance(playerXYPos, transform.position) > maxPlayerDistance )
        {
            transform.position = Vector3.MoveTowards(transform.position, playerXYPos,Time.deltaTime * followSpeed );
        }
    }

    public void ConversationToggle(bool _on)
    {
        cameraAngle = _on ? conversationAngle : lowAngle ;
        camOffset = _on ? conversationOffset : lowCamOffSet;
    }

}
