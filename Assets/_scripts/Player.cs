
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{

    public GameManager gameManager;
    public Animator anim;
    public NavMeshAgent nav;
    public AudioSource audioSource0;
    public AudioSource audioSource1;
    private bool  audioSourceToggle;

    public PlayerState state;
    public WorldLocation worldLocation;
    public string playerStateString;
    public float walkSpeed, rotSpeed;
    public float acceleration, deceleration;
    public float turnAngle; //buffer for when the player will start moving before facing the exact direction of travel 
    public bool inside;
    public Transform InHands;
    public GameObject heldItem,navLeadObject,pointOfInterest;
    public GameObject prevHeldItem; //to take the tool back out after the pick up screen
    private float poiRange=5;
    public Inventory inventory;
    public WorldUi placeholderActionText;


    public Transform head, animatedHead;
    public bool focusRig;
    public Rig rig;

    private bool eyesOpen, toggleToResetRepeatedAction; //e.g. player animation when appearing on camera, and do that everything they appear on camera
    public float blinkTimer;
    public SkinnedMeshRenderer leftEye, rightEye;
    public Material closedEye, openEye;

    public MouthController mouthAnimator;


    public Item pocketPendingItem;
    private int currentItem;
    private Vector3 moveDirection;

    private Rigidbody rb;

    private string AP_startFish = "start_fish";
    private string AP_walk = "walk";
    private string AP_speed = "speed";
    private string AP_stopAction = "stop_action";

    public PlayerState State (){ return state; }

    public void State(PlayerState _state) { OnStateChange(_state); state = _state; }

    public void OnStateChange(PlayerState _state)
    {
        Debug.Log("new player state: " + _state);

        //new state is same as the old state
        if (State() == _state) { return; }
        if (State() == PlayerState.showing)
        {
           
            if (pocketPendingItem != null)
            {
                //if the player is showing the item to camera it means there is room for it
                if (inventory.TryToAddItemToPockets(pocketPendingItem))
                {
                    inventory.PutItemInPocket(pocketPendingItem);
                }

                if (pocketPendingItem.stackSize == 0)
                {
                    Destroy(pocketPendingItem.gameObject);
                }
                else 
                {
                    pocketPendingItem.gameObject.SetActive(false);

                }

            }

         //   PlayAnimation(Animator(),"put_in_pocket");
            SetPendingItem( null);

            StopHidingItem();
        }


        if (_state == PlayerState.playerControlled) { SetKinematic(false); }
        if (_state == PlayerState.talking) { SetAnimationBool(anim, AP_walk, false); }
        else if (_state == PlayerState.inScene) { SetAnimationBool(anim, AP_walk, false); }
        else if (_state == PlayerState.fishing) { 
            SetAnimationBool(anim, AP_walk, false); 
            PlayAnimation(Animator(), AP_startFish); 
        }


    }


    public WorldLocation WorldLocation(){ return worldLocation; }
    public void WorldLocation(WorldLocation _location){  worldLocation = _location; }



    public Vector3 PositionRounded( )
    {
       return new Vector3(Mathf.RoundToInt(transform.position.x ), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
    }

    public Vector3 PositionRounded(Vector3 _offset)
    {
        return new Vector3(Mathf.RoundToInt(transform.position.x + _offset.x), Mathf.RoundToInt(transform.position.y + _offset.y), Mathf.RoundToInt(transform.position.z + _offset.z));
    }

    public void SetText(string text="")
    {
        if (placeholderActionText != null && text.Equals("") == false)
        {
            placeholderActionText.SetText(text);
        }

    }


    void Start()
    {

      //  PlayAnimation(anim,AP_startFish);
       // SetText("game start");

        mouthAnimator = GetComponent<MouthController>();
        rb = GetComponent<Rigidbody>();
        foreach (Item el in inventory.itemsInStorage)
        {
            GameObject newitem = Instantiate(el.gameObject,Vector3.zero,transform.rotation);
            inventory.itemsInPockets.Add(newitem.GetComponent<Item>());
            newitem.SetActive(false);
        }

    }

  
    void Update()
    {
        

        //TODO: find a better way to do this
        if (gameManager.DialogueIsRunning() == true)
        {
            //return;
        }

        //for debug porpoises
        playerStateString = state.ToString();
        
        PlayerStates();




        if (leftEye != null)
        { BlinkTimer(); }


        if (InputControls.MenuButton())
        { gameManager.ToggleMenu(Menu.radial); }

    }




    public void PlayerStates()
    {
        if (state == PlayerState.playerControlled)
        {
            if (InputControls.PickUpButton())
            {
                PickUp();
            }

            if (InputControls.NextButton())
            { NextItem(1); }

            if (InputControls.PreviousButton())
            { NextItem(-1); }

            PlayerControlled();

            FocusPOI(pointOfInterest);

        }
        else if (state == PlayerState.talking)
        {
            Talking();
        }
        else if (state == PlayerState.fishing)
        {
            Fishing();



        }
        else if (state == PlayerState.inMenu)
        {

            SetVelocities(Vector3.zero, Vector3.zero);
            InMenuControls();

        }
        else if (state == PlayerState.showing)
        {
            if (poiRange < 10)
            {
                poiRange = 10;
            }

            SetVelocities(Vector3.zero, Vector3.zero);

            GameObject cam = GameManager.instance.cam.gameObject;
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(cam.transform.position.x, transform.position.y, cam.transform.position.z) - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);

            FocusPOI(GameManager.instance.cam.gameObject);

        }
        else if (state == PlayerState.acting)
        {



        }
        else if (state == PlayerState.inScene)
        {



        }
        else { }
        return;
        // else { SetVelocities(Vector3.zero, Vector3.zero); }

        if (nav != null && nav.enabled)
        {
            if (nav.velocity.magnitude > 0.1f)
            {
                SetAnimationBool(anim, AP_walk, true);
                SetAnimationParameter(anim, AP_speed, nav.velocity.magnitude * 0.5f);
                anim.speed = Mathf.Clamp(nav.velocity.magnitude * 0.7f, 0.5f, walkSpeed);
            }
            else {
                anim.speed = 1;
                SetAnimationBool(anim, AP_walk, false);
            }
        }
        else 
        {
            if (rb.velocity.magnitude > 0.1f)
            {
                SetAnimationBool(anim, AP_walk, true);
                SetAnimationParameter(anim,AP_speed, rb.velocity.magnitude * 0.5f);
                anim.speed = Mathf.Clamp(rb.velocity.magnitude * 0.5f, 0.5f, walkSpeed);
            }
            else {
                anim.speed = 1;
                SetAnimationBool(anim, AP_walk, false);
            }
        }

        
    }


    public void PlayerControlled()
    {


        if (InputControls.InteractButton())
        {
            Interact();

            //if (heldItem == null || heldItem.GetComponent<Item>().usable == false)
            //{
            //}
            //else 
            //{
                
            //}
            
        
        }

        Movement();


    }



    public void Talking()
    {
        LookAtAction(gameManager.ActiveObject(),rotSpeed);
        SetVelocities(Vector3.zero, Vector3.zero);
    }



    public void InMenuControls()
    {
        if (InputControls.HortAsButton()) { gameManager.UiManager().MoveCursor((int)Mathf.Sign(InputControls.HorizontalAxis()),0); }

        else if(InputControls.VertAsButton()) { gameManager.UiManager().MoveCursor(0,-1 * (int)Mathf.Sign(InputControls.VerticalAxis())); }

        else if(InputControls.DpadVertAsButton()) { gameManager.UiManager().MoveCursor(0,-1 * (int)Mathf.Sign(InputControls.DpadVert())); }

        else if(InputControls.NextButton()) { gameManager.UiManager().MoveCursor(0, 1); }
        else if(InputControls.PreviousButton()) { gameManager.UiManager().MoveCursor(0, -1); }

        else if(InputControls.DpadHortAsButton()) { gameManager.UiManager().MoveCursor( (int)Mathf.Sign(InputControls.DpadHort()), 0); }
        else {  }

        InputControls.TrackAxisButtons();
    }



    public void Movement()
    {

        moveDirection = Vector3.right * InputControls.HorizontalAxis();
        moveDirection = moveDirection + (Vector3.forward * InputControls.VerticalAxis());

        Walk(moveDirection,walkSpeed );

    }

    public void Walk(Vector3 _dir,float _speed)
    {
        if (_dir.magnitude > 0)
        {

            if (rb.velocity == Vector3.zero) 
            {
                SetAnimationBool(anim, AP_walk, true);
            }


            moveDirection = _dir; 

            //rebalance the speed for the input, avoid the goldeneye diagonal speed multiplier while also remaining still with no input
            if (_dir.magnitude > 1)
            { moveDirection = (_dir).normalized; }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), rotSpeed * Time.deltaTime);


            float angle = Vector3.Angle(moveDirection , transform.forward);

            //TODO: make a constant for the dead movement zone
            if (_dir.magnitude < 0.75f && rb.velocity.magnitude < 0.1f)
            {
                //this allows the player to rotate in place without moving the character
                return;

            }

           


            

           


            RaycastHit hit;
            if (Physics.SphereCast(transform.position + new Vector3(0, 0.5f, 0), 0.2f, transform.forward, out hit, 0.2f))
            {
                rb.velocity = Vector3.zero;

            }
            else if (Physics.SphereCast(transform.position + moveDirection + new Vector3(0, 0.5f, 0), 0.2f, Vector3.down, out hit, 2.2f) && hit.transform.tag == "water")
            {
                rb.velocity = Vector3.zero;

            }
            else 
            {
                

                //the stutter direction change while running from animal crossing
                if (angle > turnAngle)
                {
                    if (rb.velocity.magnitude > 0)
                    {
                      //  rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * deceleration);

                        if (rb.velocity.magnitude <= 0.05f)
                        {
                            rb.velocity = Vector3.zero;
                            rb.angularVelocity = Vector3.zero;
                            //SetNavMeshSpeed(0);
                            //  SetNavDestination(transform.position);
                        }
                    }
                }
                else { rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * _speed * moveDirection.magnitude, Time.deltaTime * acceleration); }
            }

            anim.speed = (rb.velocity.magnitude * 2) / walkSpeed;
            SetAnimationParameter(anim, AP_speed, rb.velocity.magnitude / walkSpeed);

        }
        else
        {
            //no movement input, slow to a stop
            if (rb.velocity.magnitude > 0)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * deceleration);
                anim.speed = (rb.velocity.magnitude * 2) / walkSpeed;
                SetAnimationParameter(anim, AP_speed, rb.velocity.magnitude / walkSpeed);
                if (rb.velocity.magnitude <= 0.05f)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    SetAnimationBool(anim, AP_walk, false);
                    anim.speed = 1;
                    //SetNavMeshSpeed(0);
                    // moveDirection = _dir;

                    //  SetNavDestination(transform.position);
                }
            }

        }
    }




    public void MoveTo(Vector3 _moveDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        float angle = Vector3.Angle((transform.position + moveDirection) - transform.position, transform.forward);

        //larger turnAngle will have a rounder run arc instead of angular turns
        if (angle < turnAngle)
        {

            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * walkSpeed, Time.deltaTime * acceleration);

        }

    }


    public void TeleportPlayer(Vector3 _pos) 
    {
        transform.position = _pos;


        if (nav != null && nav.enabled == true)
        {
            WarpNav(_pos);
        }
        else 
        {
                

        }


    }




    //conversation target, item pickup, or directly at the camera
    public void LookAtAction(Transform _lookat,float _rotSpeed=1)
    {
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_lookat.position.x, transform.position.y, _lookat.position.z) - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
    }




    public bool HasItem(ItemClass _item)
    {
        //in case there are narrative items, system items, or other conditional checks we might need the 'check if in iventory is called here
        foreach (Item el in inventory.itemsInPockets)
        {
            if (el.itemClass == _item) { return true; }
        }

        return false;
    }


    public void PickUp()
    {
        Vector3 squarePos = new Vector3(Mathf.RoundToInt((transform.position + (transform.forward * 0.3f)).x), Mathf.FloorToInt(transform.position.y), Mathf.RoundToInt((transform.position + (transform.forward * 0.3f)).z));

        if (gameManager.TerrainManager().GetMapSquare(squarePos) == null) {
          //  Debug.Log("No square saved at : " + squarePos); 
            return; 
        }
        //check if there is an item
        //check that it can be picked up
        TerrainSquare _square = gameManager.TerrainManager().GetMapSquare(transform.position + (transform.forward * 0.3f));

        Item _item = _square.GetItem();
        if (_item != null)
        {
            //check that it can stack and if the player has a not full stack of it
            if (inventory.TryToAddItemToPockets(_item))
            {
                inventory.PutItemInPocket(_item);
                _square.SetItem(null);
                _item.gameObject.SetActive(false);
            }
            else
            {
                //the item didnt fit in the player's pockets
                if (_item.stackSize > 0)
                {
                    //TODO: no room messaging
                }
            }


            //check that the player has room
            //if room, pick up

            if (_item.stackSize == 0)
            {
                _square.SetItem(null);
                _item.gameObject.SetActive(false);
            }

        }
      //  else { Debug.Log("No item saved at : " + squarePos); }



    }

    public void HideHeldItem()
    {
        //briefly stop showing the tool used when showing off a new item
        if (heldItem != null)
        {
            prevHeldItem.gameObject.SetActive(false);
            prevHeldItem = heldItem;
        }

    }

    public void StopHidingItem()
    {
        //if a tool ws hidden during pickup bring it back out
        if (prevHeldItem != null)
        {
            prevHeldItem.gameObject.SetActive(true);
            prevHeldItem = null;
        }
    }



    //Interact with characters and objects, Use tool or item, TODO:Move furniture(Press and hold A)
    public void Interact()
    {

        Debug.Log("Interact");


        //try to talk first, then try to use an item the player is in front of, otherwise if they have a tool in their hand, use it.

        //check forward first, then check down

       // Vector3 fwd = (transform.position + (Vector3.up * 0.5f)) - (transform.position + (transform.forward * 0.5f));

       
       // if (Physics.SphereCast(transform.position + (Vector3.up * 0.5f), 0.2f, transform.TransformDirection(Vector3.forward), out hit, 0.3f))
       
           
        if (InteractWithVillager())
        {

        }
        else if (heldItem != null && heldItem.activeSelf && heldItem.GetComponent<Item>() != null)
        {

            if (heldItem.GetComponent<Item>().TryCatch())
            {

                return;
            }
            else 
            {
                    heldItem.GetComponent<Item>().Use(this);
            }


        }
        else
        {
            RaycastHit hit;

                if (Physics.SphereCast(transform.position + (Vector3.up * 0.65f), 0.05f, transform.TransformDirection((Vector3.down + Vector3.forward + Vector3.forward).normalized), out hit, 3.5f))
                {
                    if (hit.transform.GetComponent<Item>() != null && hit.transform.GetComponent<Item>().Interact(this))
                    {
                    }
                    

                }

        }
        //if no one to talk to check hands for tools
        //     UseTool();




    }

    public bool InteractWithVillager()
    {


        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Villager");
        Vector3 dir = Vector3.down + (Vector3.forward * 3);//transform.TransformDirection(dir.normalized)

        if (Physics.SphereCast(transform.position + (Vector3.up * 0.65f), 0.05f, transform.forward, out hit, 3.5f, mask))
        {

            if (hit.transform.GetComponent<Villager>() != null)
            {

                if (heldItem != null && heldItem.GetComponent<Net>())
                {
                    gameManager.BonkVillager(hit.transform.GetComponent<Villager>());
                    
                }
                else
                {
                    gameManager.InteractWithVillager(hit.transform.GetComponent<Villager>());

                }

                return true;
            }
        

        }

        return false;
    }


    public void OnTriggerStay(Collider other)
    {

        if (pointOfInterest == null && other.gameObject.tag == "POI")
        {
            SetPoi(other.transform);


        }
    }

    public void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.tag == "POI")
        //{
        //    rig.weight = 0;
        //}
    }

    public void SetPoi(Transform _poi)
    {
        float angle = Vector3.Angle(_poi.transform.position - transform.position, transform.forward);

        if (angle < 110)
        {
            pointOfInterest = _poi.gameObject;
            poiRange = Vector3.Distance(_poi.transform.position, transform.position) * 1.1f;


        }
    }

    public void FocusPOI(GameObject _poi=null)
    {
        if (_poi == null)
        {
            if (rig.weight > 0)
            {
                rig.weight = Mathf.Lerp(rig.weight, 0, Time.deltaTime);
            }
        }
        else 
        {
            head.position = _poi.transform.position;

            float angle = Vector3.Angle(_poi.transform.position - transform.position, transform.forward);

            if (angle > 175 || Vector3.Distance(_poi.transform.position,transform.position) > poiRange)
            {
                if (_poi == pointOfInterest)
                { 
                    pointOfInterest = null;
                }
            }
            else 
            {
                rig.weight = Mathf.Lerp(rig.weight, 1 - (Vector3.Distance(transform.position, _poi.transform.position) / poiRange), Time.deltaTime);
            }

        }


        
    }











    public void Fishing()
    {
        SetVelocities(Vector3.zero, Vector3.zero);


        if (InputControls.InteractButton())
        {
            if (heldItem != null)
            {
               
            }

            EndFish();


        }


    }

    public void EndFish()
    {
        //TODO: difference between bugs and fish -> catchbug adds to the inventory but its a spawned item not the prefab

        if (heldItem != null)
        {
            if (heldItem.GetComponent<FishingRod>() != null)
            {
                SetAnimationTrigger(Animator(), AP_stopAction);
                Item caughtFish = GameManager.instance.ItemManager().CatchFish(transform.position, heldItem.GetComponent<FishingRod>().currentFishChange);

                if (caughtFish != null)
                {

                    Item showFish = Instantiate(caughtFish, transform.position, transform.rotation);
                    GetItem(showFish);

                }
                else 
                {
                   
                    State(PlayerState.playerControlled);
                }

            }

            heldItem.GetComponent<Item>().SetSubItem(null);
            heldItem.GetComponent<Item>().ResetsubObjectPos();
        }
        
       // CatchBug();
       // State(PlayerState.playerControlled);
    }

    public void GetItem(Item _item)
    {
        

        GameManager.instance.CatchBug(_item);
    }





    public void HoldToCamera(GameObject _obj)
    {
        transform.LookAt(new Vector3(transform.position.x,transform.position.y,transform.position.z -10));
        _obj.transform.position = InHands.position;
        _obj.transform.rotation = InHands.rotation;
    }

    public void HoldToCamera(Transform _obj)
    {
       // transform.LookAt(new Vector3(transform.position.x, transform.position.y, transform.position.z - 10));

        _obj.position = new Vector3(InHands.position.x, InHands.position.y, InHands.position.z - 0.5f);
        //_obj.position = InHands.position;
        //_obj.rotation = InHands.rotation;
    }

    public void ChangeHeldItem()
    { 
    
    }

    public void NextItem(int _forward=1) //or -1 for previous item
    {
        if (inventory == null) { return; }

        

      //  inventory[currentItem].gameObject.SetActive(false);

        if (_forward == 0)
        {
            currentItem = 0;
            return;
        }

        currentItem += _forward;

        //if at the end of the inventory in either direction dont loop over until the player toggles again
        //this will prevent an infinite loop and allow the player to cycle through everything and back to empty hands
        if (currentItem < 0) { 
            currentItem = inventory.PocketItemsCount();
            if(heldItem != null)
            {
                heldItem.GetComponent<Item>().SetSubItem(null);
                heldItem.SetActive(false);
            }

            return;
        }
        if (currentItem >= inventory.PocketItemsCount()) 
        { 
            currentItem = -1; 

            if (heldItem != null) 
            {
                heldItem.GetComponent<Item>().SetSubItem(null);
                heldItem.SetActive(false);
            }

            return; 
        }

        Item tempItem = new Item();
        tempItem = inventory.GetFromPockets(currentItem);
        //if (tempItem == null) { return; }

        if (tempItem != null && tempItem.usable == true)
        {
            //swap the current item for the new one
          //  Debug.Log("LoadingItem: " + (tempItem.itemName));
            string itempath = "items/" + (tempItem.itemName);
            GameObject newheldItem = inventory.GetFromPockets(currentItem).gameObject;

            newheldItem = Instantiate(newheldItem, InHands.position, InHands.rotation);
            newheldItem.transform.parent = InHands.transform;

            newheldItem.transform.position = InHands.transform.position;
            newheldItem.transform.rotation = InHands.transform.rotation;

            //TODO: keep the loaded items/pocket items in a way that is more efficent
            if (heldItem != null)
            {
                heldItem.SetActive(false);
                heldItem.transform.parent = null;
                //Destroy(heldItem); 
            }
            
            heldItem = newheldItem;
            heldItem.SetActive(true);
        }
        else 
        {
           //only toggle through usable items
           NextItem(_forward); 
                
        }
       


    }

    public void SetHeldItem(Item _item)
    {

        //TODO: keep the loaded items/pocket items in a way that is more efficent
        if (heldItem != null) 
        {
            //Destroy(heldItem);
            heldItem.SetActive(false);
            heldItem.transform.parent = null;
        }

        //check if the item can be put into the players hands, or if there is
        // a context based interaction: e.g. if using a grub check for the fishing rod

        if (_item != null )
        {

            //TODO: check if its already loaded before going to the resource menu

            //swap the current item for the new one
            //  Debug.Log("LoadingItem: " + (tempItem.itemName));

            string itempath = "items/" + (_item.itemName);
            Item newheldItem = _item.Hold(this);

            if (newheldItem == null)
            {
                heldItem = null;
                return;
            }

            heldItem = newheldItem.gameObject;


            heldItem.transform.parent = InHands.transform;
            heldItem.transform.position = InHands.transform.position;
            heldItem.transform.rotation = InHands.transform.rotation;
            heldItem.SetActive(true);
        }

    }



    public void SetPendingItem(Item _item)
    {
        pocketPendingItem = _item;
    }

    public void SetVelocities(Vector3 vel, Vector3 angularVel)
    {
        rb.velocity = vel;
        rb.angularVelocity = angularVel;
    }


    public void SetNavLeadObject(Vector3 _dest,float _speed)
    {
        if (nav == null) { return; }

        if (nav.enabled == false)
        {
            nav.enabled = true;
            nav.Warp(transform.position);
            SetKinematic(true);
        }

        
        

        SetNavDestination(_dest);
        SetNavMeshSpeed(_speed);

    }

    public void EndNavLeadObject()
    {

        if (nav == null) { return; }

        if (nav.enabled == true)
        {
            nav.enabled = false;
            SetKinematic(false);
        }

  

    }


    public void SetKinematic(bool _kinematic)
    {
        if (rb == null)
        { rb = GetComponent<Rigidbody>(); }

        if (rb == null) { return; }

        rb.isKinematic = _kinematic;
    }

    public bool IsInside() { return inside; }
    public Vector3 MoveDirection() { return moveDirection; }



    public AudioSource AudioSource()
    {
        audioSourceToggle = !audioSourceToggle;

        if (audioSourceToggle)
        {
            return audioSource0;
        }
        else 
        {
            return audioSource1;
        }
    }

    public void PlayFootstep()
    {
        gameManager.AudioManager().PlayFootStep(transform, AudioSource());
    }



    public void ControlHeadFocus()
    {
        if (head == null || animatedHead == null || GameManager.instance.player == null)
        {
            if (rig != null)
            {
                rig.weight = 0;

            }
            return;
        }

        if (focusRig)
        {
            if (rig != null)
            {
                rig.weight = Mathf.Lerp(rig.weight, 1, Time.deltaTime);

            }


            head.position = Vector3.MoveTowards(head.position, new Vector3(GameManager.instance.player.transform.position.x, animatedHead.position.y, GameManager.instance.player.transform.position.z), 5 * Time.deltaTime);

        }
        else
        {
            if (rig != null)
            {
                rig.weight = Mathf.Lerp(rig.weight, 0, Time.deltaTime);
                head.position = Vector3.MoveTowards(head.position, animatedHead.position + transform.forward, 5 * Time.deltaTime);

            }
        }



    }






    public void AnimateMouth(MouthPattern _pattern, float _length)
    {
        if (mouthAnimator == null)
        { mouthAnimator = GetComponent<MouthController>(); }

        if (mouthAnimator != null)
        {
            mouthAnimator.SetMouthPattern(_pattern, _length);
        }
    }



    public void SetMouth(Mood _mood) { mouthAnimator.SetMouth(_mood); }


    public void BlinkTimer()
    {

        blinkTimer -= Time.deltaTime;

        if (blinkTimer <= 0)
        {
            if (eyesOpen)
            {
                eyesOpen = false;
                if (leftEye.materials.Length > 1)
                {
                    Material[] mats = new Material[2];

                    mats[0] = closedEye;
                    mats[1] = closedEye;
                    leftEye.materials = mats;
                    rightEye.materials = mats;
                }
                else
                {
                    leftEye.material = closedEye;
                    rightEye.material = closedEye;
                }

                blinkTimer = UnityEngine.Random.Range(0.01f, 0.5f);

            }
            else
            {
                eyesOpen = true;
                if (leftEye.materials.Length > 1)
                {
                    Material[] mats = new Material[2];

                    mats[0] = openEye;
                    mats[1] = openEye;
                    leftEye.materials = mats;
                    rightEye.materials = mats;
                }
                else
                {
                    leftEye.material = openEye;
                    rightEye.material = openEye;
                }
                blinkTimer = UnityEngine.Random.Range(1.0f, 6.0f);
            }



        }


    }
    public void RollEyes()
    {
        //timer += Time.deltaTime;
        //if (timer >= rolltime)
        //{
        //    timer = 0;
        //    if (count >= eyesLeft.Count)
        //    {
        //        count = 0;
        //    }



        //    mats = new Material[1];

        //    //   obj1.GetComponent<SkinnedMeshRenderer>().material = leftEyeMaterial;
        //    leftEyeMaterial.SetTexture("Texture2D_72426835", eyesLeft[count]);
        //    rightEyeMaterial.SetTexture("Texture2D_72426835", eyesRight[count]);


        //    count++;
        //}

    }


    public void MoveNavmesh(float _speed = 5,float _minagle=15)
    {
        if (nav == null) { return; }

        float angle = Vector3.Angle(GetNavMeshSteeringTarget() - transform.position, transform.forward);

        if (angle > _minagle * 2)
        {
            SetNavMeshSpeed(1);


        }
        else if (angle <= _minagle)
        {
       //     SetNavMeshSpeed(Mathf.Lerp(nav.speed, _speed, Time.deltaTime * acceleration));
            SetNavMeshSpeed( _speed);


        }
        else
        {
           // SetNavMeshSpeed(Mathf.Lerp(nav.speed, _speed * 0.5f, Time.deltaTime * acceleration));
            SetNavMeshSpeed(_speed );
        }

        //SetNavMeshSpeed(Mathf.Lerp(nav.speed, _speed, Time.deltaTime * acceleration));
     //   SetNavMeshSpeed(walkSpeed);


    }



    public void SetNav(bool _on)
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        if (nav != null )
        {
            nav.enabled = _on;

        }
    }

    public void SetNavDestination(Vector3 dest)
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        //&& GetComponent<NavMeshAgent>().destination != dest && nav.enabled
        if (nav != null )
        {
            nav.enabled = true;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(dest, out hit, 1f, NavMesh.AllAreas))
            {
                GetComponent<NavMeshAgent>().destination = dest;
            }
            else 
            {
                Debug.Log("Navmesh: No hit -- trying to find a closer target");

                //if no hit, try to find one closer, but not infinitely
                if (dest.magnitude > 0.02f)
                { SetNavDestination(dest * 0.5f); }
            }



        }
    }

    public Vector3 GetNavDestination()
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        if (nav != null)
        {
            return GetComponent<NavMeshAgent>().destination;

        }
        return transform.position;
    }

    public void SetNavMeshSpeed(float _speed)
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        if (nav != null  && GetComponent<NavMeshAgent>().speed != _speed )
        {
            GetComponent<NavMeshAgent>().speed = _speed;

        }
    }


    public void WarpNav(Vector3 _warpTo)
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        if (nav != null  )
        {
            nav.enabled = true;
            nav.Warp(_warpTo);

            //NavMeshHit hit;
            //if (NavMesh.SamplePosition(_warpTo, out hit, 1f, NavMesh.AllAreas))
            //{
            //    nav.Warp(_warpTo);
            //}
            //else
            //{
            //    Debug.Log("Navmesh: No hit -- trying to find a closer target");

            //    //if no hit, try to find one closer, but not infinitely
            //    if (_warpTo.magnitude > 0.02f)
            //    { WarpNav(_warpTo * 0.5f); }
            //}

     


        }
    }

    public void SetNavVelocity(Vector3 _newvel)
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        if (nav != null && nav.enabled )
        {
            GetComponent<NavMeshAgent>().velocity = _newvel;


        }
    }

    public Vector3 GetNavVelocity()
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        if (nav != null )
        {
            return nav.velocity;


        }

        return Vector3.zero;
    }


    public Vector3 GetNavMeshSteeringTarget()
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        if (nav != null )
        {
            return GetComponent<NavMeshAgent>().steeringTarget;

        }
        //otherwise return forward
        return transform.position + transform.forward;
    }

    public Vector3 GetNavNextPosition()
    {
        if (nav == null)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        if (nav != null )
        {
            return GetComponent<NavMeshAgent>().nextPosition;

        }
        //otherwise return forward
        return transform.position + transform.forward;
    }

    public Animator Animator( )
    {
        return anim;

    }

    public void StopAnimator(Animator animator)
    {
        if (animator == null) { return; }

        animator.StopPlayback();

    }


    public void SetAnimationBool(Animator animator, string _animParameter, bool _animState)
    {
        if (animator == null ) { return; }

        animator.SetBool(_animParameter, _animState);

    }



    public void SetAnimationParameter(Animator animator, string _animParameter, int _animState)
    {
        if (animator == null) { return; }

        if (ContainsParam(animator, _animParameter))
        { animator.SetInteger(_animParameter, _animState); }

    }

    public void SetAnimationParameter(Animator animator, string _animParameter, float _animState)
    {
        if (animator == null ) { return; }

        if (ContainsParam(animator, _animParameter))
        { animator.SetFloat(_animParameter, _animState); }

    }

    public void SetAnimationParameter(Animator animator, string _animParameter, bool _animState)
    {
        if (animator == null) { return; }

        if (ContainsParam(animator, _animParameter))
        { animator.SetBool(_animParameter, _animState); }

    }



    public float GetAnimationFloat(Animator animator, string _animParameter)
    {
        if (animator == null) { return 0; }

        return animator.GetFloat(_animParameter);

    }


    public void PlayAnimation( string _animation, bool replay = false)
    {
        if (anim == null) { anim = GetComponent<Animator>(); }

        if (anim == null) { return; }

        anim.Play(_animation);
    }


    public void PlayAnimation(Animator animator, string _animation, bool replay = false)
    {
        if (animator == null) { return; }

        animator.Play(_animation);



    }




    public void EnableAnimator(Animator animator, bool _on)
    {
        if (animator == null) { animator = GetComponent<Animator>(); }

        if (animator == null) { return; }

        animator.enabled = _on;


    }

    public void SetAnimationTrigger(Animator animator, string _animation)
    {
        if (animator == null ) { return; }

        animator.SetTrigger(_animation);

 

    }

    public bool ContainsParam(Animator _anim, string _ParamName)
    {
        foreach (AnimatorControllerParameter param in _anim.parameters)
        {
            if (param.name == _ParamName) return true;
        }
        return false;
    }



}
