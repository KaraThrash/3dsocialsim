using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableScene : MonoBehaviour
{
    //the villagers for this scene
    //if already 'in' the world pull them over, 
    //public List<Villagers> villagers;

    public int stage; //to track progression through the scene for blocking and villager movement

    //allow these scenes to have their own scripts to allow for them to be called from a character without having to load them with all the dialogue and direction
    public YarnProgram scriptToLoad;

    //e.g. the bus: open the door, slow down, speed up etc.
    public Animator enviromentAnimator;

    public Transform cameraStartingPosition;
    public Transform playerStartingPosition;


    //the child objects should be dummy versions of the villagers for the scene which are then pulled in at placement time
    //This way we can check if they are already spawned into the 'world' proper or if they need to be instantiated
    public Transform villagerStartingLocations;

    //the  locations relevant for this scene
    public List<Transform> checkPoints;

    private void Start()
    {
        if (scriptToLoad != null)
        {
            GameManager.instance.dialogueRunner.Add(scriptToLoad);
        }
        Init();
        NextStage();
    }

    private void Update()
    {
        RunningScene();
    }

    public void StartYarn()
    {
        if (scriptToLoad != null)
        {
            GameManager.instance.dialogueRunner.StartDialogue("bus9");
        }
    }

    public virtual void Init()
    {

    }

    public void PlaceVillagers()
    {
        int count = villagerStartingLocations.childCount - 1;
        while (count >= 0)
        {
            if (villagerStartingLocations.childCount > count)
            {
                Villager villager = GameManager.instance.FindVillager(villagerStartingLocations.GetChild(count).name);

                if (villager != null)
                {
                    villager.Teleport(villagerStartingLocations.GetChild(count));

                }
            }

            count--;
        }
    }


    public virtual void RunningScene()
    { }

    public virtual void NextStage()
    {
        if (stage == 0)
        {
           
        }
        else if (stage == 1)
        {
  
        }
        else if (stage == 2)
        {

        }

        stage++;
    }


}
