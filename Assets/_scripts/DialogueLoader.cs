using System.Collections.Generic;
using UnityEngine;

public class DialogueLoader
{
    //dictionary by name for npc's dialogue. When an npc is encountered load their dialogue into this dictionary from the spreadsheet
    // npc name: dialogue type
    public static Dictionary<string, List<Dialogue>> loadedDialogue;

    public static void IntializeDialogueDictionary()
    {
        Debug.Log("Intializing master dictionary");
        loadedDialogue = new Dictionary<string, List<Dialogue>>();

        LoadCharacterDialogue("Generic");
    }

    public static List<Dialogue> GetDialogue(string _characterName)
    {
        if (loadedDialogue == null)
        {
            IntializeDialogueDictionary();
        }

        //if the character's dialogue is in the dictionary return it
        if (loadedDialogue.ContainsKey(_characterName))
        {
            return loadedDialogue[_characterName];
        }
        else
        {
            //otherwise load the dialogue.
            LoadCharacterDialogue(_characterName);

            if (loadedDialogue.ContainsKey(_characterName))
            {
                return loadedDialogue[_characterName];
            }
        }
        //in the case there is no dialogue for this character load the 'generic' dialogue

        if (loadedDialogue.ContainsKey("Generic") == false)
        {
        }

        return loadedDialogue["Generic"];
    }

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

            // MasterDialogueList.Add(newitem);

            count++;
        }
    }

    //load only one characters dialogue, and only keep in memory the ones that are active
    //character dialogue saved in the Resources/Character_Dialogue folder
    public static void LoadCharacterDialogue(string _characterName)
    {
        if (Resources.Load<TextAsset>("Character_Dialogue/" + _characterName) == null)
        {
            Debug.Log("No dialogue for that character found");

            if (_characterName != "Generic") { LoadCharacterDialogue("Generic"); }
            return;
        }

        if (loadedDialogue == null)
        {
            IntializeDialogueDictionary();
        }

        // string text = File.ReadAllText("./Resources/DialogueSpreadsheet.txt");
        //Load a text file (Assets/Resources/....)
        string text = Resources.Load<TextAsset>("Character_Dialogue/" + _characterName).ToString();
        string[] strValues = text.Split('\n');

        int count = 1; //0 is the header
        while (count < strValues.Length)
        {
            string[] tempstring = strValues[count].Split(',');

            //if this is the dialogue for the specified character add it to the master dictionary
            //each entry should be a minimum of 4 elements: name, type, mood, body
            if (tempstring.Length > 3 && tempstring[3] != "")
            {
                //check if the character already has an entry, if not then create one
                if (loadedDialogue.ContainsKey(_characterName) == false)
                {
                    loadedDialogue.Add(_characterName, new List<Dialogue>());
                }

                List<string> newbody = new List<string>();

                for (int i = 3; i < tempstring.Length; i++)
                {
                    //make sure no blank lines are mistakenly added due to text formatting
                    if (tempstring[i].Length > 1)
                    { newbody.Add(tempstring[i]); }
                }

                //make sure there were valid dialogue lines after accounting for text formatting mistakes
                if (newbody.Count > 0)
                {
                    //make a new dialogueline object from the spreadsheet entry // string:type string:mood List<string>:dialogue body by line
                    Dialogue tempLine = new Dialogue(tempstring[1], tempstring[2], newbody);

                    loadedDialogue[_characterName].Add(tempLine);
                }
            }

            count++;
        }
    }
}