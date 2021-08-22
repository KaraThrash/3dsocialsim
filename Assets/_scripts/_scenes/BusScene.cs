using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusScene : ScriptableScene
{
    public Villager licon;

    public float rotSpeed;

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
           


            if (Vector3.Distance(licon.transform.position, checkPoints[1].position) < 0.05f)
            {
               // licon.SetNavMesh(false);
                NextStage();
            }
            else { licon.SetNavMeshDestination(checkPoints[1].position); }

        }
        else if (stage == 1)
        {

         

            Quaternion targetRotation = Quaternion.LookRotation( licon.transform.position - (licon.transform.position + new Vector3(0, 0, 1)) );

                licon.transform.rotation = Quaternion.Slerp(licon.transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
               // float angle = Vector3.Angle(playerStartingPosition.position - licon.transform.position, licon.transform.forward);
                float angle = Vector3.Angle(licon.transform.position - (licon.transform.position + new Vector3(0, 0, 1)), licon.transform.forward);

        
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

            SetAnimatorParameter("advance");
        }
        else if (stage == 0)
        {
            licon.SetNavMesh(false);
        }
        else if (stage == 1)
        {
           
            licon.SetAnimatorParameter("sitting",true);
           StartYarn();
        }
        else if (stage == 2)
        {

        }

        stage++;
    }


}
