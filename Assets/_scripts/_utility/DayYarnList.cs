using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YarnForDay_Config")]
public class DayYarnList : ScriptableObject
{
    public List<YarnProgram> narrativeSections;

    public YarnProgram GetYarn(int _section)
    {
        if (narrativeSections != null)
        {
            if (narrativeSections.Count >_section )
            {
                return narrativeSections[_section];
            }
        }

        

        return null;
    }
}
