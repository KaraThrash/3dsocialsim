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
}
