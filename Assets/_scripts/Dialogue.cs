using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dialogue 
{

    //dictionary by name for npc's dialogue. When an npc is encountered load their dialogue into this dictionary from the spreadsheet
    // npc name: dialogue type
    public static Dictionary<string, List<DialogueLine> > loadedDialogue;




    public static void LoadAllDialogueFromFile()
    {
        // string text = File.ReadAllText("./Resources/DialogueSpreadsheet.txt");
        //Load a text file (Assets/Resources/....)
        string text = Resources.Load<TextAsset>("DialogueSpreadsheet").ToString();
        string[] strValues = text.Split('\n');


        int count = 1; //0 is the header
        while (count < strValues.Length)
        {
            string[] tempstring = strValues[count].Split(',');





            //  MasterDialogueList.Add(newitem);



            count++;

        }


    }



    //load only one characters dialogue, and only keep in memory the ones that are active
    public static void LoadCharacterDialogue(string _characterName)
    {
        if (loadedDialogue == null)
        {
            Debug.Log("Intializing master dictionary");
            loadedDialogue = new Dictionary<string, List<DialogueLine> >();
        }


        // string text = File.ReadAllText("./Resources/DialogueSpreadsheet.txt");
        //Load a text file (Assets/Resources/....)
        string text = Resources.Load<TextAsset>("DialogueSpreadsheet").ToString();
        string[] strValues = text.Split('\n');


        int count = 1; //0 is the header
        while (count < strValues.Length)
        {
            string[] tempstring = strValues[count].Split(',');

            //if this is the dialogue for the specified character add it to the master dictionary
            //each entry should be a minimum of 4 elements: name, type, mood, body
            if (tempstring.Length > 3 && tempstring[0].Equals(_characterName))
            {

                //check if the character already has an entry, if not then create one
                if (loadedDialogue.ContainsKey(_characterName) == false)
                {
                    loadedDialogue.Add(_characterName, new List < DialogueLine >  ());
                }

                
                    List<string> newbody = new List<string>(tempstring[3].Split(';'));

                    //make a new dialogueline object from the spreadsheet entry // string:type string:mood List<string>:dialogue body by line
                    DialogueLine tempLine = new DialogueLine(tempstring[1], tempstring[2], newbody);

                loadedDialogue[_characterName].Add(tempLine);
            }



            count++;

        }


    }



}
