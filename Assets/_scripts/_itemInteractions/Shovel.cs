using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : Item
{

    private float catchPercent = 45;

    public override bool Use(Player _player)
    {


        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward, 1);


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Item>() != null && hitCollider.GetComponent<Item>().Interact(this))
            {
                return true;
            }
        }

        // open holes in the ground and tree stumps would return true and break out, otherwise dig a new hole

        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up + transform.forward , Vector3.down, out hit, 3.5f))
        {
            Debug.Log(hit.point.ToString());

            if (hit.transform.tag.Equals("grass") || hit.transform.tag.Equals("dirt"))
            {
                GameManager.instance.InteractWithGround(hit.point, "dig");
                return true;
            }
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
