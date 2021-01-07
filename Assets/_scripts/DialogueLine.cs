using System.Collections;
using System.Collections.Generic;

public class DialogueLine
{
    public string type; //greeting, farewell, smalltalk, question
    public string mood; //happy,sad,angry,etc

    public List<string> body;
    //e.g. "hello character do you like my dress?"
    //      -yes- -no-
    //      "response text"

    public DialogueLine(string _type, string _mood, List<string> _body)
    {
        type = _type;
        mood = _mood;
        body = _body;
    }

}
