using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class axe : Item
{


    public override bool Use(Player _user,RaycastHit _hit)
    {
        RaycastHit hit;
        // if (Physics.SphereCast(transform.position + (Vector3.up * 0.5f), 0.2f, transform.TransformDirection(Vector3.forward), out hit, 0.3f))
        if (Physics.SphereCast(_user.transform.position + (Vector3.up * 0.65f), 0.05f, transform.TransformDirection(Vector3.forward), out hit, 1.0f))
        {

            if (hit.transform.GetComponent<Tree>() != null)
            {
                hit.transform.GetComponent<Tree>().Chop();

                return true;
            }
        }
         


        return false;
    }


}
