using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int pocketSize;
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

    public void IntializeList()
    { 
        itemsInPockets = new List<Item>();
        string itempath = "items/ax" ;
        itemsInPockets.Add( Resources.Load(itempath) as Item);
        itempath = "items/fishingrod";
        itemsInPockets.Add(Resources.Load(itempath) as Item);
        itempath = "items/shovel";
        itemsInPockets.Add(Resources.Load(itempath) as Item);

    }
}
