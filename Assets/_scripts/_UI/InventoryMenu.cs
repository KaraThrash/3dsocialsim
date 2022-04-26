using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMenu : Menus
{
    public int currentPage, itemsPerPage;
    private int rows = 2, cols = 5;
    


    private List<InventorySlotUI> inventorySlots;
    private List<Item> inventory;
   
    public List<InventorySlotUI> SlotList ()
    {
        if (inventorySlots == null)
        {
            inventorySlots = new List<InventorySlotUI>();

            if (SelectibleElementsParent() != null)
            {
                foreach (Transform el in SelectibleElementsParent())
                {
                    if (el.GetComponent<InventorySlotUI>() != null)
                    {
                        InventorySlotUI newSlot = el.GetComponent<InventorySlotUI>();
                        if (inventorySlots.Count >= cols)
                        {
                            el.transform.localPosition = new Vector2((inventorySlots.Count - cols) * newSlot.Width(),newSlot.Height());
                        }
                        else
                        {
                            el.transform.localPosition = new Vector2(inventorySlots.Count * newSlot.Width(),0);
                        }
                        inventorySlots.Add(newSlot);
                    }
                }
            }

        }


        return inventorySlots;
    }


    private void OnEnable()
    {
        Init();
    }
    public override void Init()
    {
        //disable the highlight, reset the cursor location

        inventory = GameManager.instance.player.inventory.itemsInPockets;

        SetMenuElements();

        // cursorTarget = 0;
        // MoveCursor(0);
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            MoveCursor(-1,0);

        }
        if (Input.GetKeyDown(KeyCode.B))
        {

            MoveCursor(1,0);
        }
    }

    public override void MoveCursor(int _xdir, int _ydir)
    {
        int oldpage = currentPage;
        //wrap around the top row to the bottom row, wrap off the side left to right ascending by count
        cursorTarget += (_ydir * cols);
        cursorTarget += _xdir;

        if (cursorTarget >= itemsPerPage)
        { 
            cursorTarget = 0; currentPage++;
        }
        else if (cursorTarget < 0)
        {
            cursorTarget = itemsPerPage - 1; currentPage--; 
        }

       // currentPage += _xdir;

        if (currentPage < 0 )
        { currentPage = 0 ; }
        else if (currentPage * itemsPerPage > inventory.Count )
        { currentPage = 0; }

      //  if (cursorTarget % selectibleElementsParent.childCount != oldCursorTarget % selectibleElementsParent.childCount)
        if (currentPage != oldpage)
        {
            SetMenuElements();
        }


        
        if (cursorTarget >= 0 && cursorTarget < SlotList().Count)
        { EventSystem.current.SetSelectedGameObject(SlotList()[cursorTarget].slotButton.gameObject); }

    }

    public void  SetMenuElements()
    {
        int count = 0;
        int cap = selectibleElementsParent.childCount;

        while (count < SlotList().Count)
        {
            if (inventory.Count > (currentPage * (rows * cols) ) + count)
            {
                SlotList()[count].SetItem(inventory[(currentPage * (rows * cols)) + count]);
            }
            else { SlotList()[count].SetItem(null); }

            count++;
        }


    }



    public override void SetCursorLocation(Transform _cursor, Vector3 _pos)
    {
        _cursor.position = _pos;
    }

    public override void SetCursorActive(Transform _cursor, bool _enabled)
    {
        _cursor.gameObject.SetActive(_enabled);
    }



}
