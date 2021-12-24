using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grub : Item
{
   

    public override bool Interact(Item _item)
    {
        //for being dug up
        if (_item.GetComponent<Shovel>())
        {
            //Leave a hole behind, but dont shunt the player off their space
            Vector3 pos = GameManager.instance.player.transform.position + GameManager.instance.player.transform.forward;

            GameManager.instance.InteractWithGround(pos, "dig");
            GameManager.instance.CatchBug(this);
            return true;
        }

        return false;


    }


    public override Item Hold(Player _player)
    {
        //the bugs are bait for the fishing rod, if it is in the inventory it should smart switch to that and not the bug itself when picking from the inventory
        Item usedWith = _player.inventory.GetFromPockets(ItemClass.fishingrod);

        if (usedWith != null)
        {
            usedWith.SetSubItem(this);
            //stackSize--;
            return usedWith;
        }

        return this;
    }


}
