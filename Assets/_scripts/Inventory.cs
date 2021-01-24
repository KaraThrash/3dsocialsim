using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private int pocketSize;
    public Sprite emptyslot;
    public Transform inventorySlots;
    private List<Item> itemsInPockets,itemsInStorage;
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

    public int PocketItemsCount()
    {
        if (itemsInPockets == null) { IntializeList(); }
        return itemsInPockets.Count;
    }



    public void SetIconsInInventoryScreen()
    {
        int count = 0;
        while (count < inventorySlots.childCount )
        {
            if (count < itemsInPockets.Count && inventorySlots.GetChild(count).GetComponent<Image>() != null)
            {
               

                if (itemsInPockets[count].icon != null)
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
        while (itemsInPockets.Count < pocketSize)
        { itemsInPockets.Add(null); }
        SetIconsInInventoryScreen();
    }
}
