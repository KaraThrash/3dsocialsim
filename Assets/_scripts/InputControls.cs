using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputControls 
{
    public static bool gamePad;

    public static string hortAxis = "Horizontal", vertAxis = "Vertical", interactButton = "B", actionButton = "A";
    public static KeyCode interactKey = KeyCode.Space, actionKey = KeyCode.LeftControl;
    // Start is called before the first frame update
     static void Start()
    {
     
    }

    // Update is called once per frame
     static void Update()
    {


        if (gamePad == false)
        {
            KeyboardControls();
        }
        else
        {
            GamepadControls();
        }
    }

    public static bool InteractButton()
    {
        if (Input.GetButtonDown(interactButton) || Input.GetKeyDown(interactKey))
        { return true; }
        return false;
    }

    public static bool ActionButton()
    {
        if (Input.GetButtonDown(actionButton) || Input.GetKeyDown(actionKey))
        { return true; }
        return false;
    }

    public static float HorizontalAxis()
    { return Input.GetAxis(hortAxis); }

    public static float VerticalAxis()
    { return Input.GetAxis(vertAxis); }

    public static void KeyboardControls()
    {
        if (Input.GetAxis(hortAxis) != 0)
        { 
            
        }
    }

    public static void GamepadControls()
    { 
    
    }

}
