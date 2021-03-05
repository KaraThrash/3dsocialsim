using System;
using System.Collections.Generic;
using UnityEngine;


public enum VillagerState { inScene,inPrison, offScreen,moving,idle,waiting,talking,activity }

public class Villager : MonoBehaviour
{
    public GameManager gameManager;

    public VillagerState currentState;
    public string npcName,mood;
    public float rotSpeed,speed,timeBetweenGreetings = 120.0f, turnDirection = -1; //time between greetings is the value of seconds that need to elapse before this npc will use a greeting instead of smalltalk
    public DateTime lastInteractionDate;
    private Dialogue activeDialogue ;
    private Rigidbody rb;
    private float movingTimer,idleToMoveRange = 10.0f; //number to use to randomize how long a villager should stand idle before pacing around

    public void State(VillagerState _state) { currentState = _state; }
    public VillagerState State() { return currentState; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) { Interact(); }
        //if (Input.GetKeyDown(KeyCode.N)) { activeDialogue = null; }


        Act();
        
    }


    public void Act()
    {
        if (State() == VillagerState.talking )
        {
            Talking();
        }
        else if (State() == VillagerState.moving)
        {
            Walking();
        }

    }


    public void Talking()
    {

        if ( activeDialogue != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(gameManager.GetPlayer().transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }

    public void Walking()
    {
       

        if (movingTimer >= 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(gameManager.GetPlayer().transform.position - transform.position);

            transform.Rotate(0, turnDirection * rotSpeed * Time.deltaTime,0);

            RaycastHit hit;
            if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), transform.forward, out hit, 1.0f))
            { }
            else 
            {
                transform.position = Vector3.MoveTowards(transform.position,transform.position + transform.forward, speed * Time.deltaTime);

            }



            movingTimer -= Time.deltaTime;
            if (movingTimer <= 0)
            {
                movingTimer = UnityEngine.Random.Range(-idleToMoveRange , 0);
            }

        }
        else if (movingTimer < 0)
        {
            

            movingTimer += Time.deltaTime;

            if (movingTimer > 0)
            {
                movingTimer = UnityEngine.Random.Range(0,idleToMoveRange);

                //to randomize rotating left or right
                if (UnityEngine.Random.Range(0,1.0f) > 0.5f) { turnDirection = -1; }
                else { turnDirection = 1; }
            }

        }

    }


    public void Interact()
    {
        if (activeDialogue == null)
        {
            if ((lastInteractionDate - DateTime.Now).TotalSeconds >= timeBetweenGreetings)
            {
                activeDialogue = FindDialogue("greeting");
            }
            else 
            {
                activeDialogue = FindDialogue("smalltalk");
            }

            State(VillagerState.talking);
        }

   


        //send the next line of dialogue to the gamemanager to display in the chat box
        gameManager.ShowDialogue(npcName,activeDialogue.NextDialogueLine());


        if (activeDialogue.EndOfDialogue())
        {  activeDialogue = null; }
    }


    



    

    public Dialogue FindDialogue(string _type="smalltalk")
    {
        List<Dialogue> dialogueList = DialogueLoader.GetDialogue(npcName);
        List<Dialogue> listToRandomize = new List<Dialogue>();

        for (int i = 0; i < dialogueList.Count; i++)
        {
            if (dialogueList[i].mood == mood && dialogueList[i].type == _type)
            {
                listToRandomize.Add(dialogueList[i]);
            }
        
        }

        return listToRandomize[(int)UnityEngine.Random.Range(0,listToRandomize.Count)];
    }

}
