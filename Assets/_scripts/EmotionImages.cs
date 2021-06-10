using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EmotionImages 
{
    private static Material angry;
    public static Material GetEmotion(Mood _mood)
    {
        string emotionToLoad = "emotions/";
        if (_mood == Mood.angry)
        {
            emotionToLoad += "angry";
        }
        else if (_mood == Mood.happy)
        {
            emotionToLoad += "happy";

        }
        else if (_mood == Mood.tired)
        {
            emotionToLoad += "tired";
        }
        else if (_mood == Mood.sad)
        {
            emotionToLoad += "sad";
        }

        return Resources.Load<Material>(emotionToLoad);

    }

    public static Material Angry()
    {
        if (angry == null)
        {
            angry = Resources.Load<Material>("emotions/angry");
        }
        return angry;
    }

}
