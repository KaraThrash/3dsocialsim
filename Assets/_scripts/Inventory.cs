using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int pocketSize;
    public Sprite emptyslot;
    public Transform inventorySlots;
    public List<Item> itemsInPockets,itemsInStorage;
    // Start is called before the first frame update



    public Item GetFromPockets( int _item)
    {
        if (itemsInPockets == null) { IntializeList(); }
        return GetItem(itemsInPockets, _item);

    }
    public Item GetFromPockets(string _item)
    {
        if (itemsInPockets == null) { IntializeList(); }
        return GetItem(itemsInPockets, _item);

    }

    public Item GetFromStorage(int _item)
    {
        if (itemsInPockets == null) { IntializeList(); }
        return GetItem(itemsInStorage, _item);

    }
    public Item GetFromStorage(string _item)
    {
        if (itemsInPockets == null) { IntializeList(); }
        return GetItem(itemsInStorage, _item);

    }


    public Item GetItem(List<Item> listToSearch , int _item)
    {
        if (listToSearch.Count <= _item) 
        { return null; }

        return listToSearch[_item];
    }

    public Item GetItem(List<Item> listToSearch, string _item)
    {
        if (itemsInPockets == null) { IntializeList(); }
        int count = 0;
        while (listToSearch.Count > count)
        {
            if (listToSearch[count].itemName.Equals(_item))
            { return listToSearch[count]; }
            count++;
        }

        return null;
    }

    public void SetItemInList(List<Item> listToAddTo,Item _item, int _spot)
    {
        if (listToAddTo.Count > _spot)
        {
            listToAddTo[_spot] = _item;
        }
    }



    public bool TryToAddItemToPockets(Item _item)
    {
        //checks that there is space in the pockets, or unfinished stacks.
        //tops off unfinished stacks, if there is leftover the remaining items 'dont fit'

        //return true if a new item slot needs to be taken for the item
        //return false if it doesnt fit or if all of it fits into unfinished stacks

        //if the item does not stack there needs to be an empty inventory spot for it
        //if the item is at max stacks dont check if there is an incomplete stack
        if (_item.maxStackSize == _item.stackSize)
        {
            if (PocketItemsCount() < pocketSize)
            {
                return true; 
            }
        }
        else 
        {
            foreach (Item el in itemsInPockets)
            {
                if (el != null && el.name.Equals(_item.name) && _item.maxStackSize == _item.stackSize)
                {
                    while (el.maxStackSize > el.stackSize )
                    {
                        el.stackSize++;
                        _item.stackSize--;
                        if (_item.stackSize == 0) { SetIconsInInventoryScreen(); return false; }
                    }
                }
            }

            //if unstacked items are still leftover and there is room in the pockets
            if (_item.stackSize > 0 && PocketItemsCount() < pocketSize)
            { return true; }
        }





        return false;
    }

    public void PutItemInPocket(Item _item)
    {
        //replace a null space with an item
        int count = 0;
        while (count < pocketSize)
        {
            if (itemsInPockets[count] == null)
            {
                //found an empty inventory slot, add the item and return
                //itemsInPockets[count] = _item;
                SetItemInList(itemsInPockets, _item, count);
                SetIconsInInventoryScreen();
                return;
            }
            count++;
          
        }
    }







    public int PocketItemsCount()
    {
        if (itemsInPockets == null) { IntializeList(); }
        int count = 0;
        foreach (Item el in itemsInPockets)
        {
            if (el != null )
            {
                count++;
            }
        }
        //Debug.Log("Pocket Item Count: " + count + " Counted but list.count: " + itemsInPockets.Count);
        return count;
    }



    public void SetIconsInInventoryScreen()
    {
        int count = 0;
        while (count < inventorySlots.childCount )
        {
            if (count < itemsInPockets.Count && inventorySlots.GetChild(count).GetComponent<Image>() != null)
            {
               

                if (itemsInPockets[count] != null && itemsInPockets[count].icon != null)
                {
                    inventorySlots.GetChild(count).GetComponent<Image>().sprite = itemsInPockets[count].icon;
                    inventorySlots.GetChild(count).GetComponent<Image>().color = Color.black;

                }
                else 
                {
                    SetIconToEmpty(inventorySlots.GetChild(count).GetComponent<Image>());
                }


            }
            else 
            {

                SetIconToEmpty(inventorySlots.GetChild(count).GetComponent<Image>());
            }

            count++;
        }
    
    }

    public void SetIconToEmpty(Image _img)
    {
        _img.sprite = emptyslot;
        _img.color = Color.blue;
    }


    public void IntializeList()
    { 
        itemsInPockets = new List<Item>() ;
        string itempath = "items/axe" ;
        itemsInPockets.Add(( Resources.Load(itempath) as GameObject).GetComponent<Item>());
        itempath = "items/fishingRod";
        itemsInPockets.Add((Resources.Load(itempath) as GameObject).GetComponent<Item>());
        itempath = "items/shovel";
        itemsInPockets.Add((Resources.Load(itempath) as GameObject).GetComponent<Item>());
        itempath = "items/net";
        itemsInPockets.Add((Resources.Load(itempath) as GameObject).GetComponent<Item>());
        while (itemsInPockets.Count < pocketSize)
        { itemsInPockets.Add(null); }
        SetIconsInInventoryScreen();
    }
}
