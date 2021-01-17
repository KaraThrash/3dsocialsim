using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public float walkSpeed, rotSpeed;
    public float acceleration, deceleration;
    public float turnAngle; //buffer for when the player will start moving before facing the exact direction of travel 

    
    public List<Item> inventory;
    private int currentItem;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

  
    void Update()
    {
        if (gameManager.InConversation())
        {
            LookAtAction(gameManager.GetActiveObject());
            rb.velocity = Vector3.zero;
        }
        else 
        {
            if (gameManager.PlayerCanMove())
            { Movement(); }
        }
        

        if (InputControls.InteractButton() )
        { Interact(); }

        if (InputControls.ActionButton())
        { PerformAction(); }

        if (InputControls.NextButton())
        { NextItem(1); }
        if (InputControls.PreviousButton())
        { NextItem(-1); }

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


    //pick up items, talk to villagers, open doors, use enviromental object[mailbox, terminals, etc]
    public void Interact()
    {

        Debug.Log("Interact");


        
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + (Vector3.up * 0.5f),0.2f, transform.TransformDirection(Vector3.forward), out hit, 1.0f))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.GetComponent<Villager>() != null)
            {
                gameManager.InteractWithVillager(hit.transform.GetComponent<Villager>());
               
            }
        }

    }

    public void PerformAction()
    {
        if (inventory[currentItem].itemName == "shovel")
        {
            gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f),"dig");
        }
        else if (inventory[currentItem].itemName == "axe")
        {
            gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f),"chop");
        }
        else if (inventory[currentItem].itemName == "fishingrod")
        {
            gameManager.InteractWithGround(transform.position + (transform.forward * 0.6f), "fish", inventory[currentItem].subItem);
        }
        Debug.Log("PerformAction");

    }


    public void ChangeHeldItem()
    { 
    
    }

    public void NextItem(int _forward=1) //or -1 for previous item
    {
        if (inventory == null) { return; }
        inventory[currentItem].gameObject.SetActive(false);

        if (_forward == 0)
        {
            currentItem = 0;
            return;
        }

        currentItem += _forward;
        if (currentItem < 0) { currentItem = inventory.Count - 1; }
        if (currentItem >= inventory.Count) { currentItem = 0; }


        if (inventory[currentItem].usable == true)
        { inventory[currentItem].gameObject.SetActive(true); }
        else { NextItem( _forward ); }
       


    }


}
