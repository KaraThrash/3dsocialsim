using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostWoodsArea : MonoBehaviour
{
    public LostWoods lostWoods;
    public LostWoodsArea north,south,east,west;
    public int id;
    public Transform treeLayout;

    public void ResetAllConnections()
    {
        north = null;
        south = null;
        east = null;
        west = null;
    
    }

    public void SetConnection(Transform _parent)
    {
        transform.parent = _parent;
    }


    public void BreakConnection(Transform forestLayouts)
    {
        
            transform.parent = forestLayouts;
            transform.position = forestLayouts.position;
            ResetAllConnections();

        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            string direction = "north";
            if (other.transform.position.x < transform.position.x - 2)
            { direction = "east"; }
            else if (other.transform.position.x > transform.position.x + 2)
            { direction = "west"; }
            else if (other.transform.position.z > transform.position.z + 2)
            { direction = "north"; }
            else if (other.transform.position.z < transform.position.z - 2)
            { direction = "south"; }

            lostWoods.EnterNewArea(id, other, direction);

        }
    }
}
