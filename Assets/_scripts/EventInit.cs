using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInit : MonoBehaviour
{
    public LostWoods lostWoods;


    public void StartLostWoods(GameManager gameManager)
    {

        lostWoods.StartLostWoods();

        gameManager.actionTimer = 0.35f;
        gameManager.transitionTimer = 0.3f;

        gameManager.player.state = PlayerState.acting;

        gameManager.player.transform.position = new Vector3(lostWoods.transform.position.x, lostWoods.transform.position.y, lostWoods.transform.position.z);
        gameManager.cameraControls.fadetoblack.Play();
        gameManager.cameraControls.StartCameraEffect("lostwoods");




    }


}
