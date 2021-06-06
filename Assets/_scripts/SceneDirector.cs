using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yarn.Unity;
public class SceneDirector : MonoBehaviour
{
    public ScriptableScene activeScene;

    public Player player;

    public Villager primary, secondary;

    public Vector3 startPos, endPos;

    public List<Villager> villagers;
    public List<MapLocation> locations;

    public bool sceneActive = false;

    public SceneObject currentScene;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneActive) 
        {
            if (currentScene.actionType == SceneAction.walkAndTalk)
            {
                WalkAndTalk();
            }
            else if (currentScene.actionType == SceneAction.leadPlayer)
            {
                LeadPlayer();
            }
        }
    }


    public void EndScene()
    {
        if (currentScene.actionType == SceneAction.walkAndTalk)
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
                player.SetState(PlayerState.talking);
            }
        }
        else if (currentScene.actionType == SceneAction.leadPlayer)
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
                player.SetState(PlayerState.talking);
            }
        }
        else 
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
                player.SetState(PlayerState.talking);
            }
        }



        sceneActive = false;
        ReEnabledContinueButton();


    }

    public void ReEnabledContinueButton()
    {
        GameManager.instance.ContinueButton().GetComponent<Image>().enabled = true;
        GameManager.instance.ContinueButton().transform.GetChild(0).GetComponent<Text>().enabled = true;
        GameManager.instance.ContinueButton().interactable = true;

        EventSystem.current.SetSelectedGameObject(GameManager.instance.ContinueButton().gameObject);

    }

    public void WalkAndTalk()
    {
        if (primary == null) { return; }

        if (currentScene == null  || Vector3.Distance(primary.transform.position, currentScene.targetPos) < 1  ) 
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

        if (currentScene == null || Vector3.Distance(primary.transform.position, currentScene.targetPos) < 1)
        {
            ReEnabledContinueButton();
            //EndScene();
            return;
        }

        SceneActions.LeadPlayer(primary.transform, currentScene.targetPos, player, currentScene.primarySpeed);

        if (currentScene.linesOfDialogue > 0)
        {
            RunDialogueByDistance();
        }



    }

   public void  RunDialogueByDistance()
   {

        if (Vector3.Distance(primary.transform.position, currentScene.startPos) >= (currentScene.distanceIncrement * 1) && currentScene.linesOfDialogue > currentScene.phase)
        {
            currentScene.startPos = primary.transform.position;
            currentScene.phase++;
            GameManager.instance.dialogueRunner.GetComponent<DialogueUI>().MarkLineComplete();
        }


    }






    public void InitScene(ScriptableScene _scene)
    {

        primary = _scene.primary;
        secondary = _scene.secondary;



        activeScene = _scene;
    }


}
