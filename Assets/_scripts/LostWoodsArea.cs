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

            lostWoods.EnterNewArea(id, other);

        }
    }
}
