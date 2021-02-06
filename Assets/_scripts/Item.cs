using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName,toolUsable; //what tool can be used on this item if any [axe on trees, shovel on holes]
    public Sprite icon;
    public bool usable, placable, holdable;
    public int stackSize = 1, maxStackSize = 1,footprintWidth=-1,footprintDepth=-1;
    
    public GameObject subItem; //fishing bob type items
    
    private Vector3 subItemStartPos;



    void OnEnable()
    {
        //for items that have a secondary component to them
        if (subItem != null)
        { 
            subItemStartPos = subItem.transform.localPosition;
         }
    }
    void OnDisable()
    {
        if (subItem != null)
        {
            subItem.transform.localPosition = subItemStartPos;
        }
    }

    void Update()
    {
        
    }

    public void ResetSubItemPos()
    {
        subItem.transform.localPosition = subItemStartPos;
    }

    public virtual void Interact(GameManager gameManager) { }

}
