using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> green, yellow, red;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetClip(string _color)
    {
        if (_color.Equals("green") && green.Count > 0) { return green[0]; }
        if (_color.Equals("yellow") && yellow.Count > 0) { return yellow[0]; }
        if (_color.Equals("red") && red.Count > 0) { return red[0]; }

        return null;
    }

}
