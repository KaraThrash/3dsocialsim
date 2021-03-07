using System;
using System.Collections.Generic;
using UnityEngine;


public enum VillagerState { moving,idle,waiting,talking,activity }
public enum VillagerStoryState { inScene,inPrison, offScreen }
public enum Mood { happy,sad,neutral,scared,angry,confused,tired }

public class Villager : MonoBehaviour
{
    public GameManager gameManager;

    public VillagerState currentState;
    public VillagerStoryState currentStoryState;
    public string npcName,mood;
    public float rotSpeed,speed,timeBetweenGreetings = 120.0f ; //time between greetings is the value of seconds that need to elapse before this npc will use a greeting instead of smalltalk
    public Animator anim;

    private DateTime lastInteractionDate;

    private Dialogue activeDialogue ;
    private Rigidbody rb;
    private float movingTimer,idleToMoveRange = 10.0f, turnDirection = -1; //number to use to randomize how long a villager should stand idle before pacing around

    public void State(VillagerState _state) { OnStateChange(State(),_state); currentState = _state; }
    public VillagerState State() { return currentState; }

    public void StoryState(VillagerStoryState _state) { currentStoryState = _state; }
    public VillagerStoryState StoryState() { return currentStoryState; }


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


        if (StoryState() == VillagerStoryState.inScene)
        {
            InScene();
        }
        else 
        {
            if (State() == VillagerState.talking)
            {
                Talking();
            }
            else if (State() == VillagerState.moving)
            {
                Walking();
            }
            else if (State() == VillagerState.idle)
            {
                Idle();
            }
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
       


            Quaternion targetRotation = Quaternion.LookRotation(gameManager.GetPlayer().transform.position - transform.position);

            transform.Rotate(0, turnDirection * rotSpeed * Time.deltaTime,0);

            RaycastHit hit;
            if (Physics.SphereCast(transform.position + (Vector3.up * 0.5f),0.2f, transform.forward, out hit, 1.0f))
            { 
                //if something is in front dont move
            }
            else 
            {
                transform.position = Vector3.MoveTowards(transform.position,transform.position + transform.forward, speed * Time.deltaTime);

            }



            movingTimer -= Time.deltaTime;
            if (movingTimer <= 0)
            {
                State(VillagerState.idle);
                movingTimer = UnityEngine.Random.Range(1,idleToMoveRange );
            }

        
         

    }


    public void Idle()
    {
   


            movingTimer -= Time.deltaTime;

            if (movingTimer <= 0)
            {
                State(VillagerState.moving);
                movingTimer = UnityEngine.Random.Range(0.5f, idleToMoveRange);

                //to randomize rotating left or right
                if (UnityEngine.Random.Range(0, 1.0f) > 0.5f) { turnDirection = -1; }
                else { turnDirection = 1; }
            }

        
    }

    public void InScene()
    {
        if (GetComponent<SceneActions>() != null)
        {
            GetComponent<SceneActions>().TrailPlayer(transform);
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



    public void OnStateChange(VillagerState _oldState,VillagerState _state)
    {
        if (_state == VillagerState.moving)
        { SetAnimatorParameter("walking", true); }
        else if (_state == VillagerState.idle)
        { SetAnimatorParameter("walking", false); }
        else if (_state == VillagerState.talking)
        { SetAnimatorParameter("walking", false); }
   

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


    public void SetAnimatorParameter(string _parameter, bool _value)
    {
        if (anim != null)
        { anim.SetBool(_parameter,_value); }
    }

    public void SetAnimatorParameter(string _parameter, float _value)
    {
        if (anim != null)
        { anim.SetFloat(_parameter, _value); }
    }


}
