using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    public GameManager gameManager;

    public string npcName,mood;
    public float rotSpeed,speed;
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
            activeDialogue = FindDialogue();
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
            if (dialogueList[i].type == _type)
            {
                listToRandomize.Add(dialogueList[i]);
            }
        
        }

        return listToRandomize[(int)Random.Range(0,listToRandomize.Count)];
    }

}
