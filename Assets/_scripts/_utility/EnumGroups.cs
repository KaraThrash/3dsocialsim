


using System;

public enum GameState {free,transitioning,singleCamera,multiCamera }
public enum CameraState { basic, outside, low,high,inside,showoff,conversation,focusing,zoomIn, lostwoods,custom }

public enum PlayerState { playerControlled, inMenu, talking, choosing, fishing, acting, showing, inScene }

//world location for game state checks
public enum WorldLocation { overworldNorth,overWorldSouth, inside, lostwoods, bus  }

//maplocation for orienteering
public enum MapLocation { townhall,constructionSite, townSquare,playerHouse, voiceInWall, lostwoodsSouthEntrance,lostwoodsNorthEntrance,northRoadTurn, southRoadTurn, townEntrance, southeastRoadEnd }

public enum VillagerState { moving, idle, waiting, talking, activity }
public enum VillagerStoryState { idle, inScene, inPrison, offScreen }
public enum Mood { happy, sad, neutral, scared, angry, confused, tired }
public enum MouthPattern { happy, sad, neutral, scared, angry, confused, tired,fast,slow }
public enum GroundTypes { grass,wood,stone,leaves,snow,squish}

public enum SceneAction { none,talk,leadPlayer, trailPlayer,watchPlayer,fliers,walkAndTalk,walkingToCheckpoint, holdingAnimation }


public static class EnumGroups 
{

    public static Mood MoodFromString(string _pattern)
    {
        foreach (Mood el in (Mood[])Enum.GetValues(typeof(Mood)))
        {
            if (el.ToString().Equals(_pattern))
            { return el; }
        }
       

        return Mood.neutral;
    }

    public static VillagerState VillagerStateFromString(string _pattern)
    {
        foreach (VillagerState el in (VillagerState[])Enum.GetValues(typeof(VillagerState)))
        {
            if (el.ToString().Equals(_pattern))
            { return el; }
        }


        return VillagerState.idle;
    }


    public static VillagerStoryState VillagerStoryStateFromString(string _pattern)
    {
        foreach (VillagerStoryState el in (VillagerStoryState[])Enum.GetValues(typeof(VillagerStoryState)))
        {
            if (el.ToString().Equals(_pattern))
            { return el; }
        }


        return VillagerStoryState.idle;
    }









    public static MouthPattern ConvertMouthPattern(string _pattern)
    {
        if (_pattern.Equals("happy"))
        { return MouthPattern.happy; }
        else if (_pattern.Equals("sad"))
        { return MouthPattern.sad; }
        else if (_pattern.Equals("neutral"))
        { return MouthPattern.neutral; }
        else if (_pattern.Equals("scared"))
        { return MouthPattern.scared; }
        else if (_pattern.Equals("angry"))
        { return MouthPattern.angry; }

        else if (_pattern.Equals("confused"))
        { return MouthPattern.confused; }
        else if (_pattern.Equals("tired"))
        { return MouthPattern.tired; }
        else if (_pattern.Equals("fast"))
        { return MouthPattern.fast; }
        else if (_pattern.Equals("slow"))
        { return MouthPattern.slow; }


        return MouthPattern.neutral;
    }



}