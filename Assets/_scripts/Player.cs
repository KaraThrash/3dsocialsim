using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public float walkSpeed, rotSpeed;
    public float acceleration, deceleration;
    public float turnAngle; //buffer for when the player will start moving before facing the exact direction of travel 

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
            Movement();
        }
        

        if (InputControls.InteractButton() )
        { Interact(); }
        if (InputControls.ActionButton())
        { PerformAction(); }

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

        Debug.Log("PerformAction");

    }




}
