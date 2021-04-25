using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : Item
{
    // Start is called before the first frame update
    public override void Catch()
    {
        GameManager.instance.CatchBug(this);

    }
}
