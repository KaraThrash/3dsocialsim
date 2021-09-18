using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : Item
{
    private float speed = 0.1f;
    private Vector3 startPos, varience = new Vector3(0,0.1f,0);

    public override bool Use(RaycastHit _hit)
    {
        if (_hit.transform.tag.Equals("water"))
        {
            //subItem.transform.position = new Vector3(_hit.point.x, _hit.transform.position.y, _hit.point.z);
            subItem.transform.position = _hit.point;
            startPos = _hit.point;
            on = true;

            return true;
        }
        

        return false;
    }

    public override void IsOn()
    {
        subItem.transform.position = Vector3.MoveTowards(subItem.transform.position, startPos + varience,Time.deltaTime * speed);

        if (subItem.transform.position == startPos + varience)
        {
            varience *= -1;
        }


    }

}
