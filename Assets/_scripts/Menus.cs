using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public int cursorTarget, selectedTarget; //where in the ui the cursors should be
    public Transform cursor, selectionHighlight;
    public Transform selectibleElementsParent;

    void Start()
    {
        //disable the highlight, reset the cursor location
        SetCursorActive(cursor, true);
        SetCursorActive(selectionHighlight, false);

        cursorTarget = 0;
        MoveCursor(0);
    }

    void Update()
    {
        
    }

    public void Init()
    {
        //disable the highlight, reset the cursor location
        SetCursorActive(cursor,true);
        SetCursorActive(selectionHighlight, false);

       // cursorTarget = 0;
       // MoveCursor(0);
    }

    public void MoveCursor(int _dir)
    {
        //wrap around the top row to the bottom row, wrap off the side left to right ascending by count
        cursorTarget += _dir;
        if (cursorTarget >= selectibleElementsParent.childCount)
        { cursorTarget = cursorTarget % selectibleElementsParent.childCount; }
        if (cursorTarget < 0)
        { cursorTarget = cursorTarget + selectibleElementsParent.childCount; }

        SetCursorLocation(cursor, selectibleElementsParent.GetChild(cursorTarget).position);

    }



    public void SetCursorLocation(Transform _cursor, Vector3 _pos)
    {
        _cursor.position = _pos;
    }

    public void SetCursorActive(Transform _cursor, bool _enabled)
    {
        _cursor.gameObject.SetActive(_enabled);
    }


}
