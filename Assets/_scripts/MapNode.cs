using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField]
    private LocationItems areaSpecificItems;
    [SerializeField]
    public MapLocation nodeID;

    
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public LocationItems ItemList()
    {
        return areaSpecificItems;
    }


    public void NodeID(MapLocation _newID)
    {
        nodeID = _newID;
    }

    public MapLocation NodeID()
    {
        return nodeID;
    }
}
