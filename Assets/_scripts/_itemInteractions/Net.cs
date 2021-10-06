using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : Item
{

    // Start is called before the first frame update

    public override bool TryCatch()
    {
        //Vector3 dir = Vector3.down + (Vector3.forward * 3);

        //RaycastHit hit;
        //// if (Physics.SphereCast(transform.position + (Vector3.up * 0.5f), 0.2f, transform.TransformDirection(Vector3.forward), out hit, 0.3f))
        //if (Physics.SphereCast(transform.position + (Vector3.up * 0.65f), 0.05f, transform.TransformDirection(dir.normalized), out hit, 3.5f))
        //{ }

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
