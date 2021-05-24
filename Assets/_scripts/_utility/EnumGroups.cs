


public enum GameState {free,transitioning,singleCamera,multiCamera }
public enum CameraState {outside,low,high,inside,showoff,conversation,focusing,lostwoods }

public enum PlayerState { playerControlled, inMenu, talking, choosing, fishing, acting, showing }
public enum WorldLocation { overworld, inside, lostwoods, bus }

public enum VillagerState { moving, idle, waiting, talking, activity }
public enum VillagerStoryState { idle, inScene, inPrison, offScreen }
public enum Mood { happy, sad, neutral, scared, angry, confused, tired }
public enum SceneAction { none,leadPlayer, trailPlayer,watchPlayer,fliers }
