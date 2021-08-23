using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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

     //   Application.LoadLevel(_scene);
        SceneManager.LoadScene(_scene);

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

    [YarnCommand("AdvanceSceneAnimator")]
    public void AdvanceSceneAnimator()
    {
        Debug.Log("Yarn AdvanceSceneAnimator");

        //if there is an active scene and it has an animator, set its trigger to advance
        GameManager.instance.AdvanceSceneAnimator();
    }



    [YarnCommand("PlaySoundEffect")]
    public void PlaySoundEffect(string _clip)
    {
        Debug.Log("Yarn PlaySoundEffect");

        //if the clip is in resources, load and play it
        GameManager.instance.PlaySoundEffect(_clip);
    }



    /// <summary>
    /// end System Commands
    /// </summary>




    /// <summary>
    /// Transitions
    /// </summary>



    [YarnCommand("FadeToBlack")]
    public void FadeToBlack(string _speed)
    {
        Debug.Log("Yarn FadeToBlack");

        GameManager.instance.cameraControls.anim.speed = float.Parse(_speed);

        GameManager.instance.cameraControls.anim.Play("CameraClearToBlackAndStay");
    }

    [YarnCommand("FadeFromBlack")]
    public void FadeFromBlack(string _speed)
    {
        Debug.Log("Yarn FadeFromBlack");

        GameManager.instance.cameraControls.anim.speed = float.Parse(_speed);

        GameManager.instance.cameraControls.anim.Play("CameraBlackToClear");
    }

    [YarnCommand("BlackOut")]
    public void BlackOut(string _speed)
    {
        Debug.Log("Yarn BlackOut");

        GameManager.instance.cameraControls.anim.speed = float.Parse(_speed);

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






    [YarnCommand("SceneSpecificAction")]
    public void SceneSpecificAction()
    {
        Debug.Log("Yarn SceneSpecificAction");

        //set camera angle
        //lock player movement
        //have player face speaker

        GameManager.instance.SceneSpecificAction();

    }




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


        if (villager == null) { return; }

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

    [YarnCommand("VillagerThought")]
    public void VillagerThought(string _villager, string _thought)
    {
        Debug.Log("Yarn SetVillagerMood");

        Villager villager = GameManager.instance.FindVillager(_villager);

        if (villager == null) { return; }

        villager.ThoughtBubble(EnumGroups.MoodFromString(_thought.ToLower()),2);

    }

    [YarnCommand("SetVillagerMood")]
    public void SetVillagerMood(string _villager, string _mood)
    {
        Debug.Log("Yarn SetVillagerMood");

        Villager villager = GameManager.instance.FindVillager(_villager);

        if (villager == null) { return; }

        villager.CurrentMood(EnumGroups.MoodFromString(_mood.ToLower()));

    }

    [YarnCommand("SetVillagerState")]
    public void SetVillagerState(string _villager, string _state)
    {
        Debug.Log("Yarn SetVillagerState");

        Villager villager = GameManager.instance.FindVillager(_villager);

        if (villager == null) { return; }

        villager.State(EnumGroups.VillagerStateFromString(_state));
    }

    [YarnCommand("SetVillagerStoryState")]
    public void SetVillagerStoryState(string _villager, string _state)
    {
        Debug.Log("Yarn SetVillagerStoryState");

        Villager villager = GameManager.instance.FindVillager(_villager);

        if (villager == null) { return; }

        villager.StoryState(EnumGroups.VillagerStoryStateFromString(_state));

    }

    [YarnCommand("VillagerHoldAnimationUntilOnScreen")]
    public void VillagerHoldAnimationUntilOnScreen(string _villager, string _animation)
    {
        Debug.Log("Yarn VillagerHoldAnimationUntilOnScreen");

        Villager villager = GameManager.instance.FindVillager(_villager);

        if (villager == null) { return; }

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

    //[YarnCommand("PlayAnimation")]
    //public void PlayAnimation(string _animation)
    //{
    //    Debug.Log("Yarn animations");
    //    Debug.Log(_animation);

    //    GameManager.instance.activeObject.GetComponent<Villager>().PlayAnimation(_animation.ToLower());

    //    //<<yarntest Sally name>>
    //    //<<yarncommand Actor parameters>>
    //}

    [YarnCommand("PlayAnimation")]
    public void PlayAnimation(string _who,string _animation)
    {
        Debug.Log("_who and Yarn animations");
        Debug.Log(_who + " " + _animation);

        Villager villager = GameManager.instance.FindVillager(_who);


        if (villager == null) { Debug.Log("didnt find");  return; }

            villager.PlayAnimation(_animation);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }

    [YarnCommand("ReleaseHeadRig")]
    public void ReleaseHeadRig(string _who)
    {
        Debug.Log("ReleaseHeadRig");

        Villager villager = GameManager.instance.FindVillager(_who);


        if (villager == null) { Debug.Log("didnt find"); return; }

        if (villager.rig != null)
        {
            villager.rig.weight = 0;
        }

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }



    //sit and stand toggle the 'sitting' boolean for the animation state to work with the mask for their legs
    [YarnCommand("Sit")]
    public void Sit(string _who)
    {
        Debug.Log("sit: " + _who);

        Villager villager = GameManager.instance.FindVillager(_who);


        if (villager == null) { return; }

        villager.SetAnimatorParameter("sitting",true);

    }

    [YarnCommand("Stand")]
    public void Stand(string _who)
    {
        Debug.Log("Stand: " + _who);

        Villager villager = GameManager.instance.FindVillager(_who);


        if (villager == null) { return; }

        villager.SetAnimatorParameter("sitting", false);

    }


    [YarnCommand("AnimateMouth")]
    public void AnimateMouth(string _who, string _pattern,string _length)
    {
        Debug.Log("Yabrn AnimateMouth");

        Villager villager = GameManager.instance.FindVillager(_who);


        if (villager == null) { return; }

        villager.AnimateMouth(EnumGroups.MouthPatternFromString(_pattern), float.Parse(_length));

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

