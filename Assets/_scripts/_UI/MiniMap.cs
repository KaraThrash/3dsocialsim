using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    private Player player;
    public RawImage map ;
    public Transform  playerIcon,scrollableArea;

    public float scrollSpeed;

    private float mapWidth=100.0f, mapHeight = 82.0f;
    private float imageWidth = 1342.0f, imageHeight = 719.0f;

    public bool updateMap = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (updateMap)
        {
            scrollableArea.transform.localPosition = Vector2.Lerp(scrollableArea.transform.localPosition, TranslatedPosition(), scrollSpeed * Time.deltaTime);
            playerIcon.transform.eulerAngles = new Vector3(0, 0, UnwrapAngle(player.transform.rotation.eulerAngles.y));
        }




    }

    public Vector2 TranslatedPosition()
    {
        Vector2 pos = Vector2.zero;
        if (Player().WorldLocation() == WorldLocation.overWorldSouth)
        {
            pos = new Vector2((Player().transform.position.x - 10) / mapWidth, (Player().transform.position.z - 14) / mapHeight);
            pos = new Vector2(pos.x * -imageWidth, -pos.y * imageHeight);
          //  pos = new Vector2(pos.x - 198.4f, pos.y - 236.8f);
        }
        else if (Player().WorldLocation() == WorldLocation.overWorldNorth)
        {
            pos = new Vector2((Player().transform.position.x - 130) / mapWidth, (Player().transform.position.z - 10) / mapHeight);
            pos = new Vector2(pos.x * -imageWidth, -pos.y * imageHeight);
        }

        return pos;
    }


    private float UnwrapAngle(float angle)
    {
       
        if (angle < 0)
        { return angle - 360; }

        return 360 - angle;
    }


    public Player Player()
    {
        if (player == null)
        { player = GameManager.instance.player; }

        return player;
    }

}
