using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptedScene")]
public class ScriptableScene : ScriptableObject
{
    //transforms if the scene is in a specific location
    public Transform startLocation, endLocation;
    public List<Transform> locations;

    //Vector3 if the scene is in am arbitrary location
    public Vector3 startPos, endPos;
    public List<Vector3> positions;





    public void StartPosition(Vector3 _pos)
    { startPos = _pos; }

    public Vector3 StartPosition()
    { return startPos; }
    public void EndPosition(Vector3 _pos)
    { endPos = _pos; }

    public Vector3 EndPosition()
    { return endPos; }

    public void AddPosition(Vector3 _pos)
    {
        if (positions == null || positions.Count == 0)
        { positions = new List<Vector3>(); }

        positions.Add(_pos);

    }



    public void StartLocation(Transform _pos)
    { startLocation = _pos; }

    public Transform StartLocation()
    { return startLocation; }
    public void EndLocation(Transform _pos)
    { endLocation = _pos; }

    public Transform EndLocation()
    { return endLocation; }

    public void AddLocation(Transform _pos)
    {
        if (locations == null || locations.Count == 0)
        { locations = new List<Transform>(); }

        locations.Add(_pos);

    }

}
