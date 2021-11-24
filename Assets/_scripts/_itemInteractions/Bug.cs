using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : Item
{
    // Start is called before the first frame update
    public override bool Catch()
    {
        GameManager.instance.CatchBug(this);
        return true;
    }

    public override bool Catch(Item _item)
    {
        if (_item.GetComponent<Net>() != null)
        {
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
