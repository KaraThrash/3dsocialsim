
using System.Collections.Generic;
using UnityEngine;


    public class Dialogue
    {
        public string type; //greeting, farewell, smalltalk, question
        public string mood; //happy,sad,angry,etc
    public int currentLine;
        public List<string> body;
        //e.g. "hello character do you like my dress?"
        //      -yes- -no-
        //      "response text"

        public Dialogue(string _type, string _mood, List<string> _body)
        {
            type = _type;
            mood = _mood;
            body = new List<string>();
            body.AddRange(_body);
            Debug.Log(_body.Count);
            currentLine = -1;
        }

        public string NextDialogueLine()
        {
            currentLine++;
            //get next line of the conversation and progress the point
            if (body.Count > currentLine)
            {
                
                return body[currentLine];
            }

            

            return "";
        }

        public void ResetCurrentLine()
        {
        currentLine = -1;
        }

        public bool EndOfDialogue()
        {

            //if the last line of dialogue was given reset the line tracker
            if (currentLine >= body.Count ) 
            { 
             currentLine = -1;
            return true;
            }

            return false; 
    
        }

    }