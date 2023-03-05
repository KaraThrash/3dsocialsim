using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item fishPrefab, fishPrefab2;

    //for when there isnt a node found fall back to the default
    public Item genericFish, genericGrub;

    public YarnProgram dialogue; //the flavor text for catching bugs/fish

    // Start is
    // called before
    // the first
    // frame update
    private void Start()
    {
        if (dialogue != null)
        {
            GameManager.instance.dialogueRunner.Add(dialogue);
        }
    }

    // Update is
    // called once
    // per frame
    private void Update()
    {
    }

    public Item CatchFish(Vector3 _pos, float _chance)
    {
        //TODO: ask narrative about what to do about fish types if any

        MapNode node = GameManager.instance.LocationManager().FindClosestMapNode(_pos);

        Debug.Log("Found Node While Fishing: " + node.NodeID());

        if (_chance < 15)
        {
            //do stuff
            return null;
        }
        else if (node == null)
        {
            //should have caught something but the node wasnt found, so send the generic fish as a fallback
            return genericFish;
        }
        else if (_chance < 50)
        {
            //do stuff
            return node.ItemList().fish;
        }

        return node.ItemList().rareFish;
    }

    public Item GetGrub(Vector3 _pos, float _chance)
    {
        MapNode node = GameManager.instance.LocationManager().FindClosestMapNode(_pos);

        Debug.Log("Found Node: " + node.NodeID());

        if (_chance < 15)
        {
            //do stuff
            return null;
        }
        else if (node == null)
        {
            //should have caught something but the node wasnt found, so send the generic grub as a fallback
            return genericFish;
        }
        else if (_chance < 50)
        {
            //do stuff
            return node.ItemList().grub;
        }

        return node.ItemList().rareGrub;
    }

    public YarnProgram GetDialogue()
    { return dialogue; }
}