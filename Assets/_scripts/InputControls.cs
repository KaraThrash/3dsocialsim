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
    public static bool dPadVertPressed,dPadHortPressed,vertPressed,hortPressed;

    public static KeyCode interactKey = KeyCode.Space, pickupKey = KeyCode.LeftControl, actionKey = KeyCode.RightControl, nextKey = KeyCode.A, previousKey = KeyCode.A, dPadDownKey = KeyCode.DownArrow;
    public static KeyCode menuKey = KeyCode.Return;

    private static float deadtime = 0.2f, deadClock; //holding an axis while using as a button



    // https://game8.co/games/Animal-Crossing-New-Horizons/archives/284569#hl_3

    //    Control Action
    //A Interact with characters and objects
    //Use tool or item
    //Move furniture(Press and hold A)
    //B Cancel action
    //X   Open up pockets(Inventory)
    //Y Pick up item
    //Left Control Stick Move(Walk/Run)
    //Right Control Stick Pan Camera(Outdoors)
    //Rotate Camera(Indoors)
    //ZL Use NookPhone
    //R   Chat with other players on the island
    //ZR  Use Reactions
    //↑	Open up the Tool Ring(Unlockable)
    //← or →	Switch between tools
    //↓	Store tool
    //-	Save and quit game
    //+	Switch between permits(When using the Island Designer app)
    //Capture Take a screenshot
    //Capture the last 30-second gameplay(Press and hold the Capture button)






    static void Start()
    {
     
    }

    // Update is called once per frame
     static void Update()
    {

        TrackAxisButtons();


        if (gamePad == false)
        {
           // KeyboardControls();
        }
        else
        {
            //GamepadControls();
        }
    }

    public static void TrackAxisButtons()
    {
        //for using the axises as a button, can only press again once they reset to 0
        if (dPadVertPressed == true && Input.GetAxis(dPadVertButton) == 0)
        { dPadVertPressed = false; }

        if (dPadHortPressed == true && Input.GetAxis(dPadHortButton) == 0)
        { dPadHortPressed = false; }

        if (hortPressed == true && Input.GetAxis(hortAxis) == 0)
        { hortPressed = false; }

        if (vertPressed == true && Input.GetAxis(vertAxis) == 0)
        { vertPressed = false; }

        if (deadClock > 0)
        {
            deadClock -= Time.deltaTime;
            if (deadClock <= 0)
            {
                dPadVertPressed = false;
                dPadHortPressed = false;
                hortPressed = false;
                vertPressed = false;
            }
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
        if (Input.GetButtonDown(nextButton) || Input.GetKeyDown(nextKey) )
        {  return true; }
        return false;
    }

    public static bool PreviousButton()
    {
        if (Input.GetButtonDown(previousButton) || Input.GetKeyDown(previousKey) )
        {  return true; }
        return false;
    }

    public static bool DpadHortAsButton()
    {
        if (dPadHortPressed == false && (Input.GetAxis(dPadHortButton) != 0))
        {
            dPadHortPressed = true;
            return true;
        }
        return false;
    }

    public static bool DpadVertAsButton()
    {
        if (dPadVertPressed == false && Input.GetAxis(dPadVertButton) != 0)
        {
            deadClock = deadtime;
            dPadVertPressed = true;
            return true;
        }
        return false;
    }



    public static bool VertAsButton()
    {
        if (vertPressed == false && Input.GetAxis(vertAxis) != 0)
        {
            deadClock = deadtime;
            vertPressed = true;
            return true;
        }
        return false;
    }

    public static bool HortAsButton()
    {
        if (hortPressed == false && (Input.GetAxis(hortAxis) != 0))
        {
            deadClock = deadtime;
            hortPressed = true;
            return true;
        }
        return false;
    }

    public static float HorizontalAxis()
    { return Input.GetAxis(hortAxis); }

    public static float VerticalAxis()
    { return Input.GetAxis(vertAxis); }

    public static float DpadHort()
    {

        return Input.GetAxis(dPadHortButton);
    }

    public static float DpadVert()
    {

        return Input.GetAxis(dPadVertButton);
    }



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
