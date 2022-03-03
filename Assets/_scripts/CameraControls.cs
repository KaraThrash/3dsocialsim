using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    

    public CameraState cameraState;
    public Camera cam;


    public Player player;
    public Transform otherTarget;

    public float followSpeed, maxPlayerDistance, maxPlayerDistanceInside, cameraAngle,lowAngle = 45.0f, highAngle = 75.0f, conversationAngle = 25.0f,insideAngle=50.0f;
    public float focusAngle;
    public float customAngle;

    public float defaultCamAdjustSpeed,defaultFollowSpeed=8,camAdjustSpeed = 5,followSpeedAdjustment=1; //followspeed adjustment for controlling how cloesly the camera follows

    public Vector3 camOffset,highCamOffset,lowCamOffSet,conversationOffset,insideOffset;
    public Vector3 focusOffset;
    public Vector3 customOffset;

    public CameraEffect activeCameraEffect,lostwoodsEffect;

    public Animation fadetoblack;
    public Animator anim;
    public bool focusing;

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
        else if (State() == CameraState.lostwoods)
        { EndCameraEffect("lostwoods"); }


        if (_state == CameraState.lostwoods)
        {
            StartCameraEffect("lostwoods");
        }
        else if (_state == CameraState.basic)
        {
            followSpeed = defaultFollowSpeed;
            camAdjustSpeed = defaultCamAdjustSpeed;

            camOffset = lowCamOffSet;
            cameraAngle = lowAngle;

        }


    }





    void Start()
    {
        if (anim == null)
        {
            if (GetComponent<Animator>() != null)
            {
                anim = GetComponent<Animator>();
            }
        }
        

        camOffset = lowCamOffSet;
        cameraAngle = lowAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.transitionTimer > 0) { return; }

        // if (State() != CameraState.focusing) { }

        if (transform.position.y > 0 && lostwoodsEffect.GetComponent<LostWoodCameraEffect>().leaf0.gameObject.activeSelf)
        {
            lostwoodsEffect.EndEffect();
        }

        Act();


        ManualAdjust();
       

      

        

    }


    public void Act()
    {
        //basic, outside, low,high,inside,showoff,conversation,focusing,zoomIn, lostwoods,custom
        if (cameraState == CameraState.basic) 
        {
            Track(player.transform.position);
            cam.transform.localEulerAngles = Vector3.Slerp(cam.transform.localEulerAngles, new Vector3(cameraAngle, 0, 0), Time.deltaTime * camAdjustSpeed);
        }
        else if (cameraState == CameraState.outside || cameraState == CameraState.lostwoods)
        {
            Track(player.transform.position);
            cam.transform.localEulerAngles = Vector3.Slerp(cam.transform.localEulerAngles, new Vector3(cameraAngle, 0, 0), Time.deltaTime * camAdjustSpeed);
        }
        else if (cameraState == CameraState.inside)
        { 
            TrackInside(player.transform.position);
            cam.transform.localEulerAngles = Vector3.Slerp(cam.transform.localEulerAngles, new Vector3(cameraAngle, 0, 0), Time.deltaTime * camAdjustSpeed);
        }
        else if (cameraState == CameraState.showoff)
        { }
        else if (cameraState == CameraState.conversation)
        {
            
            //look at the point between the player and the target, default to south of the player if the active object isnt set
            Vector3 tempTrackPosition = player.transform.position - new Vector3(0, 0, 1);

            if (GameManager.instance.activeObject != null)
            {
                tempTrackPosition = (player.transform.position + ((GameManager.instance.activeObject.position - player.transform.position) * 0.5f)) ;
                tempTrackPosition = GameManager.instance.ActiveObject().position;


            }

            Track(tempTrackPosition - new Vector3(0, 0, 1));
           // transform.LookAt(tempTrackPosition);
            cam.transform.localEulerAngles = Vector3.Slerp(cam.transform.localEulerAngles, new Vector3(cameraAngle, 0, 0), Time.deltaTime * camAdjustSpeed);

        }
        else if (cameraState == CameraState.focusing)
        { 

              //todo: right now the 'infocuszone function handles this which is called by the ontriggerstay of the focus zone. change this to be handled by the camera
        
        }
        else if (cameraState == CameraState.zoomIn)
        { }

        else if (cameraState == CameraState.custom)
        { }





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

    }



    public void Track(Vector3 _target)
    {
        Vector3 _targetXYPos = camOffset + _target;

        CameraEffect();
        cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, camOffset, Time.deltaTime * followSpeed * followSpeedAdjustment);

        //so the camera doesnt jitte dont move if the target is only slightly off from center
        if (Vector3.Distance(_target, transform.position) > maxPlayerDistance)
        {
            transform.position = Vector3.Lerp(transform.position, _target, Time.deltaTime * (followSpeed + Vector3.Distance(transform.position, player.transform.position) ));
        }
    }


    public void TrackInside(Vector3 _target)
    {
        Vector3 playerXYPos = camOffset + player.transform.position;

        //more freedom of movement before the camera starts moving to center the focus
        if (Vector3.Distance(_target, transform.position) > maxPlayerDistanceInside)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * followSpeed * followSpeedAdjustment);
            cam.transform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, camOffset, Time.deltaTime * followSpeed * followSpeedAdjustment);
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
       // if (activeCameraEffect == null) { return; }
       
        lostwoodsEffect.EndEffect();
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

    public void SetCameraTrackingOffset(float _angle, float _xOffset, float _yOffset, float _zOffset)
    {
        cameraAngle = _angle;
        camOffset = new Vector3(_xOffset, _yOffset, _zOffset);

    }


    public void InFocusZone(Vector3 _offset,float _angle,float _speed)
    {


      //  camAdjustSpeed = Mathf.Lerp(camAdjustSpeed,_speed, Time.deltaTime);
      //  followSpeed = Mathf.Lerp(followSpeed, _speed, Time.deltaTime);
      //  camOffset = _offset;
      //  cameraAngle = _angle;

        Vector3 playerXYPos = _offset ;
        if (Vector3.Distance(playerXYPos, transform.position) > maxPlayerDistanceInside)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerXYPos, Time.deltaTime * _speed * followSpeedAdjustment);
        }

        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, new Vector3(_angle, 0, 0), Time.deltaTime * _speed);

        if (focusing == false)
        {
            // State(CameraState.focusing);
            focusing = true;
           //  camAdjustSpeed = 0;
           // followSpeed = 0;

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
