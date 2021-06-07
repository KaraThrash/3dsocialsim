using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventInit : MonoBehaviour
{
    public SceneDirector sceneDirector;
    public LostWoods lostWoods;



    public void HavePlayerFollow(GameManager gameManager, Villager _villager, Vector3 _location)
    {

        _villager.StoryState(VillagerStoryState.inScene);
        _villager.ScriptedAction(SceneAction.leadPlayer);
        _villager.SetNavMeshDestination(_location);



    }

    public void SetCheckPointLocation(GameManager gameManager, Transform _location)
    {
        sceneDirector.checkPoint = _location.position;
        GameManager.instance.SetContinueButton(false);
    }

    

    public void LeadPlayer(GameManager gameManager, Villager _villager,Vector3 _location,int _linesOfDialogue,float _speed=1)
    {

        gameManager.cameraControls.SetCameraTrackingOffset("high");
        //gameManager.ContinueButton().GetComponent<Image>().enabled = false;
        //gameManager.ContinueButton().transform.GetChild(0).GetComponent<Text>().enabled = false;
        //gameManager.ContinueButton().interactable = false;

        sceneDirector.primary = _villager;

        sceneDirector.currentScene = new SceneObject();
        sceneDirector.currentScene.actionType = SceneAction.leadPlayer;
        sceneDirector.currentScene.targetPos = _location;
        sceneDirector.currentScene.startPos = _villager.transform.position;
        sceneDirector.currentScene.linesOfDialogue = _linesOfDialogue;
        sceneDirector.currentScene.primarySpeed = _speed;
        sceneDirector.currentScene.distanceIncrement = Vector3.Distance(transform.position, _location) / _linesOfDialogue;
        //sceneDirector.checkPoint = Vector3.zero;
        _villager.StoryState(VillagerStoryState.inScene);
        _villager.ScriptedAction(SceneAction.leadPlayer);
        _villager.SetNavMeshDestination(_location);

        gameManager.GetPlayer().SetState(PlayerState.inScene);

        Vector3 playerTarget = (_villager.transform.position - gameManager.GetPlayer().transform.position);
        playerTarget = new Vector3(playerTarget.x, 0, playerTarget.z).normalized;

      //  gameManager.cameraControls.otherTarget = _villager.transform;


        sceneDirector.sceneActive = true;

    }





    public void MovePlayer(GameManager gameManager,Transform _location)
    {

        gameManager.actionTimer = 1.1f;
        gameManager.transitionTimer = 0.5f;


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
