using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputControls 
{
    public static bool gamePad;
    //controller buttons
    public static string hortAxis = "Horizontal", vertAxis = "Vertical", interactButton = "A", pickupButton = "Y", actionButton = "B", nextButton = "RB", previousButton = "LB";
    public static string menuButton = "start";
    //controller axis as buttons
    public static string dPadVertButton = "DpadVert",dPadHortButton = "DpadHort";
    public static bool dPadVertPressed,dPadHortPressed;

    public static KeyCode interactKey = KeyCode.Space, pickupKey = KeyCode.LeftControl, actionKey = KeyCode.RightControl, nextKey = KeyCode.A, previousKey = KeyCode.A, dPadDownKey = KeyCode.DownArrow;
    public static KeyCode menuKey = KeyCode.Return;
    // Start is called before the first frame update
     static void Start()
    {
     
    }

    // Update is called once per frame
     static void Update()
    {
        if (dPadVertPressed == true && Input.GetAxis(dPadVertButton) == 0)
        { dPadVertPressed = false; }

        if (dPadHortPressed == true && Input.GetAxis(dPadHortButton) == 0)
        { dPadHortPressed = false; }


        if (gamePad == false)
        {
           // KeyboardControls();
        }
        else
        {
            //GamepadControls();
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

    public static bool MenuButton()
    {
        if (Input.GetButtonDown(menuButton) || Input.GetKeyDown(menuKey))
        { return true; }
        return false;
    }

    public static bool PickUpButton()
    {
        if (Input.GetButtonDown(pickupButton) || Input.GetKeyDown(pickupKey))
        { return true; }
        return false;
    }






    public static bool NextButton()
    {
        if (Input.GetButtonDown(nextButton) || Input.GetKeyDown(nextKey) || (dPadHortPressed == false && Input.GetAxis(dPadHortButton) == 1))
        { dPadHortPressed = true; return true; }
        if (Input.GetAxis(dPadHortButton) == 0)
        { dPadHortPressed = false; }
        return false;
    }

    public static bool PreviousButton()
    {
        if (Input.GetButtonDown(previousButton) || Input.GetKeyDown(previousKey) || (dPadHortPressed == false && Input.GetAxis(dPadHortButton) == -1))
        { dPadHortPressed = true;  return true; }
        return false;
    }

    public static bool DpadVert()
    {
        if (dPadVertPressed == false && Input.GetAxis(dPadVertButton) != 0)
        {
            dPadVertPressed = true;
            return true; 
        }
        return false;
    }

    public static bool DpadHort()
    {
        if (dPadHortPressed == false && (Input.GetAxis(dPadHortButton) != 0))
        {
           // dPadHortPressed = true;
            return true;
        }
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
