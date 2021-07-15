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
    /// System Commands
    /// </summary>


    [YarnCommand("LoadNewUnityScene")]
    public void LoadNewUnityScene(string _scene)
    {
        Debug.Log("Yarn LoadNewUnityScene");

        Application.LoadLevel(_scene);

    }


    [YarnCommand("EnableContinueButton")]
    public void EnableContinueButton()
    {
        Debug.Log("Yarn EnableContinueButton");

        GameManager.instance.SetContinueButton(true);
    }

    [YarnCommand("DisableContinueButton")]
    public void DisableContinueButton()
    {
        Debug.Log("Yarn DisableContinueButton");


        GameManager.instance.SetContinueButton(false);
    }



    /// <summary>
    /// end System Commands
    /// </summary>




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

    [YarnCommand("SetConversationTargetLocation")]
    public void SetConversationTargetLocation(string _location)
    {
        Debug.Log("Yarn SetConversationTargetLocation");

        GameManager.instance.SetConversationTargetLocation(_location);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }



    /// <summary>
    /// end Movement
    /// </summary>
    /// 


    /// <summary>
    /// Villager Controls
    /// </summary>
    /// 

    [YarnCommand("StartGroupPlacement")]
    public void StartGroupPlacement()
    {
        Debug.Log("Yarn StartGroupPlacement");

    }


    [YarnCommand("TeleportVillager")]
    public void TeleportVillager(string _villager,string _location)
    {
        Debug.Log("Yarn TeleportVillager");

        Villager villager = GameManager.instance.FindVillager(_villager);

        villager.Teleport(GameManager.instance.LocationManager().FindLocation(_location));

    }

    [YarnCommand("TeleportVillagerVector3")]
    public void TeleportVillagerVector3(string _villager, string _x,string _z)
    {
        Debug.Log("Yarn TeleportVillagerVector3");

        Villager villager = GameManager.instance.FindVillager(_villager);
        if (villager == null) { return; }
        Vector3 tempvec = new Vector3(Int32.Parse(_x), villager.transform.position.y, Int32.Parse(_z) );


        villager.Teleport(tempvec);

    }



    [YarnCommand("SetVillagerMood")]
    public void SetVillagerMood(string _villager, string _mood)
    {
        Debug.Log("Yarn SetVillagerMood");

        Villager villager = GameManager.instance.FindVillager(_villager);

        villager.CurrentMood(EnumGroups.MoodFromString(_mood));

    }

    [YarnCommand("SetVillagerState")]
    public void SetVillagerState(string _villager, string _state)
    {
        Debug.Log("Yarn SetVillagerState");

        Villager villager = GameManager.instance.FindVillager(_villager);

        villager.State(EnumGroups.VillagerStateFromString(_state));
    }

    [YarnCommand("SetVillagerStoryState")]
    public void SetVillagerStoryState(string _villager, string _state)
    {
        Debug.Log("Yarn SetVillagerStoryState");

        Villager villager = GameManager.instance.FindVillager(_villager);

        villager.StoryState(EnumGroups.VillagerStoryStateFromString(_state));

    }

    [YarnCommand("VillagerHoldAnimationUntilOnScreen")]
    public void VillagerHoldAnimationUntilOnScreen(string _villager, string _animation)
    {
        Debug.Log("Yarn VillagerHoldAnimationUntilOnScreen");

        Villager villager = GameManager.instance.FindVillager(_villager);

        villager.StoryState(VillagerStoryState.inScene);
        villager.ScriptedAction(SceneAction.holdingAnimation);
        villager.heldAnimation = _animation;

    }



    /// <summary>
    /// End Villager Controls
    /// </summary>













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

