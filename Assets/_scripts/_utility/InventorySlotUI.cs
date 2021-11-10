using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Menus menu;
    public Button slotButton;
    public Image picture;
    public Sprite itemIcon;
    public Text itemName, itemCount;
    public GameObject itemQuantityBackground;
    public Item item;

    // Start is called before the first frame update
    void Start()
    {
        if (picture == null)
        { picture = GetComponent<Image>(); }
        
        picture.sprite = itemIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonPressed()
    {
        if (menu != null && item != null)
        {
            menu.SlotButtonPressed(this);
        }
    }



    public void SetPicture(Sprite _sprite)
    {

        if (picture == null)
        { return; }

        picture.color = Color.clear;
        itemIcon = _sprite;
        picture.sprite = itemIcon;
        if (_sprite != null)
        {
            
            picture.color = Color.black;
        }
        else
        {
            

        }
        


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
        if (itemQuantityBackground != null)
        {
            itemQuantityBackground.SetActive((_count > 1 ? true : false));
        }

        if (itemName != null)
        { itemName.text = _name; }
        if (itemCount != null)
        {
            if (_count > 1)
            { 
                itemCount.text = _count.ToString();
          
            }
        }
    }

    public void SetItem(Item _item)
    {
        item = _item;

        if (_item == null)
        {
            itemName.text = "";
            itemCount.text = "";
            SetPicture(null);
            if (itemQuantityBackground != null)
            {
                itemQuantityBackground.SetActive(false);
            }
        }
        else 
        {
            if (itemQuantityBackground != null)
            {
                itemQuantityBackground.SetActive((_item.stackSize > 1 ? true : false));
            }

            if (itemName != null)
            { itemName.text = _item.itemName; }
            if (itemCount != null)
            { itemCount.text = _item.stackSize.ToString(); }

            SetPicture(_item.icon);
        }
       
    }

}
