public static class Constants
{
    public static int HOURS_PER_DAY;
    public static int HOURS_PER_NIGHT;

    public static int ACTIONS_PER_DAY = 9;
    public static int ACTIONS_PER_NIGHT = 5;

    //transition time is the length to move from the starting location to the new location before making the game visible again
    public static float CHUNK_TRANSITION_TIME = 1.1f;

    //the time to lock out the player controls as they move from one location to another
    public static float CHUNK_ACTION_TIME = 2.1f;

    public static string ANIM_CLEAR_TO_BLACK = "CameraClearToBlack";
    public static string ANIM_Black_TO_CLEAR = "CameraBlackToClear";
}