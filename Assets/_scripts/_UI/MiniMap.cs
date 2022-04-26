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
    private float imageWidth = 450, imageHeight = 800.0f;

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

        TerrainChunk chunk = GameManager.instance.LocationManager().GetChunk(Player().WorldLocation());
        
        Vector2 pos = Vector2.zero;

        if (Player().WorldLocation() == WorldLocation.overWorldSouth)
        {
            pos = new Vector2((Player().transform.position.x - 10) / chunk.Width(), (Player().transform.position.z - 14) / chunk.Height());
            pos = new Vector2(pos.x * -imageWidth, -pos.y * (0.5f * imageHeight));
            //pos = new Vector2(pos.x - 100.4f, pos.y - 60.8f);
        }
        else if (Player().WorldLocation() == WorldLocation.overWorldNorth)
        {
            pos = new Vector2((Player().transform.position.x - 150) / chunk.Width(), (Player().transform.position.z - 14) / chunk.Height());
            pos = new Vector2(pos.x * -imageWidth, -pos.y * (0.5f * imageHeight));
            pos = new Vector2(pos.x + 130.4f, pos.y + (0.5f * imageHeight));
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
