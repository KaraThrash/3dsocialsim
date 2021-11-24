using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item
{


    public override bool Use(Player _player)
    {
       

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + _player.transform.forward, 1);


        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Item>() != null && hitCollider.GetComponent<Item>().Interact(this))
            {
                return true;
            }
        }

        return false;
    }


}
