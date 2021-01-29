using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject chatBox, inventory, pauseMenu;
    public Transform inventorySlots;
    public Image emptySlotIcon;
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

}
