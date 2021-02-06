using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { playerControlled,inMenu,talking, choosing,fishing,acting,showing}

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerState state { get; set; }
    public string playerStateString;
    public float walkSpeed, rotSpeed;
    public float acceleration, deceleration;
    public float turnAngle; //buffer for when the player will start moving before facing the exact direction of travel 
    public Transform InHands;
    public GameObject heldItem;
    public Inventory inventory;
    private int currentItem;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

  
    void Update()
    {

        //for debug porpoises
        playerStateString = state.ToString();
        
        PlayerStates();

        if (InputControls.InteractButton() )
        { Interact(); }

        if (InputControls.PickUpButton())
        { 
            PickUp(); 
        }

        if (InputControls.NextButton())
        { NextItem(1); }
        if (InputControls.PreviousButton())
        { NextItem(-1); }


        if (InputControls.MenuButton())
        { gameManager.ToggleMenu("inventory"); }

    }

    public void SetState(PlayerState _state)
    {
        state = _state;
    }

    public void PlayerStates()
    {
        if (state == PlayerState.playerControlled)
        {
            PlayerControlled();
        }
        else if (state == PlayerState.talking)
        {
            Talking();
        }
        else if (state == PlayerState.fishing)
        {
            LookAtAction(gameManager.GetActiveObject());
            SetVelocities(Vector3.zero, Vector3.zero);
        }
        else if (state == PlayerState.inMenu)
        {

            SetVelocities(Vector3.zero, Vector3.zero);
            InMenuControls();

        }
        else { SetVelocities(Vector3.zero, Vector3.zero); }

    }

    public void PlayerControlled()
    {
        Movement();

  
    }

    public void Talking()
    {
        LookAtAction(gameManager.GetActiveObject());
        SetVelocities(Vector3.zero, Vector3.zero);
    }


    public void InMenuControls()
    {
        if (InputControls.HortAsButton()) { gameManager.UiManager().MoveCursor((int)Mathf.Sign(InputControls.HorizontalAxis())); }
        else if (InputControls.VertAsButton()) { gameManager.UiManager().MoveCursor(-10 * (int)Mathf.Sign(InputControls.VerticalAxis())); }
        else if (InputControls.DpadVertAsButton()) { gameManager.UiManager().MoveCursor(-10 * (int)Mathf.Sign(InputControls.DpadVert())); }
        else if (InputControls.DpadHortAsButton()) { gameManager.UiManager().MoveCursor( (int)Mathf.Sign(InputControls.DpadHort())); }
        else { InputControls.TrackAxisButtons(); }
    }



    //conversation target, item pickup, or directly at the camera
    public void LookAtAction(Transform _lookat)
    {
        Quaternion targetRotation = Quaternion.LookRotation(_lookat.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    public void Movement()
    {

        if (InputControls.HorizontalAxis() != 0 || InputControls.VerticalAxis() != 0)
        {
            //get the intended direction then rotate before moving
            Vector3 moveDirection = Vector3.right * InputControls.HorizontalAxis();
            moveDirection = moveDirection + (Vector3.forward * InputControls.VerticalAxis());
            
            //rebalance the speed for the input, avoid the goldeneye diagonal speed multiplier while also remaining still with no input
            if (moveDirection.magnitude > 1)
            { moveDirection = (moveDirection).normalized; }


            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
            float angle = Vector3.Angle((transform.position + moveDirection) - transform.position, transform.forward);

            //larger turnAngle will have a rounder run arc instead of angular turns
            if (angle < turnAngle)
            {

                rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * walkSpeed, Time.deltaTime * acceleration);

            }


        }
        else { 
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * deceleration);
            rb.angularVelocity = Vector3.zero;
        }
        

    }

    public void PickUp( )
    {
        Vector3 squarePos = new Vector3(Mathf.RoundToInt((transform.position + (transform.forward * 0.3f)).x), Mathf.FloorToInt((transform.position + (transform.forward * 0.3f)).y), Mathf.RoundToInt((transform.position + (transform.forward * 0.3f)).z));

        if (gameManager.TerrainManager().GetMapSquare(transform.position + (transform.forward * 0.3f)) == null) {
            Debug.Log("No square saved at : " + squarePos); 
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
        else { Debug.Log("No item saved at : " + squarePos); }



    }


    //Interact with characters and objects, Use tool or item, TODO:Move furniture(Press and hold A)
    public void Interact()
    {

        Debug.Log("Interact");


        //try to talk first, then try to use an item the player is in front of, otherwise if they have a tool in their hand, use it.

        RaycastHit hit;
        if (Physics.SphereCast(transform.position + (Vector3.up * 0.5f), 0.2f, transform.TransformDirection(Vector3.forward), out hit, 0.3f))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<Villager>() != null)
            {
                gameManager.InteractWithVillager(hit.transform.GetComponent<Villager>());

            }
            else if (hit.transform.GetComponent<Item>() != null && heldItem != null && hit.transform.GetComponent<Item>().toolUsable.Equals(heldItem.GetComponent<Item>().itemName))
            {
                //dig hole with shovel, chop tree with axe
                UseTool();
            }
            else if (hit.transform.GetComponent<Item>() != null && hit.transform.GetComponent<Item>().usable == true)
            {
                //iteract with item[door mail box faucet etc]

                //
                hit.transform.GetComponent<Item>().Interact(gameManager);
            }
            else { UseTool(); }
        }
        else 
        {
            //if no one to talk to check hands for tools
            UseTool();
        }

    }

    public void UseTool()
    {
        //TODO: more precise calculation of which square to interact in
        // standing in the middle of a square should target the next square, standing on the edge facing inward should target that square


        if (heldItem == null || heldItem.GetComponent<Item>() == null) { return; }

        // Item heldItem = inventory.GetFromPockets(currentItem);
        if (heldItem != null && heldItem.GetComponent<Item>().itemName.Equals("shovel") )
        {
            gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f),"dig");
        }
        else if (heldItem != null && heldItem.GetComponent<Item>().itemName.Equals("axe"))
        {
            gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f), "chop");
        }
        else if (heldItem != null && heldItem.GetComponent<Item>().itemName.Equals("fishingRod"))
        {
            gameManager.InteractWithGround(transform.position + (transform.forward * 1.6f), "fish", heldItem.GetComponent<Item>().subItem);
        }
        else if (heldItem != null && heldItem.GetComponent<Item>().itemName.Equals("net"))
        {
            gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f), "net");
        }
        Debug.Log("PerformAction");

    }


    public void HoldToCamera(GameObject _obj)
    {
        _obj.transform.position = InHands.position;
        _obj.transform.rotation = InHands.rotation;
    }

    public void HoldToCamera(Transform _obj)
    {
        _obj.position = InHands.position;
        _obj.rotation = InHands.rotation;
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
        if (currentItem < 0) { currentItem = inventory.PocketItemsCount() ; Destroy(heldItem); return; }
        if (currentItem >= inventory.PocketItemsCount()) { currentItem = -1; Destroy(heldItem); return; }

       // Debug.Log("changing item: " );
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

            //TODO: keep the loaded items/pocket items in a way that is more efficent
            if (heldItem != null) { Destroy(heldItem); }
            
            heldItem = newheldItem;

        }
        else 
        {
           //only toggle through usable items
           NextItem(_forward); 
                
        }
       


    }


    public void SetVelocities(Vector3 vel, Vector3 angularVel)
    {
        rb.velocity = vel;
        rb.angularVelocity = angularVel;
    }

}
