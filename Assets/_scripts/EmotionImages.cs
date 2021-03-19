using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EmotionImages 
{
    private static Material angry;


    public static Material Angry()
    {
        if (angry == null)
        {
            angry = Resources.Load<Material>("emotions/angry");
        }
        return angry;
    }

}
