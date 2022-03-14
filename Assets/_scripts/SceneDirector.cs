using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Yarn.Unity;
public class SceneDirector : MonoBehaviour
{
    public SceneAction currentaction;
    public ScriptableScene activeScene;

    private ScenePlanning plannedScene;

    public Player player;

    public Villager primary, secondary;

    public Vector3 startPos, endPos, checkPoint;

    public List<Villager> villagers;

    public List<MapLocation> locations;

    public bool sceneActive = false,waitForCheckpoint = false;

    public SceneObject currentScene;
    public Transform checkPointTransform;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentScene != null) { currentaction = currentScene.actionType; }

        if (sceneActive) 
        {

            if (waitForCheckpoint) 
            {
                

            }

            if (currentScene.actionType == SceneAction.walkAndTalk)
            {
              //  WalkAndTalk();
            }
            else if (currentScene.actionType == SceneAction.leadPlayer)
            {
                LeadPlayer();
            }
            else if (currentScene.actionType == SceneAction.walkingToCheckpoint)
            {
                CheckForCheckPoint();
            }
            else 
            {
                SceneActions.RotateToFace(primary.transform,player.transform);
                GameManager.instance.SetContinueButton(true);

            }
        }
    }

    public void CheckForCheckPoint()
    {
        //check if a final location was tagged, and if so make sure the villager is going there
        if (checkPoint != Vector3.zero)
        {
            
                if (Vector3.Distance(checkPoint, primary.transform.position) > 1)
                {
                    SceneActions.LeadPlayer(primary.transform, checkPoint, player, primary.GetNavMeshSpeed());
                    GameManager.instance.SetContinueButton(false);
                    return;
                }
         
        }
        else 
        {
          //  return;
        }

        currentScene.actionType = SceneAction.none;
        GameManager.instance.SetContinueButton(true);
        //RunDialogue();

    }


    public void EndScene()
    {
        if (primary != null)
        {
            primary.State(VillagerState.waiting);
            primary.StoryState(VillagerStoryState.idle);
            primary.SetNavMeshDestination(primary.transform.position);
        }
        if (player != null)
        {

            player.EndNavLeadObject();
            player.State(PlayerState.playerControlled);
        }


        waitForCheckpoint = false;
        sceneActive = false;
        GameManager.instance.SetContinueButton(true);


    }


    public void WalkAndTalk()
    {
        if (primary == null) { return; }

        if ( Vector3.Distance(primary.transform.position, currentScene.targetPos) < 1  ) 
        {
            EndScene();
            return;  
        } 

        SceneActions.HavePlayerFollow(primary.transform, currentScene.targetPos, player, 1);

        if (currentScene.linesOfDialogue > 0)
        {
            RunDialogueByDistance();
        }



    }


    public void LeadPlayer()
    {
        if (primary == null) { return; }

        if ( (Vector3.Distance(primary.transform.position, currentScene.targetPos) < 1 ))
        {
            //GameManager.instance.dialogueRunner.GetComponent<DialogueUI>().MarkLineComplete();
            if (checkPoint != Vector3.zero)
            { GameManager.instance.SetContinueButton(false); }

            endPos = currentScene.targetPos;
            currentScene.actionType = SceneAction.walkingToCheckpoint;


          //  GameManager.instance.SetContinueButton(false);
            // ReEnabledContinueButton();
            //EndScene();

            // RunDialogue();
            return;
        }

        SceneActions.LeadPlayer(primary.transform, currentScene.targetPos, player, currentScene.primarySpeed);

        if (currentScene.linesOfDialogue > currentScene.phase)
        {
          //  RunDialogueByDistance();
        }
       // else { ReEnabledContinueButton(); }



    }

   public void  RunDialogueByDistance()
   {

        if (Vector3.Distance(primary.transform.position, currentScene.startPos) >= (currentScene.distanceIncrement * 1) && currentScene.linesOfDialogue > currentScene.phase)
        {
            currentScene.startPos = primary.transform.position;
            currentScene.phase++;
            RunDialogue();
        }
        else { currentScene.actionType = SceneAction.walkingToCheckpoint; }


    }


    public void RunDialogue()
    {

        
    
            GameManager.instance.dialogueRunner.GetComponent<DialogueUI>().MarkLineComplete();
        


    }




    public void SetCheckPointPosition(Vector3 _pos)
    {
        if (checkPointTransform != null)
        {
            checkPointTransform.position = _pos;
            waitForCheckpoint = true;

        }
    }

    public void SetCheckPointPosition(Quaternion _rot)
    {
        if (checkPointTransform != null)
        {
            checkPointTransform.rotation = _rot;

        }
    }



    public void InitScene(ScriptableScene _scene)
    {

       



        activeScene = _scene;
    }

    public void PlannedScene(ScenePlanning _scene)
    {
         plannedScene = _scene;
    }

    public ScenePlanning PlannedScene()
    {
        return plannedScene;
    }
}
