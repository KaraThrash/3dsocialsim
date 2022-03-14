using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundbreaking_scene : ScenePlanning
{
    private Villager licon, wilms, fish;

    public Transform digSpot;

    public override void Setup()
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

        licon = Gamemanager().FindVillager(Villagers.licon);
        SetupVillager(licon);
        wilms = Gamemanager().FindVillager(Villagers.wilms);
        SetupVillager(wilms);
        fish = Gamemanager().FindVillager(Villagers.fish);
        SetupVillager(fish);

        

    }

    public  void SetupVillager(Villager _villager)
    {
        _villager.StoryState(VillagerStoryState.inScene);
        _villager.watchPlayer = false;
    }


    public override void StageZero()
    {
        if (Vector3.Distance(Player().transform.position, playerSpot.transform.position) < 0.25f)
        {
            Player().SetNavMeshSpeed(0);
            Player().SetNavMeshVelocity(Vector3.zero);

            Gamemanager().AdvanceDialogue();
            Player().SetAnimationParameter(Player().Animator(), "speed", 0);
            Player().SetAnimationParameter(Player().Animator(), "walk", false);
            Stage(1);
        }
    
    }

    public override void StageOne()
    {

        Player().LookAtAction(Focus(), 2);

        licon.transform.rotation = Quaternion.Slerp(licon.transform.rotation, Quaternion.LookRotation(Focus().position - licon.transform.position), 1 * Time.deltaTime);
        wilms.transform.rotation = Quaternion.Slerp(wilms.transform.rotation, Quaternion.LookRotation(Focus().position - wilms.transform.position), 1 * Time.deltaTime);
        fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, Quaternion.LookRotation(Focus().position - fish.transform.position), 1 * Time.deltaTime);


   

    }

    public override void SceneSpecificAction()
    {
        timer = 1;
        Player().PlayAnimation("put_in_pocket");
        
        Stage(2);
    }


    public override void StageTwo()
    {
        if (timer <= 0)
        {
            Focus().position = Player().transform.position;

            licon.transform.rotation = Quaternion.Slerp(licon.transform.rotation, Quaternion.LookRotation(Focus().position - licon.transform.position), 1 * Time.deltaTime);
            wilms.transform.rotation = Quaternion.Slerp(wilms.transform.rotation, Quaternion.LookRotation(Focus().position - wilms.transform.position), 1 * Time.deltaTime);
            fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, Quaternion.LookRotation(Focus().position - fish.transform.position), 1 * Time.deltaTime);

            if (Vector3.Distance(Player().transform.position, digSpot.transform.position) < 0.25f)
            {
                Player().SetNavMeshSpeed(0);
                Player().SetNavMeshVelocity(Vector3.zero);

                Player().SetAnimationParameter(Player().Animator(), "speed", 0);
                Player().SetAnimationParameter(Player().Animator(), "walk", false);


                Player().UseItem();

                licon.PlayAnimation("laugh");
                wilms.PlayAnimation("nervous");
                fish.PlayAnimation("nervous");

                Stage(3);
            }
        }
        else 
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Player().SetHeldItem(Player().inventory.GetFromPockets(ItemClass.shovel));
                Player().SetAnimationParameter(Player().Animator(), "speed", Player().GetVelocity().magnitude);
                Player().SetAnimationParameter(Player().Animator(), "walk", true);
                Player().SetNavLeadObject(digSpot.transform.position, 2);
            }
        }
        

    }

    public override void StageThree()
    {
        if (Gamemanager().DialogueIsRunning())
        {

            Player().State(PlayerState.playerControlled);


            Gamemanager().ActiveObject(null);
            Gamemanager().SceneDirector().PlannedScene(null);
            Gamemanager().cameraControls.State(CameraState.basic);
        }
    }


    public override void ProcessTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && activated == false)
        {
            activated = true;
            
            Player().State(PlayerState.inScene);
            Player().SetAnimationParameter(Player().Animator(), "speed", Player().GetVelocity().magnitude);
            Player().SetAnimationParameter(Player().Animator(), "walk", true);
            Player().SetNavLeadObject(playerSpot.transform.position, 2);

            Gamemanager().StartDialogue(yarnNodeTitle);

            Gamemanager().ActiveObject(Focus());
            Gamemanager().SceneDirector().PlannedScene(this);
            Gamemanager().cameraControls.State(CameraState.conversation);

            Stage(0);

        }
    }


}
