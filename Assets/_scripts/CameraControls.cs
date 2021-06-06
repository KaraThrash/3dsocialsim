using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Player player;
    public Transform otherTarget;
    public CameraState cameraState;
    public float followSpeed, maxPlayerDistance, maxPlayerDistanceInside, cameraAngle,lowAngle = 45.0f, highAngle = 75.0f, conversationAngle = 25.0f,insideAngle=50.0f;
    public float focusAngle;
    public float defaultCamAdjustSpeed,defaultFollowSpeed=8,camAdjustSpeed = 5,followSpeedAdjustment=1; //followspeed adjustment for controlling how cloesly the camera follows
    public Vector3 camOffset,highCamOffset,lowCamOffSet,conversationOffset,insideOffset;
    public Vector3 focusOffset;
    public CameraEffect activeCameraEffect,lostwoodsEffect;

    public Animation fadetoblack;
    public Animator anim;

    public CameraState State() { return cameraState; }

    public void State(CameraState _state) { OnStateChange(_state); cameraState = _state; }

    public void OnStateChange(CameraState _state)
    {
        //new state is same as the old state
        if (State() == _state) { return; }
        if (State() == CameraState.focusing) 
        { 
            camAdjustSpeed = defaultCamAdjustSpeed;
            followSpeed = defaultFollowSpeed;
            camOffset = lowCamOffSet;
            cameraAngle = lowAngle;
        }

    }





    void Start()
    {
        anim = GetComponent<Animator>();

        camOffset = lowCamOffSet;
        cameraAngle = lowAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.transitionTimer > 0) { return; }

        if (State() != CameraState.focusing)
        {
            if (player.State() == PlayerState.inScene && otherTarget != null)
            {
                camAdjustSpeed = Mathf.Lerp(camAdjustSpeed, defaultCamAdjustSpeed, Time.deltaTime);
                followSpeed = otherTarget.GetComponent<Villager>().GetNavmeshVelocity().magnitude;

                //Track(otherTarget);
                TrackTalking((player.transform.position + otherTarget.position) / 2);
            }
            if (player.State() == PlayerState.talking && otherTarget != null)
            {
                camAdjustSpeed = Mathf.Lerp(camAdjustSpeed, defaultCamAdjustSpeed, Time.deltaTime);
                followSpeed = otherTarget.GetComponent<Villager>().GetNavmeshVelocity().magnitude;

                TrackTalking(player.transform.position + player.transform.forward);
            }
            else
            {
                camAdjustSpeed = Mathf.Lerp(camAdjustSpeed, defaultCamAdjustSpeed, Time.deltaTime);
                followSpeed = Mathf.Lerp(followSpeed, defaultFollowSpeed, Time.deltaTime);

                Track(player.transform);
            }
        }
        


        if (player.IsInside())
        {
            TrackPlayerInside();
        }
        else 
        {
          //  Track(otherTarget);
        }


        ManualAdjust();
       

      

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(cameraAngle, 0, 0), Time.deltaTime * camAdjustSpeed);

    }

    public void ManualAdjust()
    {
        if (State() == CameraState.focusing) { return; }


            if (Input.GetKeyDown(KeyCode.I)) { camOffset = highCamOffset; }

        if (Input.GetKeyDown(KeyCode.O)) { camOffset = lowCamOffSet; }
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
    }

    public void TrackTalking(Vector3 _target)
    {
        Vector3 _targetXYPos = camOffset + _target;

        CameraEffect();

        transform.LookAt(_target);

        if (Vector3.Distance(_targetXYPos, transform.position) > maxPlayerDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetXYPos, Time.deltaTime * followSpeed * followSpeedAdjustment);
        }
    }


    public void Track(Transform _target)
    {
        Vector3 _targetXYPos = camOffset + _target.position;

        CameraEffect();

        if (Vector3.Distance(_targetXYPos, transform.position) > maxPlayerDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetXYPos, Time.deltaTime * followSpeed * followSpeedAdjustment);
        }
    }


    public void TrackPlayer()
    {
        Vector3 playerXYPos = camOffset + player.transform.position;

        CameraEffect();

        if (Vector3.Distance(playerXYPos, transform.position) > maxPlayerDistance )
        {
            transform.position = Vector3.MoveTowards(transform.position, playerXYPos,Time.deltaTime * followSpeed * followSpeedAdjustment);
        }
    }

    public void CameraEffect()
    {
        if (activeCameraEffect != null && player.WorldLocation() == WorldLocation.lostwoods)
        {
            activeCameraEffect.Effect(player.MoveDirection());
        }

    }

    public void StartCameraEffect(string _effect)
    {
        if (_effect.Equals("lostwoods"))
        {
            activeCameraEffect = lostwoodsEffect;
            activeCameraEffect.StartEffect();
        }
        else if (_effect.Equals("bus"))
        {

        }
        else 
        {
            activeCameraEffect = null;
        }

    }

    public void EndCameraEffect(string _effect)
    {
        if (activeCameraEffect == null) { return; }

        activeCameraEffect.EndEffect();
    }






    public void TrackPlayerInside()
    {
        Vector3 playerXYPos = camOffset + player.transform.position;
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

    public void InFocusZone(Vector3 _offset,float _angle,float _speed)
    {


        camAdjustSpeed = Mathf.Lerp(camAdjustSpeed,_speed, Time.deltaTime);
        followSpeed = Mathf.Lerp(followSpeed, _speed, Time.deltaTime);
        camOffset = _offset;
        cameraAngle = _angle;
        if (State() != CameraState.focusing)
        {
            State(CameraState.focusing);
           
            camAdjustSpeed = 0;
            followSpeed = 0;

        }

        //camOffset = Vector3.Lerp(camOffset, _offset, _speed * Time.deltaTime );
        //cameraAngle = Mathf.Lerp(cameraAngle, _angle, _speed * Time.deltaTime );
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
