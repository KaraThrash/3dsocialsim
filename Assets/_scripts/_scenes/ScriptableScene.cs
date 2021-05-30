using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptedScene")]
public class ScriptableScene : ScriptableObject
{

    public Villager primary, secondary;
    public List<Villager> villagers;

    //the enum group map locations to move to and from
    public MapLocation startWorldLocation, endWorldLocation;
    public List<MapLocation> worldLocations;


    //Vector3 if the scene is in an arbitrary location
    //public Vector3 startPos, endPos;
    //public List<Vector3> positions;


    public void StartLocation(MapLocation _pos)
    { startWorldLocation = _pos; }

    public MapLocation StartLocation()
    { return startWorldLocation; }
    public void EndLocation(MapLocation _pos)
    { endWorldLocation = _pos; }

    public MapLocation EndLocation()
    { return endWorldLocation; }

    public void AddLocation(MapLocation _pos)
    {
        if (worldLocations == null || worldLocations.Count == 0)
        { worldLocations = new List<MapLocation>(); }

        worldLocations.Add(_pos);

    }


    //public void StartPosition(Vector3 _pos)
    //{ startPos = _pos; }

    //public Vector3 StartPosition()
    //{ return startPos; }
    //public void EndPosition(Vector3 _pos)
    //{ endPos = _pos; }

    //public Vector3 EndPosition()
    //{ return endPos; }

    //public void AddPosition(Vector3 _pos)
    //{
    //    if (positions == null || positions.Count == 0)
    //    { positions = new List<Vector3>(); }

    //    positions.Add(_pos);

    //}



 

}
