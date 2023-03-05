using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    public Transform items, ground, water, enviroment;
    public Transform bottomLeft, topRight;
    public WorldLocation worldLocation;

    // Start is
    // called before
    // the first
    // frame update
    private void Start()
    {
    }

    // Update is
    // called once
    // per frame
    private void Update()
    {
    }

    public WorldLocation Location()
    {
        return worldLocation;
    }

    public void Load(bool _load)
    {
        if (ground != null)
        { ground.gameObject.SetActive(_load); }

        if (enviroment != null)
        { enviroment.gameObject.SetActive(_load); }

        if (items != null)
        { items.gameObject.SetActive(_load); }
    }

    public float Width()
    {
        if (bottomLeft != null && topRight != null)
        {
            return Mathf.Abs(bottomLeft.position.x - topRight.position.x);
        }

        return 0;
    }

    public float Height()
    {
        if (bottomLeft != null && topRight != null)
        {
            return Mathf.Abs(bottomLeft.position.z - topRight.position.z);
        }

        return 0;
    }

    public Vector3 BottomLeft()
    {
        if (bottomLeft != null)
        { return bottomLeft.position; }

        return Vector3.zero;
    }

    public Vector3 TopRight()
    {
        if (topRight != null)
        { return topRight.position; }

        return Vector3.zero;
    }
}