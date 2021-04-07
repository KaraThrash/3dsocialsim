using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : Item
{
    public bool open;
    public GameObject holeCollider;
    public Material openDirt, buriedGrass;
    public MeshRenderer renderer;

    public Item buriedItem;

    // Start is called before the first frame update

    public Item GetItem() { return buriedItem; }

    public void Bury(Item _item) 
    {
        if (open) 
        {
            if (_item == null)
            {
                Destroy(this.gameObject);
            }
            else 
            {
                buriedItem = _item;
                open = false;

                if (renderer != null)
                { renderer.material = buriedGrass; }

                if (holeCollider != null)
                { holeCollider.SetActive(false); }
            }
           

        }
    }

    public void Dig(Item _item)
    {
        if (!open && GetItem() != null)
        {
            open = true;

            if (renderer != null)
            { renderer.material = openDirt; }

            if (holeCollider != null)
            { holeCollider.SetActive(true); }

            //TODO: get buried item

        }
    }

}
