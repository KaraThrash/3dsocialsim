using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : Item
{

    // Start is called before the first frame update

    public override bool TryCatch()
    {
       

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward, 1);


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Item>() != null && hitCollider.GetComponent<Item>().Catch(this))
            {
                return true;
            }
        }

            return false;
    }

}
