using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <Yarn functions are laid out in this maner inside the yarn script>
///  
///  <<yarntest Sally name>>
///  <<yarncommand Actor parameters>>
/// 


public class YarnFunctions : MonoBehaviour
{
    

    /// <summary>
    /// Transitions
    /// </summary>



    [YarnCommand("FadeToBlack")]
    public void FadeToBlack()
    {
        Debug.Log("Yarn FadeToBlack");

        GameManager.instance.cameraControls.anim.speed = 1;

        GameManager.instance.cameraControls.anim.Play("CameraFadeToBlack");
    }

    [YarnCommand("EndScene")]
    public void EndScene()
    {
        Debug.Log("Yarn EndScene");

        GameManager.instance.EndScene();

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }



    /// <summary>
    /// end Transitions
    /// </summary>


    /// <summary>
    /// start - Conversation types
    /// </summary>


    [YarnCommand("StaticConversation")]
    public void StaticConversation()
    {
        Debug.Log("Yarn StaticConversation");

        //set camera angle
        //lock player movement
        //have player face speaker

        GameManager.instance.cameraControls.anim.speed = 1;

        GameManager.instance.cameraControls.anim.Play("CameraFadeToBlack");
    }


    [YarnCommand("WalkAndTalk")]
    public void WalkAndTalk(string _location, string _villager, string _lineCount)
    {
        Debug.Log("Yarn WalkAndTalk");

        //set camera angle
        //lock player movement
        //have player face speaker
   
       
        GameManager.instance.WalkAndTalk(_location, _villager, _lineCount);
        // GameManager.instance.cameraControls.anim.speed = 1;

        //  GameManager.instance.cameraControls.anim.Play("CameraFadeToBlack");
    }

    /// <summary>
    /// end - Conversation types
    /// </summary>


    /// <summary>
    /// Movement
    /// </summary>



    [YarnCommand("MovePlayer")]
    public void MovePlayer(string _location)
    {
        Debug.Log("Yarn MovePlayer");

        GameManager.instance.MovePlayer(_location);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    [YarnCommand("MovePlayerAndSpeaker")]
    public void MovePlayerAndSpeaker(string _location)
    {
        //move the player and who they are speaking to
        Debug.Log("Yarn MovePlayerAndSpeaker");

        GameManager.instance.MovePlayerAndSpeaker(_location);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    
  [YarnCommand("HavePlayerFollow")]
    public void HavePlayerFollow(string _location, string _who)
    {
        //move the player and who they are speaking to
        Debug.Log("Yarn HavePlayerFollow: " + _who);

        GameManager.instance.HavePlayerFollow(_location, _who);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    [YarnCommand("LeadPlayer")]
    public void LeadPlayer(string _location, string _who)
    {
        //move the player and who they are speaking to
        Debug.Log("Yarn LeadPlayer: " + _who);

        GameManager.instance.LeadPlayer(_location, _who);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    [YarnCommand("LeadPlayerAndTalk")]
    public void LeadPlayer(string _location, string _who, string _lineCount)
    {
        //move the player and who they are speaking to
        Debug.Log("Yarn LeadPlayer: " + _who);

        GameManager.instance.LeadPlayer(_location, _who,  _lineCount);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    [YarnCommand("LeadPlayerAndTalkRunning")]
    public void LeadPlayer(string _location, string _who, string _lineCount,string _speed)
    {
        //move the player and who they are speaking to
        Debug.Log("Yarn LeadPlayer: " + _who);

        GameManager.instance.LeadPlayer(_location, _who, _lineCount, _speed);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    [YarnCommand("MoveScene")]
    public void MoveScene(string _location)
    {
        //move every character involved in the scene
        Debug.Log("Yarn MoveScene");


        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }


    /// <summary>
    /// end Movement
    /// </summary>
    /// 



    /// <summary>
    /// Animations
    /// </summary>


    [YarnCommand("function0")]
    public void YarnFunction(string spriteName)
    {
        Debug.Log("YarnFunctions");
        Debug.Log(spriteName);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    [YarnCommand("PlayAnimation")]
    public void PlayAnimation(string _animation)
    {
        Debug.Log("Yarn animations");
        Debug.Log(_animation);

        GameManager.instance.activeObject.GetComponent<Villager>().PlayAnimation(_animation);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    [YarnCommand("PlayAnimation")]
    public void PlayAnimation(string _animation, string _who)
    {
        Debug.Log("_who and Yarn animations");
        Debug.Log(_animation);

        Villager villager = GameManager.instance.FindVillager(_who);


        if (villager == null) { return; }

            villager.PlayAnimation(_animation);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }


    [YarnCommand("AnimateMouth")]
    public void AnimateMouth(string _pattern,string _length,  string _who)
    {
        Debug.Log("Yabrn AnimateMouth");

        Villager villager = GameManager.instance.FindVillager(_who);


        if (villager == null) { return; }

        villager.AnimateMouth(EnumGroups.ConvertMouthPattern(_pattern), float.Parse(_length));

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }


    /// <summary>
    /// end Animations
    /// </summary>








    [YarnCommand("Comment")]
    public void YarnComment(string[] _comment)
    {
        Debug.Log("comment: " + _comment);

    }

}

public class CustomWaitCommand : MonoBehaviour
{

    // Drag and drop your Dialogue Runner into this variable.
    public DialogueRunner dialogueRunner;

    public void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        // Create a new command called 'custom_wait', which waits for one second.
        dialogueRunner.AddCommandHandler(
            "custom_wait",
            CustomWait
        );
    }

    // The method that gets called when '<<custom_wait>>' is run.
    private void CustomWait(string[] parameters, System.Action onComplete)
    {

        // Start a coroutine that waits for one second:
        StartCoroutine(DoWait(onComplete));

        // Because this method takes a System.Action parameter, it's a blocking
        // command. Yarn Spinner will wait until onComplete is called.
    }

    private IEnumerator DoWait(System.Action onComplete)
    {

        // Wait for 1 second
        yield return new WaitForSeconds(1.0f);

        // Call the completion handler
        onComplete();

        // Yarn Spinner will now continue running!
    }
}