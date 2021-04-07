using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnTest : MonoBehaviour
{
    [YarnCommand("yarntest")]
    public void YarnFunction(string spriteName)
    {
        Debug.Log("Testing Yarn Function");
        Debug.Log(spriteName);

        //<<yarntest Sally name>>
        //<<yarncommand Actor parameters>>
    }
}
