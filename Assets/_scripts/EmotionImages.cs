using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EmotionImages 
{
    public static Material GetEmotion(Mood _mood)
    {
        string emotionToLoad = "emotions/";

        emotionToLoad += _mood.ToString();
        //if (_mood == Mood.angry)
        //{
        //    emotionToLoad += "angry";
        //}
        //else if (_mood == Mood.happy)
        //{
        //    emotionToLoad += "happy";

        //}
        //else if (_mood == Mood.tired)
        //{
        //    emotionToLoad += "tired";
        //}
        //else if (_mood == Mood.sad)
        //{
        //    emotionToLoad += "sad";
        //}

        return Resources.Load<Material>(emotionToLoad);

    }


    //to catch questions. alert and other elements that arent 'moods'
    public static Material GetThought(string _thought)
    {
        string thoughtToLoad = "emotions/";

        thoughtToLoad += _thought;


        return Resources.Load<Material>(thoughtToLoad);

    }




}
