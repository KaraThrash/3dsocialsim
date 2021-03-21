using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum VillagerState { moving,idle,waiting,talking,activity }
public enum VillagerStoryState { idle,inScene,inPrison, offScreen }
public enum Mood { happy,sad,neutral,scared,angry,confused,tired }

public class Villager : MonoBehaviour
{
    public GameManager gameManager;

    public VillagerState currentState;
    public VillagerStoryState currentStoryState;
    public string npcName,mood;
    public float rotSpeed,speed; //time between greetings is the value of seconds that need to elapse before this npc will use a greeting instead of smalltalk
    public Animator anim;
    public Transform head,animatedHead;
    public bool watchPlayer;

    public AudioClip voice, motif;

    private DateTime lastInteractionDate;

    private EmoteBubble emoteBubble ;
    private Dialogue activeDialogue ;
    private Rigidbody rb;

    private float movingTimer,idleToMoveRange = 10.0f, turnDirection = -1; //number to use to randomize how long a villager should stand idle before pacing around
    private float timeBetweenGreetings = 120.0f, maxHeadAngle = 45.0f; //time between greetings is the value of seconds that need to elapse before this npc will use a greeting instead of smalltalk

    private Vector3 startPos;

    public void State(VillagerState _state) { OnStateChange(State(),_state); currentState = _state; }
    public VillagerState State() { return currentState; }

    public void StoryState(VillagerStoryState _state) { currentStoryState = _state; }
    public VillagerStoryState StoryState() { return currentStoryState; }

    private NavMeshAgent nav;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();

        ResetToStart();



        if (head != null && animatedHead != null)
        {
            head.transform.rotation = animatedHead.transform.rotation;
        }
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) { Interact(); }
        //if (Input.GetKeyDown(KeyCode.N)) { activeDialogue = null; }


        Act();

        if (watchPlayer)
        { WatchPlayer(); }

    }

    public void ResetToStart()
    {
        if (startPos != Vector3.zero)
        {
            transform.position = startPos;
            SetNavMeshDestination(transform.position);

        }
        else 
        { 

            startPos = transform.position;
            SetNavMeshDestination(transform.position);

        }

    }


    public void WatchPlayer()
    {
        if (head == null || animatedHead == null) { return; }

        Vector3 targetYCorrected = new Vector3(gameManager.player.transform.position.x, head.position.y, gameManager.player.transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetYCorrected - head.position);

        Quaternion newrot  = Quaternion.Slerp(head.rotation, targetRotation, rotSpeed * Time.deltaTime);

        if (Quaternion.Angle(newrot, transform.rotation) < maxHeadAngle )
        {
           

            head.rotation = newrot;
        }

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

            if (Vector3.Distance(GetNavMeshDestination(),transform.position) < 0.1f)
            {
                State(VillagerState.idle);
                movingTimer = UnityEngine.Random.Range(1, idleToMoveRange);

            }
            else 
            {
              //  transform.position = Vector3.MoveTowards(transform.position,transform.position + transform.forward, speed * Time.deltaTime);

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

                SetNavMeshDestination(transform.position + (transform.forward + (transform.right * turnDirection)));
                SetNavMeshSpeed(speed);
                

            }

        
    }

    public void InScene()
    {
        if (GetComponent<SceneActions>() != null)
        {
            GetComponent<SceneActions>().TrailPlayer(this);
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


    public void Bonk()
    {
        //getting hit with the net

        //TODO: check scene, and state for cases where this is part of the scene or an idle bonking
        ThoughtBubble("angry");

    }


    public void ThoughtBubble(string _thought)
    {
        if (emoteBubble == null)
        { 
            GameObject clone = Instantiate(gameManager.emoteBubblePrefab,transform.position,transform.rotation);
            clone.transform.parent = transform;
            emoteBubble = clone.GetComponent<EmoteBubble>();
        }

        emoteBubble.gameObject.SetActive(true);

        if (_thought.Equals("happy")) { }
        else if (_thought.Equals("angry")) { emoteBubble.SetMaterial(EmotionImages.Angry()) ; }
        else if (_thought.Equals("tired")) { }

        


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



    public AudioClip Voice() { return voice; }
    public AudioClip Motif() { return motif; }
    public void Motif(AudioClip _clip) { motif = _clip; ; }
    public void Voice(AudioClip _clip) { voice = _clip; }



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




    //NavMeshFunctions
    public void SetNavMesh(bool _on)
    {
        if (nav != null)
        {
            nav.enabled = _on;

        }
    }

    public void SetNavMeshDestination(Vector3 dest)
    {
        if (nav != null && nav.destination != dest )
        {
            nav.destination = dest;

        }
    }

    public Vector3 GetNavMeshDestination()
    {
        if (nav != null)
        {
            return nav.destination;

        }
        return transform.position;
    }

    public void SetNavMeshSpeed(float _speed)
    {
        if (nav != null && nav.speed != _speed )
        {
            nav.speed = _speed;

        }
    }


    public void WarpNavMesh(Vector3 _warpTo)
    {
        if (nav != null )
        {
            nav.Warp(_warpTo);


        }
    }

    public void SetNavmeshVelocity(Vector3 _newvel)
    {
        if (nav != null )
        {
            nav.velocity = _newvel;


        }
    }

    public Vector3 GetNavMeshSteeringTarget()
    {
        if (nav != null )
        {
            return nav.steeringTarget;

        }
        //otherwise return forward
        return transform.position + transform.forward;
    }

    public Vector3 GetNavMeshNextPosition()
    {
        if (nav != null )
        {
            return nav.nextPosition;

        }
        //otherwise return forward
        return transform.position + transform.forward;
    }

}
