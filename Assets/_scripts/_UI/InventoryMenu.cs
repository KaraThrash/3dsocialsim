using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : Menus
{
    public int currentPage, itemsPerPage;
    private List<Item> inventory;
   

    private void OnEnable()
    {
        Init();
    }
    public override void Init()
    {
        //disable the highlight, reset the cursor location

        SetCursorActive(cursor, true);
        SetCursorActive(selectionHighlight, false);
        inventory = GameManager.instance.player.inventory.itemsInPockets;

        SetMenuElements();

        // cursorTarget = 0;
        // MoveCursor(0);
    }

    public override void MoveCursor(int _xdir, int _ydir)
    {
        int oldpage = currentPage;
        //wrap around the top row to the bottom row, wrap off the side left to right ascending by count
        cursorTarget += _ydir;

        if (cursorTarget >= itemsPerPage)
        { 
            cursorTarget = 0; currentPage++;
        }
        else if (cursorTarget < 0)
        {
            cursorTarget = itemsPerPage - 1; currentPage--; 
        }

       // currentPage += _xdir;

        if (currentPage < 0 && selectibleElementsParent.childCount > 0)
        { currentPage = ((inventory.Count ) / itemsPerPage) ; }
        else if (currentPage > ((inventory.Count) / itemsPerPage) && selectibleElementsParent.childCount > 0)
        { currentPage = 0; }

      //  if (cursorTarget % selectibleElementsParent.childCount != oldCursorTarget % selectibleElementsParent.childCount)
        if (currentPage != oldpage)
        {
            SetMenuElements();
        }

        //if (cursorTarget >= inventory.Count)
        //{ cursorTarget = cursorTarget % selectibleElementsParent.childCount; }
        //if (cursorTarget < 0)
        //{ cursorTarget = inventory.Count - 1; }

        SetCursorLocation(cursor, selectibleElementsParent.GetChild(cursorTarget ).position);

    }

    public void  SetMenuElements()
    {
        int count = 0;
        int cap = selectibleElementsParent.childCount;

        while (count < itemsPerPage)
        {
            if (inventory.Count > (currentPage * cap) + count)
            {
                selectibleElementsParent.GetChild(count).GetComponent<InventorySlotUI>().SetItem(inventory[(currentPage * itemsPerPage) + count]);
            }
            else { selectibleElementsParent.GetChild(count).GetComponent<InventorySlotUI>().SetItem(null); }

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
