using System.Collections.Generic;
using UnityEngine;

public class StoryTracking : MonoBehaviour
{
    public List<DayYarnList> narrativeByDay;

    private int day;
    private int sectionOfCurrentDay;

    private int dayOfPreviousScript;
    private int sectionOfPreviousScript;

    private string yarnNode = "node";

    private Villagers nextStoryStart;

    //as a safety check: this should be toggled on at the start of a dialogue and then off at the end
    //this way if 'dialoguestarted' is toggled on and a new dialogue is attempting to start we know there was a problem with the previous dialogue
    private bool dialogueStarted;

    // Start is
    // called before
    // the first
    // frame update
    private void Start()
    {
        day = 1;
        nextStoryStart = Villagers.licon;
    }

    // Update is
    // called once
    // per frame
    private void Update()
    {
    }

    public YarnProgram StartSection()
    {
        YarnProgram dialogue = GetYarn(day, sectionOfCurrentDay);
        Debug.Log("day and section: " + day + " : " + sectionOfCurrentDay);

        if (dialogue != null)
        {
            Debug.Log("dialogue: " + dialogue);
            dayOfPreviousScript = day;
            sectionOfPreviousScript = sectionOfCurrentDay;

            sectionOfCurrentDay++;

            dialogueStarted = true;
            return dialogue;
        }

        return null;
    }

    public YarnProgram StartSection(int _day, int _section)
    {
        if (dialogueStarted)
        {
            if (_day > dayOfPreviousScript || _section > sectionOfPreviousScript)
            {
                //TODO: this case means the last script didnt finish but the next one is trying to run
                //TODO: plan safety checks for erros from yarn, either technical or clerical
            }
        }

        YarnProgram dialogue = GetYarn(_day, _section);
        if (dialogue != null)
        {
            dayOfPreviousScript = _day;
            sectionOfPreviousScript = _section;
            dialogueStarted = true;
            return dialogue;
        }

        return null;
    }

    public void EndSection(int _day, int _section)
    {
        dialogueStarted = false;
    }

    public void FromYarnSetStoryElements()
    {
        //TODO: create elements that can be flagged from yarn to let narrative set blockers and conditionals for how the narrative is presented
    }

    public YarnProgram GetYarn(int _day, int _section)
    {
        if (narrativeByDay != null)
        {
            if (narrativeByDay.Count > _day)
            {
                return narrativeByDay[_day].GetYarn(_section);
            }
        }

        return null;
    }

    public Villagers NextNarrativeActor()
    {
        return nextStoryStart;
    }

    public string NodeTitle()
    { return yarnNode; }

    public int GetDay()
    { return day; }
}