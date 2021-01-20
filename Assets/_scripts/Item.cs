using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName;
    public Image icon;
    public bool usable, placable, holdable;
    public int stackSize = 1, maxStackSize = 1;
    
    public GameObject subItem; //fishing bob
    
    private Vector3 subItemStartPos;



    void OnEnable()
    {
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

}
