


public enum GameState {free,transitioning,singleCamera,multiCamera }
public enum CameraState {outside,low,high,inside,showoff,conversation,focusing,lostwoods }

public enum PlayerState { playerControlled, inMenu, talking, choosing, fishing, acting, showing }

//world location for game state checks
public enum WorldLocation { overworldNorth,overWorldSouth, inside, lostwoods, bus  }

//maplocation for orienteering
public enum MapLocation { townhall,constructionSite, townSquare,playerHouse, voiceInWall, lostwoodsSouthEntrance,lostwoodsNorthEntrance,northRoadTurn, southRoadTurn, townEntrance }

public enum VillagerState { moving, idle, waiting, talking, activity }
public enum VillagerStoryState { idle, inScene, inPrison, offScreen }
public enum Mood { happy, sad, neutral, scared, angry, confused, tired }
public enum MouthPattern { happy, sad, neutral, scared, angry, confused, tired,fast,slow }

public enum SceneAction { none,talk,leadPlayer, trailPlayer,watchPlayer,fliers,walkAndTalk }
