using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject chatBox, inventory, pauseMenu;
    public Transform inventorySlots,inventoryCursor,inventorySelectionHighlight;
    public Image emptySlotIcon;
    public Sprite emptyslot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMenu(string _menu,bool _open)
    {
        if (_menu.Equals("inventory")) 
        {
            inventory.SetActive(_open);
           
        }
        else if (_menu.Equals("pause")) 
        {
        
        }

        
    }

    public bool OpenMenu(string _menu)
    {
        if (_menu.Equals("inventory"))
        {
            inventory.SetActive(!inventory.activeSelf);
            return inventory.activeSelf;
        }
        else if (_menu.Equals("pause"))
        {

        }


        return false;
    }



    public void SetIconsInInventoryScreen(List<Item> _itemList)
    {
        int count = 0;
        while (count < inventorySlots.childCount)
        {
            if (count < _itemList.Count && inventorySlots.GetChild(count).GetComponent<Image>() != null)
            {


                if (_itemList[count] != null && _itemList[count].icon != null)
                {
                    inventorySlots.GetChild(count).GetComponent<Image>().sprite = _itemList[count].icon;
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








}
