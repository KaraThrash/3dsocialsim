using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;


public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get => singleton.instance;
    }

    private static GlobalSingletonGetter<GameManager> singleton =
        new GlobalSingletonGetter<GameManager>(gameObjectName: "GameManager");

    public GameState gameState;
    public Player player;
    public DialogueRunner dialogueRunner;
    public YarnFunctions yarnFunctions;
    public TerrainManager terrainManager;
    public TimeManager timeManager;
    public UiManager uiManager;
    public AudioManager audioManager;
    public CameraControls cameraControls;
    public ItemManager itemManager;
    public LocationManager locationManager;
    public SceneDirector sceneDirector;
    public StoryTracking storyTracker;

    public ScriptableScene activeScene;


    public GameObject chatbox,showCameraItem;
    public Text chatText,titleText;//for character names

    public Transform cam,activeObject, groundParent,villagerParent;
    public GameObject emoteBubblePrefab,dirtsquare, grassSquare;

    public bool acceptInput;
    private bool inConversation,playerCanMove;

    private EventInit eventInit;
    public float actionTimer,transitionTimer;

    public Button continueButton;

    public Vector3 pendingNewPosition; //apply at the end of the transitioning step

    public UnityEvent pendingEvent;

    // Start is called before the first frame update
    void Start()
    {
        


        UnlockPlayerMovement(true);

        StartDay();
    }

    public void ActionTimeLockout()
    {
        actionTimer -= Time.deltaTime;
        if (actionTimer <= 0 && actionTimer != -1)
        {
            actionTimer = 0;
            player.State(PlayerState.playerControlled);
            cameraControls.ConversationToggle(false);
            //if (player.State() == PlayerState.animating)
            //{
            //    cameraControls.ConversationToggle(false);

            //}
            //player.State(PlayerState.playerControlled);

            //PlayerStateTransition();
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (actionTimer > 0)
        {
            ActionTimeLockout();
        }

        if (transitionTimer > 0)
        {
            transitionTimer -= Time.deltaTime;

            if (transitionTimer <= 0)
            {
                transitionTimer = 0;
                State(GameState.free);
                
              //  cameraControls.SetLocation(player.transform.position);
            }
        }



        GameStateSwitch();


        //for spawning sound clouds
        AudioManager().TrackSilenceTime();



        if (Input.GetKey(KeyCode.L))
        {
            TimeManager().AdvanceTime(5);
         

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();


        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(0);


        }

    }

    public void GameStateSwitch()
    {

        if (State() == GameState.free)
        { }
        else if (State() == GameState.transitioning) 
        { }
        else if (State() == GameState.scripted)
        { }
        else if (State() == GameState.talking)
        { }
    }




    public void AdvanceSceneAnimator()
    {
        //for calling from yarn to advance the scene aniamtor
        if (activeScene != null)
        {
            activeScene.SetAnimatorParameter("advance");
        }
    }

    public void SceneSpecificAction()
    {
        //for calling from yarn to advance the scene aniamtor
        if (activeScene != null)
        {
            activeScene.SceneSpecificAction();
        }
    }

    public void EndScene()
    {
        SceneDirector().EndScene();
    }


    public void PlaySoundEffect(string _clip)
    {
        AudioManager().PlaySoundEffect(_clip);
    }


    public void MovePlayerAndSpeaker(string _location)
    {
        //maintain any speaker

        //when called from a yarnfunction get the location from the provided string
        MovePlayer(LocationManager().FindLocation(_location));

    }


    public void MovePlayer(string _location)
    {
       
        //TODO: end conversations and interactions

        //when called from a yarnfunction get the location from the provided string
        MovePlayer(LocationManager().FindLocation(_location));

    }
    

    

    public void MovePlayer(Transform _location)
    {
       // if (sceneDirector.sceneActive == true) { SceneDirector().EndScene(); }
        EventInit().MovePlayer(GetComponent<GameManager>(),_location);

    }

    public void SetConversationTargetLocation(string _location)
    {


        //when called from a yarnfunction get the location from the provided string
        SetConversationTargetLocation(LocationManager().FindLocation(_location));

    }

    public void SetConversationTargetLocation(Transform _location)
    {

        // set where the villager needs to walk to before the conversation can continue
        //disables the continue button until arrival
        EventInit().SetCheckPointLocation(GetComponent<GameManager>(), _location);

    }
    public void LeadPlayer(string _location,string _villagerName)
    {

        //TODO: end conversations and interactions

        //when called from a yarnfunction get the location from the provided string
        LeadPlayer(LocationManager().FindLocation(_location), FindVillager(_villagerName));

    }

    public void LeadPlayer(string _location, string _villagerName,string _lineCount)
    {

        //TODO: end conversations and interactions

        //when called from a yarnfunction get the location from the provided string
        LeadPlayer(LocationManager().FindLocation(_location), FindVillager(_villagerName), Int32.Parse(_lineCount));

    }

    public void LeadPlayer(string _location, string _villagerName, string _lineCount,string _speed)
    {

        //TODO: end conversations and interactions

        //when called from a yarnfunction get the location from the provided string
        LeadPlayer(LocationManager().FindLocation(_location), FindVillager(_villagerName), Int32.Parse(_lineCount), float.Parse(_speed));

    }

    public void LeadPlayer(Transform _location,Villager _villager,int _lineCount=1,float _speed=1)
    {
       // if (SceneDirector().sceneActive == true) { SceneDirector().EndScene(); }

        EventInit().LeadPlayer(GetComponent<GameManager>(), _villager,_location.position, _lineCount, _speed);

    }

    public void HavePlayerFollow(string _location, string _villagerName)
    {
      //  if (sceneDirector.sceneActive == true) { SceneDirector().EndScene(); }
        EventInit().HavePlayerFollow(GetComponent<GameManager>(), FindVillager(_villagerName), LocationManager().FindLocation(_location).position);

    }


    public void ToggleMenu(Menu _menu)
    {
        //if toggling the menu on set the state to inmenu, otherwise give control back to the player
        if (UiManager().OpenMenu(_menu))
        {
            player.State(PlayerState.inMenu);
        }
        else {
            actionTimer = 0.1f;
            //player.State(PlayerState.playerControlled);
        }
    }


    public void OnDialogueStart()
    {

    }

    public void OnDialogueEnd()
    {
        actionTimer = 0.2f;
        Time.timeScale = 1;
    }





    public void BonkVillager(Villager _villager)
    {
        //todo: check for scene state here
        _villager.Bonk();

    }

    public void InteractWithVillager(Villager _villager)
    {

        
            //the next major story step in the day starts with this villager
          //  YarnProgram nextStory= StoryTracker().StartSection();
           // Debug.Log(nextStory);

           // if (nextStory != null)
           // {
                //the next major story step in the day starts with this villager
             //   dialogueRunner.Add(nextStory);
                if (inConversation == false)
                {
                    StartConversation();
                    audioManager.PlayWorldEffect(_villager.Voice());
                }

                string nodeTitle = "day" + StoryTracker().GetDay().ToString() + _villager.VillagerName().ToString().ToLower();
        Debug.Log(nodeTitle);
        ActiveObject(_villager.transform);
                dialogueRunner.StartDialogue(nodeTitle);
           // }
        //if (_villager.VillagerName() == StoryTracker().NextNarrativeActor())
        //{
        //}
        //else if (_villager.scriptToLoad != null)
        //{
        //    if (inConversation == false)
        //    {
        //        StartConversation();
        //        audioManager.PlayWorldEffect(_villager.Voice());
        //    }

        //    ActiveObject(_villager.transform);
        //    //  dialogueRunner.Add(_villager.scriptToLoad);
        //    //  _villager.scriptToLoad = null;

        //    //find the script for this village based on the game state

        //    //todo: contextual checks
        //    //yarn wants a string for the title of the dialogue
        //    dialogueRunner.StartDialogue(_villager.name);

        //    // _villager.Interact();
        //}
        //else
        //{

        //    // _villager.Bonk();
        //}

        _villager.Interact();
    }


    public void InteractWithGround(Vector3 _square, string _interaction,GameObject _contextItem=null)
    {
        _square = new Vector3(_square.x, Mathf.Round(_square.y), _square.z);

        if (_interaction == "dig")
        {
            if (terrainManager.Dig(_square))
            {
                actionTimer = 0.5f;
                //player.state = PlayerState.acting;
            }

        }
      

    }

    public void CatchBug(Item _bug)
    {
        ActiveObject(_bug.transform);

        player.State(PlayerState.acting);

        actionTimer = 5;

        _bug.SubObject().gameObject.SetActive(false);

        player.SetPendingItem(_bug);
        player.SetKinematic(true);


        if (dialogueRunner.NodeExists(_bug.itemName))
        {
            //the background shouldnt remain active while getting an item
        //    Time.timeScale = 0;
            dialogueRunner.StartDialogue(player.pocketPendingItem.itemName);
            StartConversation();
        }



    }


    public void ShowItem()
    {
        player.State(PlayerState.showing);
        Item pendingItem = player.pocketPendingItem;

        if (pendingItem != null)
        {
            player.pocketPendingItem.SubObject().gameObject.SetActive(true);
            player.HoldToCamera(player.pocketPendingItem.SubObject());
        }

        cameraControls.ConversationToggle(true);

        
        
    }






    public void EnterArea(GameObject _interiorObj, GameObject _connectedArea,string _type="")
    {
       

        if (_type.Equals("building"))
        {
            cameraControls.SetCameraTrackingOffset("inside");
            player.inside = true;

        }
        else if (_type.Equals("lostwoods"))
        {
            cameraControls.SetCameraTrackingOffset("lostwoods");
            cameraControls.EndCameraEffect("lostwoods");
            if (_interiorObj.GetComponent<LostWoods>() != null)
            {
                //_interiorObj.GetComponent<LostWoods>().StartLostWoods();

                //40 is the middle of the map TODO: set this to a variable when you defeine the map layout
                EventInit().StartLostWoods(this, player.transform.position.z > 40);

            }
            else {  }

            return;
        }

        EventInit().EnterBuilding(GetComponent<GameManager>(),_connectedArea.transform);

        //actionTimer = 1.35f;
        //transitionTimer = 1.3f;

        //State(GameState.transitioning);

        //player.State(PlayerState.acting);

       // TerrainManager().EnterBuilding(_interiorObj, _connectedArea);

       // pendingNewPosition = new Vector3(_connectedArea.transform.position.x, _connectedArea.transform.position.y, _connectedArea.transform.position.z);
        //player.transform.position = new Vector3(_connectedArea.transform.position.x, _connectedArea.transform.position.y, _connectedArea.transform.position.z);
        //cameraControls.SetLocation(_connectedArea.transform.position);


        if (_connectedArea.transform.parent != null)
        {
           // cameraControls.SetLocation(_connectedArea.transform.parent.position);

        }
        else
        {
            //cameraControls.SetLocation(_connectedArea.transform.position);

        }

    }

    public void ExitArea(GameObject _interiorObj, GameObject _connectedArea, string _type = "")
    {


        if (_type.Equals("building"))
        {
            cameraControls.SetCameraTrackingOffset("low");
            player.inside = false;

        }
        else if (_type.Equals("lostwoods"))
        {
            cameraControls.SetCameraTrackingOffset("lostwoods");
            cameraControls.EndCameraEffect("lostwoods");
            if (_interiorObj.GetComponent<LostWoods>() != null)
            {
                //_interiorObj.GetComponent<LostWoods>().StartLostWoods();

                //40 is the middle of the map TODO: set this to a variable when you defeine the map layout
                EventInit().StartLostWoods(this, player.transform.position.z > 40);

            }
            else { }

            return;
        }

        EventInit().ExitBuilding(GetComponent<GameManager>(), _connectedArea.transform);



    }



    public void EnterBuilding(GameObject _interiorObj, GameObject _connectedArea,string _camSetting="")
    {
        actionTimer = 1.2f;
       // player.State(PlayerState.acting);

        //  TerrainManager().EnterBuilding(_interiorObj,_connectedArea);

        player.transform.position = new Vector3(_connectedArea.transform.position.x, -20, _connectedArea.transform.position.z);


        if (_camSetting.Equals("building"))
        {
            cameraControls.SetCameraTrackingOffset("inside");
            player.inside = true;

        }
        else if (_camSetting.Equals("lostwoods"))
        {
            cameraControls.SetCameraTrackingOffset("lostwoods");
            cameraControls.EndCameraEffect("lostwoods");
            if (_interiorObj.GetComponent<LostWoods>() != null)
            {
                //_interiorObj.GetComponent<LostWoods>().StartLostWoods();

                //40 is the middle of the map TODO: set this to a variable when you defeine the map layout
                EventInit().StartLostWoods(this, player.transform.position.z > 40);

            }
            else { }

            return;
        }


        if (_connectedArea.transform.parent != null)
        {
            cameraControls.SetLocation(_connectedArea.transform.parent.position);

        }
        else 
        {
            cameraControls.SetLocation(_connectedArea.transform.position);

        }

    }

    public void LeaveBuilding()
    {
        actionTimer = 0.2f;
     //   player.state = PlayerState.acting;

        TerrainManager().LeaveBuilding();
        player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        player.inside = false;

        cameraControls.EndCameraEffect("lostwoods");

        cameraControls.SetCameraTrackingOffset("high");
        cameraControls.SetLocation(player.transform.position);
    }

    public void EnterRoom(GameObject _connectedArea)
    {
        actionTimer = 0.1f;
       //player.state = PlayerState.acting;

        TerrainManager().RoomChange(_connectedArea);

        player.transform.position = new Vector3(_connectedArea.transform.position.x, player.transform.position.y, _connectedArea.transform.position.z);

        cameraControls.SetCameraTrackingOffset("inside");
        cameraControls.SetLocation(_connectedArea.transform.parent.position);

    }


    public void UnlockPlayerMovement(bool _canMove)
    {
        playerCanMove = _canMove;
    }





    public void StartConversation()
    {
        // cameraControls.ConversationToggle(true);



        //chatbox.SetActive(true);
        player.SetKinematic(true);
        inConversation = true;
        actionTimer = -1;
        player.State(PlayerState.talking);
    }

    public void EndConversation()
    {
        cameraControls.ConversationToggle(false);
        // chatbox.SetActive(false);
        //if (player.State() == PlayerState.showing)
        //{
        //    actionTimer = 1.0f;
        //    player.State(PlayerState.animating);
        //}
        //else
        //{
        //    inConversation = false;
        //    actionTimer = 0;
        //    player.State(PlayerState.playerControlled);
        //}
        inConversation = false;
        actionTimer = 0;
        player.State(PlayerState.playerControlled);

        if (activeObject != null && activeObject.GetComponent<Villager>() != null)
        { activeObject.GetComponent<Villager>().State(VillagerState.idle); }

    }

    public void ShowDialogue(string _line)
    {
        if (_line.Length == 0)
        {
            activeObject = null;
            EndConversation();
            return;
        }
        chatText.text = _line;
        titleText.text = "";
    }

    public void ShowDialogue(string _speakerName, string _line)
    {
        if (_line.Length == 0)
        {
            activeObject = null;
            EndConversation();
            return;
        }
        chatText.text = _line;
        titleText.text = _speakerName;
    }


    public void StartDay()
    {
        //music clouds
        //villager locations
        //reset fruit, bugs, fish?
       // TimeManager().SetSunAngle(new Vector3(TimeManager().GetDay() * -15,0,171));


        foreach (Transform el in villagerParent)
        {
            if (el.GetComponent<Villager>() != null)
            {
                el.GetComponent<Villager>().ResetToStart();

            }
        }


    }




    public Player GetPlayer()
    { return player; }


    //for cam/player focus when interacting
    public Transform GetActiveObject()
    {
        //if the activeobject isnt set, focus on the camera
        if (activeObject == null) { return cam; }
        return activeObject; 
    }



    public UiManager UiManager()
    {
        if (uiManager == null) { uiManager = FindObjectOfType<UiManager>(); }
        return uiManager;
    }

    public TerrainManager TerrainManager()
    {
        if (terrainManager == null) { terrainManager = FindObjectOfType<TerrainManager>(); }
        return terrainManager;
    }

    public TimeManager TimeManager()
    {
        if (timeManager == null) { timeManager = FindObjectOfType<TimeManager>(); }
        return timeManager;

    }

    public AudioManager AudioManager()
    {
        if (audioManager == null) { audioManager = FindObjectOfType<AudioManager>(); }
        return audioManager;
    }

    public SceneDirector SceneDirector()
    {
        if (sceneDirector == null) { sceneDirector = FindObjectOfType<SceneDirector>(); }
        return sceneDirector;
    }


    public EventInit EventInit()
    {

        if (eventInit == null)
        { eventInit = GetComponent<EventInit>(); }

        return eventInit;
    }

    public LocationManager LocationManager()
    {

        if (locationManager == null)
        { locationManager = FindObjectOfType<LocationManager>(); }

        return locationManager;
    }

    public ItemManager ItemManager()
    {

        if (itemManager == null)
        { itemManager = FindObjectOfType<ItemManager>(); }

        return itemManager;
    }

    public StoryTracking StoryTracker()
    {

        if (storyTracker == null)
        { storyTracker = FindObjectOfType<StoryTracking>(); }

        return storyTracker;
    }

    public bool DialogueIsRunning()
    {
        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning == true)
        {
            return true;
        }
        return false;
    }

    public void StartDialogue(string _name)
    { 
        dialogueRunner.StartDialogue(_name); 
    }




    public void State(GameState _state)
    {
        OnGameStateChange(_state);

        gameState = _state;
    }

    public GameState State()
    { return gameState; }


    public void OnGameStateChange(GameState _state)
    {
        //old state is same as the new state
        if (State() == _state) { return; }

        if (State() == GameState.transitioning) 
        {

         




            if (activeObject != null)
            {
                if (activeObject.GetComponent<Villager>() != null)
                {
                    Vector3 _pos = (activeObject.position - player.transform.position) + pendingNewPosition;
                    activeObject.GetComponent<Villager>().WarpNavMesh(new Vector3(_pos.x, activeObject.position.y, _pos.z));
                }
            }

            //dont move the player until the transition effect is over (e.g. fading to black, dont move until the screen is fully black) 
            player.TeleportPlayer(pendingNewPosition);

            player.SetVelocities(Vector3.zero,Vector3.zero);

            cameraControls.SetLocation(player.transform.position);


            if (SceneDirector().sceneActive == false)
            {
                acceptInput = true;
                //   dialogueRunner.GetComponent<DialogueUI>().acceptsInput = true;

                player.State(PlayerState.playerControlled);

            }

            return;
        }

        if (_state == GameState.transitioning) 
        { 
            acceptInput = false;
           //   dialogueRunner.GetComponent<DialogueUI>().acceptsInput = false;
        }
        

    }











    public Button ContinueButton()
    {
        if (continueButton == null)
        { continueButton = dialogueRunner.GetComponent<DialogueUI>().dialogueContainer.transform.Find("Continue Button").GetComponent<Button>(); }
        return continueButton;


    }

    public void SetContinueButton(bool _on)
    {
        if (continueButton == null)
        { continueButton = dialogueRunner.GetComponent<DialogueUI>().dialogueContainer.transform.Find("Continue Button").GetComponent<Button>(); }

        ContinueButton().GetComponent<Image>().enabled = _on;
        ContinueButton().transform.GetChild(0).GetComponent<Text>().enabled = _on;
        ContinueButton().interactable = _on;
        ContinueButton().Select();


    }

    public void SetDialogueBox(bool _on)
    {
        dialogueRunner.GetComponent<DialogueUI>().dialogueContainer.gameObject.SetActive(_on);


    }



    public Transform ActiveObject()
    {
        if (activeObject != null) { return activeObject; }
        return transform;
    }

    public void ActiveObject(Transform _obj)
    {
         activeObject = _obj;
    }

    public Villager FindVillager(string _name)
    {
        if (villagerParent == null)
        {
            villagerParent = transform.Find("VillagerParent");
        }

        foreach (Transform el in villagerParent)
        {
            if (el.GetComponent<Villager>() != null && el.GetComponent<Villager>().npcName.Equals(_name))
            { return el.GetComponent<Villager>(); }
        }


        return null;
    }

    public Villager FindVillager(Villagers _name)
    {
        foreach (Transform el in villagerParent)
        {
            if (el.GetComponent<Villager>() != null && el.GetComponent<Villager>().VillagerName() == _name)
            { return el.GetComponent<Villager>(); }
        }


        return null;
    }


    public void MakeGroundGrid()
    {
        int xpos = -20, zpos = -20;

        while (zpos < 21)
        {
            xpos = -20;
            while (xpos < 21)
            {
                if (zpos % 5 == 0 || xpos % 5 == 0)
                { Instantiate(dirtsquare, new Vector3(xpos, -0.5f, zpos), transform.rotation); }
                else
                { Instantiate(grassSquare, new Vector3(xpos, -0.5f, zpos), transform.rotation); }

                xpos++;
            }
            zpos++;
        }
    }
}
