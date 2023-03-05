using UnityEngine;

public class LostWoodsArea : MonoBehaviour
{
    public LostWoods lostWoods;
    public LostWoodsArea north, south, east, west;
    public int id;
    public string specialArea;
    public Transform treeLayout, specialAreas;

    public void ResetAllConnections()
    {
        north = null;
        south = null;
        east = null;
        west = null;
    }

    public void SetConnection(Transform _parent)
    {
        transform.parent = _parent;
    }

    public void BreakConnection(Transform forestLayouts)
    {
        //the exit and entrance are special conditions that shouldnt be included in the infinite loop
        if (specialArea.Equals(""))
        {
            transform.parent = forestLayouts;
            transform.position = forestLayouts.position;
        }
        else
        {
            transform.parent = specialAreas;
            transform.position = specialAreas.position;
        }

        ResetAllConnections();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null || other.tag == "Player")
        {
            string direction = "north";
            if (other.transform.position.x < transform.position.x - 2)
            { direction = "east"; }
            else if (other.transform.position.x > transform.position.x + 2)
            { direction = "west"; }
            else if (other.transform.position.z > transform.position.z + 2)
            { direction = "north"; }
            else if (other.transform.position.z < transform.position.z - 2)
            { direction = "south"; }

            lostWoods.EnterNewArea(id, other, direction);
        }
    }
}