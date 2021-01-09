using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public GameObject chatbox;
    public Text chatText,titleText;//for character names

    public Transform cam,activeObject, groundParent;
    public GameObject dirtsquare, grassSquare;

    private bool inConversation;
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InteractWithVillager(Villager _villager)
    {
        if (inConversation == false)
        { StartConversation(); }

        activeObject = _villager.transform;
        _villager.Interact();

    }



    public bool InConversation()
    {

        return inConversation;
    }






    public void StartConversation()
    {
        cam.GetComponent<CameraControls>().ConversationToggle(true);
        chatbox.SetActive(true);
        inConversation = true;
    }

    public void EndConversation()
    {
        cam.GetComponent<CameraControls>().ConversationToggle(false);
        chatbox.SetActive(false);
        inConversation = false;
    }

    public void ShowDialogue(string _line)
    {
        if (_line.Length == 0)
        {
            activeObject = null;
            EndConversation();
            return;
        }
        chatText.text = _line;
        titleText.text = "";
    }

    public void ShowDialogue(string _speakerName, string _line)
    {
        if (_line.Length == 0)
        {
            activeObject = null;
            EndConversation();
            return;
        }
        chatText.text = _line;
        titleText.text = _speakerName;
    }

    public Player GetPlayer()
    { return player; }

    //for cam/player focus when interacting
    public Transform GetActiveObject()
    {
        //if the activeobject isnt set, focus on the camera
        if (activeObject == null) { return cam; }
        return activeObject; 
    }

    public void MakeGroundGrid()
    {
        int xpos = -20, zpos = -20;

        while (zpos < 21)
        {
            xpos = -20;
            while (xpos < 21)
            {
                if (zpos % 5 == 0 || xpos % 5 == 0)
                { Instantiate(dirtsquare, new Vector3(xpos, -0.5f, zpos), transform.rotation); }
                else
                { Instantiate(grassSquare, new Vector3(xpos, -0.5f, zpos), transform.rotation); }

                xpos++;
            }
            zpos++;
        }
    }
}
