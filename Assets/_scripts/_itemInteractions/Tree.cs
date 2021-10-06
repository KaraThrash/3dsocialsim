using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Item
{
    public GameObject stump, trunk, leaves;



    public void Chop()
    {
        if (trunk.activeSelf)
        {
            trunk.SetActive(false);
            leaves.SetActive(false);
            TakedownNotice();
        }
    }

    public bool JustStump()
    {
        return !trunk.activeSelf;
    }

    public override void Dig()
    {

        GameManager.instance.InteractWithGround(transform.position, "dig");
        Destroy(this.gameObject);
    }

    public override bool Interact(Player _player)
    {
        if (trunk.activeSelf && notice != null)
        {
            if (notice.activeSelf)
            {
                notice.SetActive(false);
            }
            else 
            {
                notice.SetActive(true);
            }
            

        }

        return true;
    }

    public override void HangNotice()
    {
        if (trunk.activeSelf &&  notice != null)
        {
            notice.SetActive(true);
        }
    }

    public override void TakedownNotice()
    {
        if (notice != null)
        {
            notice.SetActive(false);
        }
    }

}
