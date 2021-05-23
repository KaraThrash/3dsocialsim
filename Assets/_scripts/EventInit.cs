using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInit : MonoBehaviour
{
    public LostWoods lostWoods;


    public void MovePlayer(GameManager gameManager,Transform _location)
    {

        gameManager.actionTimer = 1.1f;
        gameManager.transitionTimer = 1.1f;


        gameManager.State(GameState.transitioning);

        gameManager.player.SetState(PlayerState.acting);

        gameManager.pendingNewPosition = new Vector3(_location.position.x, gameManager.GetPlayer().transform.position.y, _location.position.z) - Vector3.forward;

        //camera fade to black animation is 1 second to black and .1 before the black clears
        gameManager.cameraControls.anim.speed = 1;

        gameManager.cameraControls.anim.Play("CameraFadeToBlack");

    }






    public void StartLostWoods(GameManager gameManager)
    {

        lostWoods.StartLostWoods();

        gameManager.actionTimer = 1.1f;
        gameManager.transitionTimer = 1.1f;

        //gameManager.player.state = PlayerState.acting;

       // gameManager.player.transform.position = new Vector3(lostWoods.transform.position.x, lostWoods.transform.position.y, lostWoods.transform.position.z);

        gameManager.State(GameState.transitioning);

        gameManager.player.state = PlayerState.acting;

        // TerrainManager().EnterBuilding(_interiorObj, _connectedArea);

        gameManager.pendingNewPosition = new Vector3(lostWoods.transform.position.x, lostWoods.transform.position.y, lostWoods.transform.position.z);

        //camera fade to black animation is 1 second to black and .1 before the black clears
        gameManager.cameraControls.anim.speed = 1;

        gameManager.cameraControls.anim.Play("CameraFadeToBlack");


        gameManager.cameraControls.StartCameraEffect("lostwoods");




    }


}
