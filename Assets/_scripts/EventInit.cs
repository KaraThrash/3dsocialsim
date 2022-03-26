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

       // gameManager.cameraControls.SetCameraTrackingOffset("high");
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
        _villager.watchPlayer = false;

        gameManager.GetPlayer().State(PlayerState.inScene);

        Vector3 playerTarget = (_villager.transform.position - gameManager.GetPlayer().transform.position);
        playerTarget = new Vector3(playerTarget.x, 0, playerTarget.z).normalized;

        RaycastHit hit;

        if (Physics.Raycast(gameManager.GetPlayer().transform.position + Vector3.up, Vector3.down, out hit, 5.5f))
        {
            gameManager.GetPlayer().WarpNav(hit.point);




        }
        else 
        {
            Debug.Log("Navmesh: No hit -- where do I warp to?");
            gameManager.GetPlayer().WarpNav(gameManager.GetPlayer().transform.position);
        }

        //  gameManager.cameraControls.otherTarget = _villager.transform;


        sceneDirector.sceneActive = true;

    }

    private Door previousDoor;

    public void EnterDoor(GameManager gameManager, Door _door)
    {

        previousDoor = _door;

        gameManager.TransitionTimer(Constants.CHUNK_TRANSITION_TIME) ;
        gameManager.ActionTimer(Constants.CHUNK_ACTION_TIME) ;


        gameManager.State(GameState.transitioning);

        gameManager.player.State(PlayerState.acting);

        gameManager.player.EnterDoor();


        gameManager.pendingNewPosition = _door.ExitPosition();

        gameManager.player.WorldLocation(_door.ExitLocation());
        //camera fade to black animation is 1 second to black and .1 before the black clears

        gameManager.cameraControls.anim.speed = 1;

        gameManager.cameraControls.anim.Play(Constants.ANIM_CLEAR_TO_BLACK);

        gameManager.LocationManager().LoadTerrain(_door.ExitLocation());


    }

    public bool ExitDoor(GameManager gameManager)
    {
        if (previousDoor == null)
        {
            gameManager.player.State(PlayerState.playerControlled);
            return false;
        }

        //gameManager.pendingNewPosition = previousDoor.ExitPosition();
        gameManager.player.WorldLocation(previousDoor.ExitLocation());
        gameManager.LocationManager().UnloadChunks(previousDoor.ExitLocation());
        

        gameManager.player.State(PlayerState.playerControlled);

        gameManager.player.TeleportPlayer(previousDoor.ExitPosition(), previousDoor.ExitRotation());

        


        gameManager.cameraControls.SetLocation(previousDoor.ExitPosition());
        //camera fade to black animation is 1 second to black and .1 before the black clears



        return true;


    }



    public void EnterBuilding(GameManager gameManager, Transform _location)
    {

        gameManager.actionTimer = 2.1f;
        gameManager.transitionTimer = 1.1f;


        gameManager.State(GameState.transitioning);

        gameManager.player.State(PlayerState.acting);

        gameManager.pendingNewPosition = _location.position;
        gameManager.player.WorldLocation(WorldLocation.inside);
        //camera fade to black animation is 1 second to black and .1 before the black clears
        gameManager.cameraControls.anim.speed = 1;

        gameManager.cameraControls.anim.Play("CameraClearToBlack");

    }


    public void ExitBuilding(GameManager gameManager, Transform _location)
    {

        gameManager.actionTimer = 2.1f;
        gameManager.transitionTimer = 1.1f;


        gameManager.State(GameState.transitioning);

        gameManager.player.State(PlayerState.acting);

        gameManager.pendingNewPosition = _location.position;
        gameManager.player.WorldLocation(WorldLocation.overWorldSouth);
        //camera fade to black animation is 1 second to black and .1 before the black clears
        gameManager.cameraControls.anim.speed = 1;

        gameManager.cameraControls.anim.Play("CameraClearToBlack");

    }


    public void MovePlayer(GameManager gameManager,Transform _location)
    {

        gameManager.actionTimer = 2.1f;
        gameManager.transitionTimer = 1.1f;


        gameManager.State(GameState.transitioning);

        gameManager.player.State(PlayerState.acting);

        gameManager.pendingNewPosition = new Vector3(_location.position.x, gameManager.GetPlayer().transform.position.y, _location.position.z) - Vector3.forward;

        //camera fade to black animation is 1 second to black and .1 before the black clears
        gameManager.cameraControls.anim.speed = 1;

        gameManager.cameraControls.anim.Play("CameraClearToBlack");

    }

    public void MovePlayer(GameManager gameManager, Vector3 _location)
    {

        gameManager.actionTimer = 2.1f;
        gameManager.transitionTimer = 1.1f;


        gameManager.State(GameState.transitioning);

        gameManager.player.State(PlayerState.acting);

        gameManager.pendingNewPosition = _location;

        //camera fade to black animation is 1 second to black and .1 before the black clears
        gameManager.cameraControls.anim.speed = 1;

        gameManager.cameraControls.anim.Play("CameraClearToBlack");

    }




    public void StartLostWoods(GameManager gameManager,bool _enteredFromSouth)
    {

        lostWoods.SetOverWorldExit(_enteredFromSouth);
        lostWoods.StartLostWoods();

        gameManager.actionTimer = 1.1f;
        gameManager.transitionTimer = 1.1f;

        //gameManager.player.state = PlayerState.acting;

       // gameManager.player.transform.position = new Vector3(lostWoods.transform.position.x, lostWoods.transform.position.y, lostWoods.transform.position.z);

        gameManager.State(GameState.transitioning);

        gameManager.player.State( PlayerState.acting);
        gameManager.player.WorldLocation( WorldLocation.lostwoods);

        // TerrainManager().EnterBuilding(_interiorObj, _connectedArea);

        gameManager.pendingNewPosition = new Vector3(lostWoods.transform.position.x, lostWoods.transform.position.y, lostWoods.transform.position.z);

        //camera fade to black animation is 1 second to black and .1 before the black clears
        gameManager.cameraControls.anim.speed = 1;

        gameManager.cameraControls.anim.Play("CameraClearToBlack");


        gameManager.cameraControls.State(CameraState.lostwoods);
       // gameManager.cameraControls.StartCameraEffect("lostwoods");




    }


    public void LoadNewScene(string _scene)
    {
        ScriptableScene instance = Instantiate(Resources.Load(_scene, typeof(ScriptableScene))) as ScriptableScene;

        if (instance != null)
        { 
            
        }

    }


}
