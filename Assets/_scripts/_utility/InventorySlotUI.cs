using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{

    public Image picture;
    public Text itemName, itemCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetPicture(Sprite _picture)
    {

        if (picture != null)
        { picture.sprite = _picture; }

    }

    public void SetItem(string _name, string _count)
    {

        if (itemName != null)
        { itemName.text = _name; }
        if (itemCount != null)
        { itemCount.text = _count; }
    }

    public void SetItem(string _name, int _count)
    {

        if (itemName != null)
        { itemName.text = _name; }
        if (itemCount != null)
        { itemCount.text = _count.ToString(); }
    }

    public void SetItem(Item _item)
    {
        if (_item == null)
        {
            itemName.text = "";
            itemCount.text = "";
            SetPicture(null);
        }
        else 
        {
            if (itemName != null)
            { itemName.text = _item.itemName; }
            if (itemCount != null)
            { itemCount.text = _item.stackSize.ToString(); }

            SetPicture(_item.icon);
        }
       
    }

}
