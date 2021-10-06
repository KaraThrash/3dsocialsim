using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Item
{

    private float catchPercent = 45;

    public override bool Use(Player _player,RaycastHit _hit)
    {

        if (_hit.transform.tag.Equals("grass"))
        {
            CheckGrubs();

            return true;
        }
        else if (_hit.transform.GetComponent<Hole>() != null)
        {
         

            return true;
        }
        else if (_hit.transform.GetComponent<Tree>() != null && _hit.transform.GetComponent<Tree>().JustStump())
        {
           

            return true;
        }

        return false;

    }

    public bool CheckGrubs()
    {
        float rnd = Random.Range(0,100);

        if (rnd > catchPercent)
        {
            if (rnd >= catchPercent * 2)
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        else
        {
       
        }


        return false;
    }
}
