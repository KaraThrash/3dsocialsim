using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : Item
{
    public bool open;
    public GameObject holeCollider,buriedIndicator;
    public Material openDirt, buriedGrass;
    public MeshRenderer renderer;

    public Item buriedItem;

    // Start is called before the first frame update


    public override bool Interact(Item _item)
    {

        if (_item.GetComponent<Shovel>() )
        {
            //just cover the hole
            Bury(null);
        }
        else 
        {
            Bury(_item);
        }


        return true;
    }








    public Item GetItem() { return buriedItem; }

    public void Bury(Item _item) 
    {
      //  if (open) 
      //  {
            if (_item == null)
            {
                Destroy(this.gameObject);
            }
            else 
            {
                buriedItem = _item;
                open = false;
                buriedIndicator.SetActive(true);
                if (renderer != null)
                { renderer.material = buriedGrass; }

                if (holeCollider != null)
                { holeCollider.SetActive(false); }
            }
           

       // }
    }

    public void Dig(Item _item)
    {
        if (!open && GetItem() != null)
        {
            buriedIndicator.SetActive(false);
            open = true;

            if (renderer != null)
            { renderer.material = openDirt; }

            if (holeCollider != null)
            { holeCollider.SetActive(true); }

            //TODO: get buried item

        }
    }

}
