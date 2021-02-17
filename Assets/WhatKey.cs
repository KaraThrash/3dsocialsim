using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WhatKey : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            detectPressedKeyOrButton();
        }
        
           // Debug.Log("DpadHort Axis down: " + Input.GetAxis("DpadHort"));
    }

    public void detectPressedKeyOrButton()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                Debug.Log("KeyCode down: " + kcode);
        }
    }

}
