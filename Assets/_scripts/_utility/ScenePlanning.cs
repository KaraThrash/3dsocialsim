using System.Collections.Generic;
using UnityEngine;

public class ScenePlanning : MonoBehaviour
{
    private GameManager gameManager;
    private Player player;
    public WorldLocation location;
    public List<StandInForScenePlanning> villagerStandIns;

    public StandInForScenePlanning playerSpot;

    public List<Transform> keyPositions;
    public Transform sceneFocus; //where characters should be looking
    public string yarnNodeTitle;

    public int stage;
    public bool activated = false;

    protected float timer;

    public void Start()
    {
        Setup();
    }

    public virtual void Setup()
    {
        if (villagerStandIns != null)
        {
            foreach (StandInForScenePlanning el in villagerStandIns)
            {
                Villager villagerToMove = Gamemanager().FindVillager(el.villager);
                villagerToMove.transform.position = el.transform.position;
                villagerToMove.WarpNavMesh(el.transform.position);
                villagerToMove.transform.rotation = el.transform.rotation;
                el.gameObject.SetActive(false);
            }
        }
        playerSpot.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Stage() != -1)
        {
            SceneIsRunning();
        }
    }

    public virtual void SceneIsRunning()
    {
        if (stage == 0)
        {
            StageZero();
        }
        else if (stage == 1)
        {
            StageOne();
        }
        else if (stage == 2)
        {
            StageTwo();
        }
    }

    public virtual void StageZero()
    { }

    public virtual void StageOne()
    { }

    public virtual void StageTwo()
    { }

    public virtual void StageThree()
    { }

    public virtual void SceneSpecificAction()
    { }

    public int Stage()
    { return stage; }

    public void Stage(int _stage)
    { stage = _stage; }

    public void OnTriggerEnter(Collider other)
    {
        ProcessTriggerEnter(other);
    }

    public virtual void ProcessTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && activated == false)
        {
            activated = true;
            Gamemanager().GetPlayer().SetNavLeadObject(playerSpot.transform.position, 10);
            Gamemanager().StartDialogue(yarnNodeTitle);
        }
    }

    public Transform Focus()
    {
        if (sceneFocus == null)
        {
            sceneFocus = new GameObject().transform;
        }
        return sceneFocus;
    }

    public GameManager Gamemanager()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.instance;
        }
        return gameManager;
    }

    public Player Player()
    {
        if (player == null)
        {
            player = Gamemanager().GetPlayer();
        }
        return player;
    }
}