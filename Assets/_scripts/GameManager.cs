using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get => singleton.instance;
    }

    private static GlobalSingletonGetter<GameManager> singleton =
        new GlobalSingletonGetter<GameManager>(gameObjectName: "GameManager");


    public Player player;
    public TerrainManager terrainManager;
    public TimeManager timeManager;
    public UiManager uiManager;
    public AudioManager audioManager;
    public CameraControls cameraControls;
    public GameObject chatbox,showCameraItem;
    public Text chatText,titleText;//for character names

    public Transform cam,activeObject, groundParent,villagerParent;
    public GameObject emoteBubblePrefab,dirtsquare, grassSquare;

    private bool inConversation,playerCanMove;
    private float actionTimer;
    // Start is called before the first frame update
    void Start()
    {
        UnlockPlayerMovement(true);
        StartDay();
    }

    // Update is called once per frame
    void Update()
    {
        if (actionTimer > 0)
        {
            actionTimer -= Time.deltaTime;
            if (actionTimer <= 0 && actionTimer != -1)
            { 
                actionTimer = 0;
                PlayerStateTransition();
            }
        }

        if (Input.GetKey(KeyCode.L))
        {
            TimeManager().AdvanceTime(5);
         

        }

    }

    public void PlayerStateTransition()
    {
        if (player.state == PlayerState.showing && activeObject != null)
        {
            cameraControls.ConversationToggle(false);
            //if the player is showing the item to camera it means there is room for it
            if (player.inventory.TryToAddItemToPockets(activeObject.GetComponent<Item>()))
            {
                player.inventory.PutItemInPocket(activeObject.GetComponent<Item>());
                activeObject.gameObject.SetActive(false);
            }

        }
        
        player.state = PlayerState.playerControlled;
    }


    public void ToggleMenu(string _menu)
    {
        //if toggling the menu on set the state to inmenu, otherwise give control back to the player
        if (UiManager().OpenMenu(_menu))
        { player.SetState(PlayerState.inMenu); }
        else { player.SetState(PlayerState.playerControlled); }
    }

    public void BonkVillager(Villager _villager)
    {
        //todo: check for scene state here
        _villager.Bonk();

    }

    public void InteractWithVillager(Villager _villager)
    {
        if (inConversation == false)
        {
            
            StartConversation();
            audioManager.PlayWorldEffect(_villager.Voice());
        }

        activeObject = _villager.transform;
        _villager.Interact();

    }


    public void InteractWithGround(Vector3 _square, string _interaction,GameObject _contextItem=null)
    {
        if (_interaction == "dig")
        {
            if (terrainManager.Dig(_square))
            {
                actionTimer = 1;
                player.state = PlayerState.acting;
            }

        }
       else if (_interaction == "chop")
        {
            if (terrainManager.Chop(_square))
            {
                actionTimer = 1;
                UnlockPlayerMovement(false);
            }
        }
        else if (_interaction.Equals("fish") )
        {
            if (terrainManager.Fish(_square, _contextItem))
            {
                actionTimer = -1;
                UnlockPlayerMovement(false);
            }
        }
        else if (_interaction.Equals("net"))
        {
            GameObject _obj = terrainManager.Catch(_square);
            if (_obj != null)
            {
                activeObject = _obj.transform;

                actionTimer = 1;

                cameraControls.ConversationToggle(true);

                player.SetState(PlayerState.showing);
                player.HoldToCamera(activeObject.GetChild(0));

               

            }
        }

    }

    public void EnterBuilding(GameObject _interiorObj, GameObject _connectedArea)
    {
        actionTimer = 1;
        player.state = PlayerState.acting;

        TerrainManager().EnterBuilding(_interiorObj,_connectedArea);
        player.transform.position = new Vector3(_connectedArea.transform.position.x, -20, _connectedArea.transform.position.z);

        player.inside = true;

        cameraControls.SetCameraTrackingOffset("inside");
        cameraControls.SetLocation(_connectedArea.transform.parent.position);

    }

    public void LeaveBuilding()
    {
        actionTimer = 1;
        player.state = PlayerState.acting;

        TerrainManager().LeaveBuilding();
        player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        player.inside = false;

        cameraControls.SetCameraTrackingOffset("high");
        cameraControls.SetLocation(player.transform.position);
    }

    public void EnterRoom(GameObject _connectedArea)
    {
        actionTimer = 0.1f;
        player.state = PlayerState.acting;

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
        cam.GetComponent<CameraControls>().ConversationToggle(true);
        chatbox.SetActive(true);
        inConversation = true;
        actionTimer = -1;
        player.state = PlayerState.talking;
    }

    public void EndConversation()
    {
        cam.GetComponent<CameraControls>().ConversationToggle(false);
        chatbox.SetActive(false);
        inConversation = false;
        actionTimer = 0;
        player.state = PlayerState.playerControlled;
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
        TimeManager().SetSunAngle(new Vector3(TimeManager().GetDay() * -15,0,171));


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
    { return uiManager; }

    public TerrainManager TerrainManager()
    { return terrainManager; }

    public TimeManager TimeManager()
    { return timeManager; }





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
