using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public Transform playerHouseFrontDoor, playerRoomInterior;
    public Transform townhallFrontDoor,townhallInterior, constructionSite;
    public Transform lostWoodsSouthEntrance, lostwoodsNorthEntrance, lostwoodsInterior;
    public Transform townEntrance,voiceInWall;


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




        //if nothing matches send the player to their house
        return playerRoomInterior;
    }

}
