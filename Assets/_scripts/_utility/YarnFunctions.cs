using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnFunctions : MonoBehaviour
{
   // public GameManager gameManager;
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

}
