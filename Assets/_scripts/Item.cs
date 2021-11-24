using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Item : MonoBehaviour
{
    public string itemName;
    public ItemClass itemClass;
    public Sprite icon;
    public bool usable, placable, holdable,buryable;
    public bool on;
    public int stackSize = 1, maxStackSize = 1,footprintWidth=-1,footprintDepth=-1;
    
    public GameObject subObject; //fishing bob type items
    public Item subItem; //if other items are fed into this one: e.g. bait used with the fishing rod
    public GameObject notice;
    
    private Vector3 subObjectStartPos;


    public UnityEvent conditionalEvent;
    

    void OnEnable()
    {

        //for items that have a secondary component to them
        if (subObject != null)
        { 
            subObjectStartPos = subObject.transform.localPosition;
         }
    }
    void OnDisable()
    {
        if (subObject != null)
        {
            subObject.transform.localPosition = subObjectStartPos;
        }
    }

    void Update()
    {
        if (on)
        {
            IsOn();
        }
    }

    public virtual void IsOn()
    { 
    
    }

    public void ResetsubObjectPos()
    {
        subObject.transform.localPosition = subObjectStartPos;
        on = false;
    }
    public virtual void SetSubItem(Item _item)
    {
     
    }

    public virtual void UseSubItem()
    {
        if (subItem == null)
        {
            return;
        }


        subItem.stackSize--;
        subItem = null;
    }

    public Transform SubObject()
    {
        if (subObject != null)
        { return subObject.transform; }

        //safety if the subitem wasnt set
        if (transform.childCount > 0 )
        { return transform.GetChild(0); }

        return transform;
    }

    public virtual Item Hold(Player _player)
    {

        return this;
    }

    public virtual bool Use()
    {

        return false;
    }

    public virtual bool Use(RaycastHit _hit)
    {

        return false;
    }

    public virtual bool Use(Player _user)
    {

        return false;
    }


    public virtual bool Use(Player _user, RaycastHit _hit)
    {

        return false;
    }


    /// <summary on the idea for Items>
    /// An 'Item' passes itself to the iteming being interacted with to allow for each 'Item'
    /// to define its own interaction based on context, or none as determined by the object
    /// This way new items that get added only need to define how they are interacted with
    /// 
    /// 
    /// </summary>




    public virtual bool Interact(GameManager gameManager) { return false; }
    public virtual bool Interact(Player _player) { return false; }
    public virtual bool Interact(Villager _villager) { return false; }
    public virtual bool Interact(Item _item) { return false; }
    public virtual bool Interact()
    {

        if (notice != null && notice.activeSelf) { }
        return false;
    }

    public virtual void Dig()
    {

       //note: cant dig here animation should be the default
    }

    public virtual bool Catch()
    {
        return false;

    }

    public virtual bool Catch(Item _item)
    {

        return false;
    }

    public virtual bool TryCatch()
    {

        return false;
    }


    public virtual void HangNotice() 
    {
        if (notice != null)
        {
            notice.SetActive(true);
        }
    }

    public virtual void TakedownNotice()
    {
        if (notice != null)
        {
            notice.SetActive(false);
        }
    }

    public bool CheckForNotice()
    {
        if (notice != null && notice.activeSelf) { return true; }
            return false;
    }

  
}
