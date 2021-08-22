using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusScene : ScriptableScene
{
    public Villager licon;


    public override void Init()
    {
        PlaceVillagers();


    }



    public override void RunningScene()
    {
        if (stage == 0)
        {
            if (Vector3.Distance(licon.transform.position, checkPoints[stage].position) > 3)
            {
                licon.WarpNavMesh(checkPoints[0].position);
            }
           


            if (Vector3.Distance(licon.transform.position, checkPoints[1].position) < 0.5f)
            {
               // licon.SetNavMesh(false);
                NextStage();
            }
            else { licon.SetNavMeshDestination(checkPoints[1].position); }

        }
        else if (stage == 1)
        {
           
                Quaternion targetRotation = Quaternion.LookRotation(playerStartingPosition.position - licon.transform.position);

                licon.transform.rotation = Quaternion.Slerp(licon.transform.rotation, targetRotation, 1 * Time.deltaTime);
                float angle = Vector3.Angle(playerStartingPosition.position - licon.transform.position, licon.transform.forward);

                //larger turnAngle will have a rounder run arc instead of angular turns
                if (angle == 0)
                {

                    NextStage();

                }
                
            

        }
        else if (stage == 2)
        {
          

        }

    }


    public override void NextStage()
    {
        if (stage == -1)
        {
            licon.Teleport(checkPoints[0]);
            licon.SetNavMesh(true);
            
            licon.WarpNavMesh(checkPoints[0].position);
            licon.SetNavMeshDestination(checkPoints[1].position);
        }
        else if (stage == 1)
        {
           
            licon.SetAnimatorParameter("sitting",true);
           // StartYarn();
        }
        else if (stage == 2)
        {

        }

        stage++;
    }


}
