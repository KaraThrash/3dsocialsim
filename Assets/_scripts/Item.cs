using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Item : MonoBehaviour
{
    public string itemName; 
    public Sprite icon;
    public bool usable, placable, holdable,buryable;
    public bool on;
    public int stackSize = 1, maxStackSize = 1,footprintWidth=-1,footprintDepth=-1;
    
    public GameObject subItem; //fishing bob type items
    public GameObject notice;
    
    private Vector3 subItemStartPos;


    public UnityEvent conditionalEvent;
    

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
        if (on)
        {
            IsOn();
        }
    }

    public virtual void IsOn()
    { 
    
    }

    public void ResetSubItemPos()
    {
        subItem.transform.localPosition = subItemStartPos;
        on = false;
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
