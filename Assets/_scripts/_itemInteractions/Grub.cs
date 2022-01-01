using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grub : Item
{








    public List<AudioClip> sfx;
    public float moveSpeed,moveAcceleration;
    public float rotSpeed,rotAcceleration,angle;
    private float moveSpd,rotSpd;


    public float actLoopLength; //the interval to check its next idle action
    public float noiseLoopLength; //the interval to check to make noise

    private float actTimer,noiseTimer;
    public Vector3 variance; // the area around local zero that the brambles rustle to mimick being walked in
    private Vector3 targetPos;

    protected float movementMagnitude;



    public override void IsOn()
    {
        actTimer -= Time.deltaTime;

        if (actTimer <= 0)
        {
            actTimer = actLoopLength;
            targetPos = subObjectStartPos + new Vector3(Random.Range(-variance.x, variance.x), 0, Random.Range(-variance.z, variance.z));
            moveSpd = 0;
            rotSpd = 0;
        }
        else
        {
            
            rotSpd = Mathf.Lerp(rotSpd,rotSpeed,Time.deltaTime * rotAcceleration);

            Quaternion targetRot = Quaternion.LookRotation(targetPos - SubObject().position);

            SubObject().rotation =  Quaternion.Slerp(SubObject().rotation, targetRot, rotSpd * Time.deltaTime);

            if (Quaternion.Angle(Quaternion.LookRotation(targetPos - SubObject().position), SubObject().rotation) < angle)
            {
                RaycastHit hit;
                
                if (Physics.SphereCast( targetPos + new Vector3(0, 0.5f, 0), 0.2f, Vector3.down, out hit, 2.2f) && hit.transform.tag == "grass")
                {
                    moveSpd = Mathf.Lerp(moveSpd, moveSpeed, Time.deltaTime * moveAcceleration);
                    SubObject().position = Vector3.MoveTowards(SubObject().position, targetPos, moveSpd * Time.deltaTime);

                }

                
            }
            
        }


    }



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
