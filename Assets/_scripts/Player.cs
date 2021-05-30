using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;


public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerState state;
    public WorldLocation worldLocation;
    public string playerStateString;
    public float walkSpeed, rotSpeed;
    public float acceleration, deceleration;
    public float turnAngle; //buffer for when the player will start moving before facing the exact direction of travel 
    public bool inside;
    public Transform InHands;
    public GameObject heldItem;
    public Inventory inventory;
    public WorldUi placeholderActionText;

    private int currentItem;
    private Vector3 moveDirection;
    private Rigidbody rb;

    public PlayerState State (){ return state; }

    public void State(PlayerState _state) { OnStateChange(_state); state = _state; }

    public void OnStateChange(PlayerState _state)
    {
        //new state is same as the old state
        if (State() == _state) { return; }

    }


    public WorldLocation WorldLocation(){ return worldLocation; }



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
        SetText("game start");
        rb = GetComponent<Rigidbody>();
    }

  
    void Update()
    {
        

        //TODO: find a better way to do this
        if (gameManager.DialogueIsRunning() == true)
        {
            return;
        }
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
            LookAtAction(gameManager.GetActiveObject(),rotSpeed);
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
        LookAtAction(gameManager.GetActiveObject(),rotSpeed);
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



    public void Movement()
    {

        if (InputControls.HorizontalAxis() != 0 || InputControls.VerticalAxis() != 0)
        {
            //get the intended direction then rotate before moving
            moveDirection = Vector3.right * InputControls.HorizontalAxis();
            moveDirection = moveDirection + (Vector3.forward * InputControls.VerticalAxis());

            //rebalance the speed for the input, avoid the goldeneye diagonal speed multiplier while also remaining still with no input
            if (moveDirection.magnitude > 1)
            { moveDirection = (moveDirection).normalized; }


            MoveTo(moveDirection);


        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * deceleration);
            rb.angularVelocity = Vector3.zero;
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















    //conversation target, item pickup, or directly at the camera
    public void LookAtAction(Transform _lookat,float _rotSpeed=1)
    {
        Quaternion targetRotation = Quaternion.LookRotation(_lookat.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotSpeed * Time.deltaTime);
    }


  


    

    public void PickUp( )
    {
        Vector3 squarePos = new Vector3(Mathf.RoundToInt((transform.position + (transform.forward * 0.3f)).x), Mathf.FloorToInt(transform.position.y), Mathf.RoundToInt((transform.position + (transform.forward * 0.3f)).z));

        if (gameManager.TerrainManager().GetMapSquare(squarePos) == null) {
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

        //check forward first, then check down

       // Vector3 fwd = (transform.position + (Vector3.up * 0.5f)) - (transform.position + (transform.forward * 0.5f));

        RaycastHit hit;
       // if (Physics.SphereCast(transform.position + (Vector3.up * 0.5f), 0.2f, transform.TransformDirection(Vector3.forward), out hit, 0.3f))
        if (Physics.SphereCast(transform.position + (Vector3.up * 0.65f), 0.15f, transform.TransformDirection((Vector3.down + Vector3.forward).normalized), out hit, 0.9f))
        {
           
            if (hit.transform.GetComponent<Villager>() != null)
            {
                InteractWithVillager(hit.transform.GetComponent<Villager>());

            }
            else if (hit.transform.GetComponent<Item>() != null )
            {
                InteractWithItem(hit.transform.GetComponent<Item>());
            }
            else if (hit.transform.GetComponent<Tree>() != null)
            {
                InteractWithItem(hit.transform.GetComponent<Tree>());
            }


            else
            {
                //if no one to talk to and not standing in front of a tree/hole/board etc check hands for tools
                UseTool(hit);
            }
            
        }
        else 
        {
            //if no one to talk to check hands for tools
       //     UseTool();


        }

    }

    public void InteractWithVillager(Villager _villager)
    {

        if (heldItem != null && heldItem.GetComponent<Item>().itemName.Equals("net"))
        {
            gameManager.BonkVillager(_villager);
        }
      //  else if()
      //  {
      //TODO
           //story relevant interation, framing, etc

      //  }
        else
        {
            gameManager.InteractWithVillager(_villager);


        }

    }
   

    public void InteractWithItem(Item _item)
    {

        if (heldItem != null)
        {

            if (heldItem.GetComponent<Item>().itemName.Equals("axe") && _item.GetComponent<Tree>() != null && !_item.GetComponent<Tree>().JustStump())
            {
                Debug.Log("chop");
                _item.GetComponent<Tree>().Chop();
            }
            else if (heldItem.GetComponent<Item>().itemName.Equals("shovel") )
            {
                _item.Dig();

                if (_item.GetComponent<Hole>() != null)
                {
                    if (_item.GetComponent<Hole>().open)
                    {
                        _item.GetComponent<Hole>().Bury(null);
                    }
                    else if (_item.GetComponent<Hole>().GetItem() != null)
                    {
                        _item.GetComponent<Hole>().Bury(null);
                    }
                }
                else if (_item.GetComponent<Tree>() != null && _item.GetComponent<Tree>().JustStump())
                { 
                
                }



            }
            else if (heldItem.GetComponent<Item>().itemName.Equals("net"))
            {
                _item.Catch();




            }

            //if (heldItem.GetComponent<Item>().usable && _item.toolUsable.Equals(heldItem.GetComponent<Item>().itemName))
            //{
            //    //dig hole with shovel, chop tree with axe
            //    UseTool(_item);
            //}
            //todo: bury held item
        }
        else if (_item.usable == true)
        {
            //if not holding an item try to interact

            if (transform.position.z < _item.transform.position.z && _item.CheckForNotice())
            { _item.TakedownNotice(); }
            else 
            {
                _item.Interact(gameManager);
            }

            //iteract with item[door mail box faucet etc]
            

        }

    }


    public void UseTool(Item _item)
    {
        Vector3 squarePos = PositionRounded(transform.forward * 0.2f);

        //TODO: more precise calculation of which square to interact in
        // standing in the middle of a square should target the next square, standing on the edge facing inward should target that square

        if (heldItem == null || heldItem.GetComponent<Item>() == null) { return; }
        // Item heldItem = inventory.GetFromPockets(currentItem);
        if (heldItem != null)
        {

            if (heldItem.GetComponent<Item>().itemName.Equals("shovel"))
            {
                gameManager.InteractWithGround(squarePos, "dig");
            }
            else if (_item.GetComponent<Tree>() != null && heldItem.GetComponent<Item>().itemName.Equals("axe"))
            {

                _item.GetComponent<Tree>().Chop();
            }
            else if (heldItem.GetComponent<Item>().itemName.Equals("fishingRod"))
            {
                gameManager.InteractWithGround(transform.position + (transform.forward * 1.6f), "fish", heldItem.GetComponent<Item>().subItem);
            }
            else if (heldItem.GetComponent<Item>().itemName.Equals("net"))
            {
                gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f), "net");
            }
            // to be able to bury an item
            else if (heldItem.GetComponent<Item>().buryable)
            {
                gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f), "net");
            }





        }

        Debug.Log("PerformAction");

    }




    public void UseTool(RaycastHit _hit)
    {
        Vector3 squarePos = PositionRounded(transform.forward * 0.2f);

        //TODO: more precise calculation of which square to interact in
        // standing in the middle of a square should target the next square, standing on the edge facing inward should target that square

        if (heldItem == null || heldItem.GetComponent<Item>() == null) { return; }
        // Item heldItem = inventory.GetFromPockets(currentItem);
        if (heldItem != null  )
        {

            if (heldItem.GetComponent<Item>().itemName.Equals("shovel"))
            {
                gameManager.TerrainManager().Dig(this, _hit);

               // Dig();
                //gameManager.InteractWithGround(squarePos, "dig");
            }
            else if ( heldItem.GetComponent<Item>().itemName.Equals("axe"))
            {

                gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f), "chop");
            }
            else if (heldItem.GetComponent<Item>().itemName.Equals("fishingRod"))
            {
                gameManager.InteractWithGround(transform.position + (transform.forward * 1.6f), "fish", heldItem.GetComponent<Item>().subItem);
            }
            else if (heldItem.GetComponent<Item>().itemName.Equals("net"))
            {
                gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f), "net");
            }
        }

        Debug.Log("PerformAction");

    }



    public void Dig()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position + (Vector3.up * 0.65f), 0.1f, transform.TransformDirection(Vector3.forward), out hit, 0.3f)) 
        {
            if (hit.transform.tag == "grass")
            { 
            
            }
            else if (hit.transform.GetComponent<Hole>() != null )
            {
                //stumps
                //grubs
                //fossiles and rocks?

            }
            else if (hit.transform.GetComponent<Tree>() != null && hit.transform.GetComponent<Tree>().JustStump())
            {
                //stumps
               

            }

        }


    }












    public void HoldToCamera(GameObject _obj)
    {
        transform.LookAt(new Vector3(transform.position.x,transform.position.y,transform.position.z -10));
        _obj.transform.position = InHands.position;
        _obj.transform.rotation = InHands.rotation;
    }

    public void HoldToCamera(Transform _obj)
    {
        transform.LookAt(new Vector3(transform.position.x, transform.position.y, transform.position.z - 10));
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


    public bool IsInside() { return inside; }
    public Vector3 MoveDirection() { return moveDirection; }


}
