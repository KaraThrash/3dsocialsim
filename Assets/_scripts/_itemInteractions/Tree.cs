using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Item
{
    public GameObject stump, trunk, leaves;

    public Villager villager;

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

    public override bool Interact(Item _item)
    {
        if (_item.GetComponent<Axe>() && trunk.activeSelf)
        {
            Chop();
            return true;
        }
        else if (_item.GetComponent<Shovel>() && trunk.activeSelf == false)
        {
            Dig();
            return true;
        }
       

        return false;
    }


    public override bool Interact(Player _player)
    {
        if (trunk.activeSelf && notice != null)
        {
            //only take down the notice if on the side of the tree facing the camera
            if (notice.activeSelf && _player.transform.position.z < transform.position.z)
            {
                notice.SetActive(false);
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

    public override bool Interact(Villager _villager)
    {
        if (trunk.activeSelf && notice != null && notice.activeSelf == false)
        {
            notice.SetActive(true);

            villager = _villager;

        }
        return true;
    }

    public override void TakedownNotice()
    {
        if (notice != null)
        {
            notice.SetActive(false);

            if (villager != null)
            {
                //TODO: how does the villager listen to this? should this interrupt other actions?

              //  villager.StoryState(VillagerStoryState.inScene);
               // villager.scriptedAction = SceneAction.fliers;

                villager = null;
            }
        }
    }

}
