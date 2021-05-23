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
    // public GameManager gameManager;



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





    /// <summary>
    /// end Transitions
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
