using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public Transform playerHouseFrontDoor, playerRoomInterior;
    public Transform townhallFrontDoor,townhallInterior, constructionSite;
    public Transform lostWoodsSouthEntrance, lostwoodsNorthEntrance, lostwoodsInterior;
    public Transform townEntrance,voiceInWall;
    public Transform northRoadTurn,southRoadTurn;
    public Transform southeastRoadEnd;

    public List<TerrainChunk> terrainChunks;


    public void UnloadChunks()
    { 
    
    }

    public void UnloadChunks(WorldLocation _activeChunk)
    {
        if (terrainChunks == null) { return; }

        foreach (TerrainChunk el in terrainChunks)
        {
            if (el.Location() != _activeChunk)
            {
                el.Load(false);
            }

        }
    }


    public void LoadTerrain(WorldLocation _activeChunk)
    {
        if (terrainChunks == null) { return; }

        foreach (TerrainChunk el in terrainChunks)
        {
            if (el.Location() == _activeChunk)
            {
                el.Load(true);
            }

        }
    }










    public Transform FindLocation(string _location)
    {
        
        //for getting the location from indicators in a yarn script

        //town
        if (_location.ToLower().Equals("townhall"))
        { return townhallFrontDoor; }
        else if (_location.ToLower().Equals("townhallinterior"))
        { return townhallInterior; }
        else if (_location.ToLower().Equals("townentrance"))
        { return townEntrance; }
        else if (_location.ToLower().Equals("constructionsite"))
        { return constructionSite; }
        else if (_location.ToLower().Equals("voiceinwall"))
        { return voiceInWall; }

        //playerhouse outside
        else if (_location.ToLower().Equals("playerhouse"))
        { return playerHouseFrontDoor; }
        else if (_location.ToLower().Equals("playerhouseoutside"))
        { return playerHouseFrontDoor; }

        //playerhouse inside
        else if (_location.ToLower().Equals("playerhouseinside"))
        { return playerRoomInterior; }
        else if (_location.ToLower().Equals("playerroom"))
        { return playerRoomInterior; }

        //lostwoods
        else if (_location.ToLower().Equals("lostwoodssouth"))
        { return lostWoodsSouthEntrance; }
        else if (_location.ToLower().Equals("lostwoodsnorth"))
        { return lostwoodsNorthEntrance; }
        else if (_location.ToLower().Equals("lostwoods"))
        { return lostWoodsSouthEntrance; }

        else if (_location.ToLower().Equals("southroadturn"))
        { return southRoadTurn; }
        else if (_location.ToLower().Equals("northroadturn"))
        { return northRoadTurn; }
        else if (_location.ToLower().Equals("southeastroadend"))
        { return southeastRoadEnd; }


        //if nothing matches send the player to their house
        return playerRoomInterior;
    }


    public Transform FindLocation(MapLocation _location)
    {

        //for getting the location from indicators in a yarn script

        //town
        if (_location == MapLocation.townhall)
        { return townhallFrontDoor; }
        else if (_location == MapLocation.townEntrance)
        { return townEntrance; }
        else if (_location == MapLocation.constructionSite)
        { return constructionSite; }
        else if (_location == MapLocation.voiceInWall)
        { return voiceInWall; }

        //playerhouse outside
        else if (_location == MapLocation.playerHouse)
        { return playerHouseFrontDoor; }

        //lostwoods
        else if (_location == MapLocation.lostwoodsSouthEntrance)
        { return lostWoodsSouthEntrance; }
        else if (_location == MapLocation.lostwoodsNorthEntrance)
        { return lostwoodsNorthEntrance; }

        else if (_location == MapLocation.northRoadTurn)
        { return northRoadTurn; }
        else if (_location == MapLocation.southRoadTurn)
        { return southRoadTurn; }
        else if (_location == MapLocation.southeastRoadEnd)
        { return southeastRoadEnd; }

        //if nothing matches send the player to their house
        return playerRoomInterior;
    }


    public MapNode FindClosestMapNode(Vector3 _location)
    {
        MapNode[] nodes = FindObjectsOfType<MapNode>();

        if (nodes != null && nodes.Length > 0)
        {
            MapNode closestNode = nodes[0];


            foreach (MapNode el in nodes)
            {
                if (Vector3.Distance(el.transform.position, _location) < Vector3.Distance(closestNode.transform.position, _location))
                {
                    closestNode = el;
                }
            }
            return closestNode;
        }
  
        return null;
    }


}
