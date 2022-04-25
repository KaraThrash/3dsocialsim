using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject chatBox, pauseMenu;

    public Menus activeMenu,inventory,radial;

    public Transform inventorySlots;

    public Image emptySlotIcon;

    public Sprite emptyslot;

    public WorldUi worldUiPrefab;

    private List<WorldUi> worldUiPool;

    public Camera cam;

    public Transform emoteCanvas;

    // Vector3 screenPos = cam.WorldToScreenPoint(target.position);

    public void PlaceBubble(Vector3 _pos, Mood _mood)
    {
        Vector3 screenPos = cam.WorldToScreenPoint(_pos);
        //TODO: use the canvas for emote bubbles instead of trying to make them work inside the rolling world

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDisable()
    {
        if (worldUiPool != null)
        {
            worldUiPool.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    public WorldUi GetWorldUi()
    {
        if (worldUiPool == null)
        {
            worldUiPool = new List<WorldUi>();

        }

        foreach (WorldUi el in worldUiPool)
        { 
            if (el.Available()) { return el; }
        }

        WorldUi newUi = Instantiate(worldUiPrefab, transform.position, transform.rotation);
        worldUiPool.Add(newUi);
        return newUi;
    }

    public void PlaceWorldText(string _text,Vector3 _position, float _lifeTime = 1)
    {
        WorldUi newUi = GetWorldUi();
        newUi.transform.position = _position;
        newUi.SetText(_text,_lifeTime);

    }




    public Item GetSelection()
    {
        if (activeMenu != null)
        {
            return activeMenu.GetSelection();
        }
        return null;
    }



    public void MoveCursor(int _xdir,int _ydir)
    {
        activeMenu.MoveCursor(_xdir,_ydir);

    }



    public bool OpenMenu(Menu _menu)
    {
        if (_menu == Menu.inventory)
        {
            if (activeMenu != null)
            {
                if (activeMenu == inventory)
                {
                    activeMenu.gameObject.SetActive(false);
                    activeMenu = null;
                    return false;
                }
                else 
                {
                    //disable the previous menu
                    activeMenu.gameObject.SetActive(false);

                }
            }

            inventory.gameObject.SetActive(true);
            //reset the inventory to neutral
            inventory.Init();
            activeMenu = inventory;
            return true;
            

        }
        else if (_menu == Menu.radial)
        {
            if (activeMenu != null)
            {
                if (activeMenu == radial)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    activeMenu.gameObject.SetActive(false);
                    activeMenu = null;
                    return false;
                }
                else
                {
                    //disable the previous menu
                    activeMenu.gameObject.SetActive(false);

                }
            }

            radial.gameObject.SetActive(true);
            //reset the inventory to neutral
            radial.Init();
            activeMenu = radial;
            return true;
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
