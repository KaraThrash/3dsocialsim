using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Yarn.Unity;

public class Villager : MonoBehaviour
{
    public GameManager gameManager;

    public VillagerState currentState;
    public VillagerStoryState currentStoryState;
    public SceneAction scriptedAction;
    public Mood mood;
    public Villagers villagerName;
    public string npcName, scene, heldAnimation;
    public float rotSpeed, speed; //time between greetings is the value of seconds that need to elapse before this npc will use a greeting instead of smalltalk
    public Animator anim;
    public Transform head, animatedHead;
    public bool watchPlayer;

    public AudioClip voice, motif;

    private DateTime lastInteractionDate;

    private EmoteBubble emoteBubble;
    private Dialogue activeDialogue;
    private Rigidbody rb;

    private float movingTimer, idleToMoveRange = 10.0f, turnDirection = -1; //number to use to randomize how long a villager should stand idle before pacing around
    private float timeBetweenGreetings = 120.0f, maxHeadAngle = 75.0f; //time between greetings is the value of seconds that need to elapse before this npc will use a greeting instead of smalltalk

    public float blinkTimer;
    public SkinnedMeshRenderer leftEye, rightEye;
    public Material closedEye, openEye;

    public int phase = 0;

    private bool eyesOpen, toggleToResetRepeatedAction; //e.g. player animation when appearing on camera, and do that everything they appear on camera

    private Vector3 startPos;
    public MouthController mouthAnimator;

    public void State(VillagerState _state)
    { OnStateChange(State(), _state); currentState = _state; }

    public VillagerState State()
    { return currentState; }

    public void StoryState(VillagerStoryState _state)
    { OnStoryStateChange(StoryState(), _state); currentStoryState = _state; }

    public VillagerStoryState StoryState()
    { return currentStoryState; }

    public void ScriptedAction(SceneAction _action)
    { scriptedAction = _action; }

    public SceneAction ScriptedAction()
    { return scriptedAction; }

    public Rig rig;
    public YarnProgram scriptToLoad;
    private NavMeshAgent nav;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        mouthAnimator = GetComponent<MouthController>();
        ResetToStart();

        if (scriptToLoad != null)
        {
            DialogueRunner dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
            dialogueRunner.Add(scriptToLoad);
        }

        if (head != null && animatedHead != null)
        {
            head.transform.rotation = animatedHead.transform.rotation;
        }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) { Interact(); }
        //if (Input.GetKeyDown(KeyCode.N)) { activeDialogue = null; }

        if (StoryState() == VillagerStoryState.inScene)
        {
            StoryAct();
        }
        else
        {
            Act();
        }

        //watch player handles the toggle between using the rig to turn the head, and setting it back to use default animations
        WatchPlayer();

        if (leftEye != null)
        { BlinkTimer(); }

        SetAnimatorParameter("speed", GetNavmeshVelocity().magnitude);
    }

    public void ResetToStart()
    {
        if (startPos != Vector3.zero)
        {
            // transform.position
            // = startPos;
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
        if (head == null || animatedHead == null || GameManager.instance.player == null)
        {
            if (rig != null)
            {
                rig.weight = 0;
            }
            return;
        }

        if (watchPlayer)
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

        //TODO: fix this to slowly turn, and to go back to normal when not watching

        //Vector3 targetYCorrected = new Vector3(gameManager.player.transform.position.x, head.position.y , gameManager.player.transform.position.z);
        //Quaternion targetRotation = Quaternion.LookRotation(targetYCorrected - head.position);

        //Quaternion newrot  = Quaternion.Slerp(head.rotation, targetRotation, rotSpeed * Time.deltaTime);

        //if (Quaternion.Angle(targetRotation, transform.rotation) < maxHeadAngle + 1000)
        //{
        //    if (Quaternion.Angle(newrot, transform.rotation) <  maxHeadAngle + 1000)
        //    {
        //        head.rotation = newrot;
        //    }
        //}
        //else
        //{
        //    Quaternion.Slerp(head.rotation, animatedHead.rotation, rotSpeed * Time.deltaTime);

        //}
    }

    private SceneObject currentScene;

    public void InitScriptableScene(Transform _target, int _linesOfDialogue)
    {
        //currentScene = new SceneObject();
        currentScene.targetPos = _target.position;
        currentScene.startPos = transform.position;
        currentScene.linesOfDialogue = _linesOfDialogue;
        currentScene.distanceIncrement = Vector3.Distance(transform.position, _target.position) / _linesOfDialogue;
        // currentScene.distanceIncrement
        // = 5;
        ScriptedAction(SceneAction.walkAndTalk);
    }

    public void StoryAct()
    {
        if (ScriptedAction() == SceneAction.trailPlayer)
        {
            SceneActions.TrailPlayer(GetComponent<Villager>(), GameManager.instance.player.transform, 2);
        }
        else if (ScriptedAction() == SceneAction.fliers)
        { SceneActions.ReplaceNotice(this); }
        else if (ScriptedAction() == SceneAction.holdingAnimation)
        {
            transform.LookAt(GameManager.instance.player.transform.position);
            if (SceneActions.OnCamera(transform.position))
            {
                if (toggleToResetRepeatedAction == true)
                {
                    PlayAnimation(heldAnimation);
                    toggleToResetRepeatedAction = false;
                }
            }
            else
            {
                toggleToResetRepeatedAction = true;
            }
        }
        else if (ScriptedAction() == SceneAction.leadPlayer)
        {
            //if (phase == 0)
            //{
            //    SceneActions.HavePlayerFollow(this.transform, gameManager.LocationManager().FindLocation(MapLocation.southRoadTurn), gameManager.player.GetComponent<Player>(), 1);
            //    if (Vector3.Distance(transform.position, gameManager.LocationManager().FindLocation(MapLocation.southRoadTurn).position) < 1)
            //    {
            //        phase = 1;
            //        gameManager.dialogueRunner.GetComponent<DialogueUI>().MarkLineComplete();
            //    }

            //}
            //else if (phase == 1)
            //{
            //    SceneActions.HavePlayerFollow(this.transform, gameManager.LocationManager().FindLocation(MapLocation.playerHouse), gameManager.player.GetComponent<Player>(), 1);
            //    if (Vector3.Distance(transform.position, gameManager.LocationManager().FindLocation(MapLocation.playerHouse).position) < 1)
            //    { phase = 2; gameManager.dialogueRunner.GetComponent<DialogueUI>().MarkLineComplete(); }
            //}
            //else if (phase == 2)
            //{
            //    SceneActions.HavePlayerFollow(this.transform, gameManager.LocationManager().FindLocation(MapLocation.southRoadTurn), gameManager.player.GetComponent<Player>(), 1);
            //    if (Vector3.Distance(transform.position, gameManager.LocationManager().FindLocation(MapLocation.southRoadTurn).position) < 1)
            //    { phase = 0; gameManager.dialogueRunner.GetComponent<DialogueUI>().MarkLineComplete(); }
            //}
        }
        else if (ScriptedAction() == SceneAction.walkAndTalk)
        {
            //if (currentScene == null || currentScene.linesOfDialogue <= currentScene.phase) { return; InitScriptableScene(gameManager.LocationManager().FindLocation(MapLocation.southRoadTurn), 2); }

            //SceneActions.HavePlayerFollow(this.transform, currentScene.targetPos, gameManager.player.GetComponent<Player>(), 1);

            //if ( Vector3.Distance(transform.position, currentScene.startPos)  >  currentScene.distanceIncrement)
            //{
            //    currentScene.startPos = transform.position;
            //    currentScene.phase++;
            //    gameManager.dialogueRunner.GetComponent<DialogueUI>().MarkLineComplete();
            //}
        }
    }

    public void Act()
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
        else if (State() == VillagerState.waiting)
        {
        }
    }

    public void Talking()
    {
        SetNavMeshSpeed(0);

        Quaternion targetRotation = Quaternion.LookRotation(gameManager.GetPlayer().transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    public void Walking()
    {
        Quaternion targetRotation = Quaternion.LookRotation(gameManager.GetPlayer().transform.position - transform.position);

        if (Vector3.Distance(GetNavMeshDestination(), transform.position) < 0.1f)
        {
            State(VillagerState.idle);
            movingTimer = UnityEngine.Random.Range(1, idleToMoveRange);
        }
        else
        {
            // transform.position
            // =
            // Vector3.MoveTowards(transform.position,transform.position
            // +
            // transform.forward,
            // speed
            // * Time.deltaTime);
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

    public void Interact()
    {
        State(VillagerState.talking);

        if (scriptToLoad != null)
        {
            // scriptToLoad
            // = null;
        }
        else
        {
            ThoughtBubble(mood, 2);
        }
        //if (activeDialogue == null)
        //{
        //    if ((lastInteractionDate - DateTime.Now).TotalSeconds >= timeBetweenGreetings)
        //    {
        //        activeDialogue = FindDialogue("greeting");
        //    }
        //    else
        //    {
        //        activeDialogue = FindDialogue("smalltalk");
        //    }

        //    State(VillagerState.talking);
        //}

        ////send the next line of dialogue to the gamemanager to display in the chat box
        //gameManager.ShowDialogue(npcName,activeDialogue.NextDialogueLine());

        //if (activeDialogue.EndOfDialogue())
        //{  activeDialogue = null; }
    }

    public void OnStateChange(VillagerState _oldState, VillagerState _state)
    {
        if (_state == VillagerState.moving)
        { SetAnimatorParameter("walking", true); }
        else if (_state == VillagerState.idle)
        { SetAnimatorParameter("walking", false); }
        else if (_state == VillagerState.talking)
        { SetAnimatorParameter("walking", false); }
    }

    public void OnStoryStateChange(VillagerStoryState _oldState, VillagerStoryState _state)
    {
        toggleToResetRepeatedAction = false;

        if (_state == VillagerStoryState.inScene)
        { }
        else if (_state == VillagerStoryState.idle)
        { }
    }

    public void Bonk()
    {
        //getting hit with the net

        //TODO: check scene, and state for cases where this is part of the scene or an idle bonking
        ThoughtBubble(Mood.angry, 2);
        GameManager.instance.AudioManager().PlayWorldEffect(Voice());
    }

    public void ThoughtBubble(Mood _mood, float _duration = 2)
    {
        if (emoteBubble == null)
        {
            GameObject clone = Instantiate(gameManager.emoteBubblePrefab, transform.position, transform.rotation);
            clone.transform.parent = transform;
            emoteBubble = clone.GetComponent<EmoteBubble>();
        }

        emoteBubble.gameObject.SetActive(true);
        emoteBubble.SetMaterial(EmotionImages.GetEmotion(_mood), _duration);
    }

    public Dialogue FindDialogue(string _type = "smalltalk")
    {
        List<Dialogue> dialogueList = DialogueLoader.GetDialogue(npcName);
        List<Dialogue> listToRandomize = new List<Dialogue>();

        //for (int i = 0; i < dialogueList.Count; i++)
        //{
        //    if (dialogueList[i].mood == mood && dialogueList[i].type == _type)
        //    {
        //        listToRandomize.Add(dialogueList[i]);
        //    }

        //}

        return listToRandomize[(int)UnityEngine.Random.Range(0, listToRandomize.Count)];
    }

    public Mood CurrentMood()
    { return mood; }

    public void CurrentMood(Mood _mood)
    { mood = _mood; }

    public void SetMouth(Mood _mood)
    { mouthAnimator.SetMouth(_mood); }

    public AudioClip Voice()
    { return voice; }

    public AudioClip Motif()
    { return motif; }

    public void Motif(AudioClip _clip)
    { motif = _clip; ; }

    public void Voice(AudioClip _clip)
    { voice = _clip; }

    public Villagers VillagerName()
    { return villagerName; }

    public void Teleport(Transform _location)
    {
        transform.position = _location.position;
        // WarpNavMesh(_location.position);
    }

    public void Teleport(Vector3 _location)
    {
        transform.position = _location;
        WarpNavMesh(_location);
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

    public bool ContainsParam(Animator _Anim, string _ParamName)
    {
        foreach (AnimatorControllerParameter param in _Anim.parameters)
        {
            if (param.name == _ParamName) return true;
        }
        return false;
    }

    public void SetAnimatorParameter(string _parameter, bool _value)
    {
        if (anim == null || !ContainsParam(anim, _parameter))
        { return; }

        anim.SetBool(_parameter, _value);
    }

    public void SetAnimatorParameter(string _parameter, float _value)
    {
        if (anim == null || !ContainsParam(anim, _parameter))
        { return; }

        anim.SetFloat(_parameter, _value);
    }

    public void PlayAnimation(string _parameter)
    {
        if (anim != null)
        { anim.Play(_parameter.ToLower()); }
    }

    //NavMeshFunctions
    public void SetNavMesh(bool _on)
    {
        if (nav != null)
        {
            nav.enabled = _on;
        }
    }

    public bool SetNavMeshDestination(Vector3 _dest)
    {
        //check that the destination is on the navmesh
        if (nav != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(_dest, out hit, 1f, NavMesh.AllAreas))
            {
                nav.SetDestination(hit.position);
                return true;
            }
        }

        return false;
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
        if (nav != null && nav.speed != _speed)
        {
            nav.speed = _speed;
        }
    }

    public float GetNavMeshSpeed()
    {
        if (nav != null)
        {
            return nav.speed;
        }
        return 0;
    }

    public void WarpNavMesh(Vector3 _warpTo)
    {
        if (nav != null)
        {
            nav.Warp(_warpTo);
        }
    }

    public void SetNavmeshVelocity(Vector3 _newvel)
    {
        if (nav != null)
        {
            nav.velocity = _newvel;
        }
    }

    public Vector3 GetNavmeshVelocity()
    {
        if (nav != null)
        {
            return nav.velocity;
        }
        return Vector3.zero;
    }

    public Vector3 GetNavMeshSteeringTarget()
    {
        if (nav != null)
        {
            return nav.steeringTarget;
        }
        //otherwise return forward
        return transform.position + transform.forward;
    }

    public Vector3 GetNavMeshNextPosition()
    {
        if (nav != null)
        {
            return nav.nextPosition;
        }
        //otherwise return forward
        return transform.position + transform.forward;
    }
}