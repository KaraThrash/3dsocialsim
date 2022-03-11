using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePlanning : MonoBehaviour
{
    public MapLocation location;
    public List<StandInForScenePlanning> villagerStandIns;
    public StandInForScenePlanning playerSpot;

    public string yarnNodeTitle;

    private bool activated = false;

    public void Start()
    {
        Setup();
    }

    public void Setup()
    {
        if (villagerStandIns != null)
        {
            foreach (StandInForScenePlanning el in villagerStandIns)
            {
                Villager villagerToMove = GameManager.instance.FindVillager(el.villager);
                villagerToMove.WarpNavMesh(el.transform.position);
                villagerToMove.transform.rotation = el.transform.rotation;
                el.gameObject.SetActive(false);
            }
        }
        playerSpot.gameObject.SetActive(false);

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && activated == false)
        {
            activated = true;
            GameManager.instance.StartDialogue(yarnNodeTitle);

        }
    }

}
