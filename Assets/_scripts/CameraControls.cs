using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform player;
    public float followSpeed, maxPlayerDistance, cameraAngle,lowAngle = 45.0f, highAngle = 75.0f;
    public float camAdjustSpeed = 5;
    public Vector3 camOffset,highCamOffset,lowCamOffSet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();

        if (Input.GetKeyDown(KeyCode.P)) { camOffset = highCamOffset;  }
        if (Input.GetKeyDown(KeyCode.O)) { camOffset = lowCamOffSet;   }
        if (Input.GetKeyDown(KeyCode.I)) { cameraAngle = lowAngle; }
        if (Input.GetKeyDown(KeyCode.U)) { cameraAngle = highAngle; }

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
}
