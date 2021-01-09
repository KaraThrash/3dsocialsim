using System;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    public GameManager gameManager;

    public string npcName,mood;
    public float rotSpeed,speed,timeBetweenGreetings = 120.0f; //time between greetings is the value of seconds that need to elapse before this npc will use a greeting instead of smalltalk
    public DateTime lastInteractionDate;
    private Dialogue activeDialogue ;

    void Start()
    {
        
    }

    void Update()
    {
       // if (Input.GetKeyDown(KeyCode.Space)) { Interact(); }
        //if (Input.GetKeyDown(KeyCode.N)) { activeDialogue = null; }

        if (activeDialogue != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(gameManager.GetPlayer().transform.position - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }



    public void Interact()
    {
        if (activeDialogue == null)
        {
            if ((lastInteractionDate - DateTime.Now).TotalSeconds >= timeBetweenGreetings)
            {
                activeDialogue = FindDialogue("greeting");
            }
            else 
            {
                activeDialogue = FindDialogue("smalltalk");
            }
            
        }

   


        //send the next line of dialogue to the gamemanager to display in the chat box
        gameManager.ShowDialogue(npcName,activeDialogue.NextDialogueLine());


        if (activeDialogue.EndOfDialogue())
        {  activeDialogue = null; }
    }


    



    

    public Dialogue FindDialogue(string _type="smalltalk")
    {
        List<Dialogue> dialogueList = DialogueLoader.GetDialogue(npcName);
        List<Dialogue> listToRandomize = new List<Dialogue>();

        for (int i = 0; i < dialogueList.Count; i++)
        {
            if (dialogueList[i].mood == mood && dialogueList[i].type == _type)
            {
                listToRandomize.Add(dialogueList[i]);
            }
        
        }

        return listToRandomize[(int)UnityEngine.Random.Range(0,listToRandomize.Count)];
    }

}
