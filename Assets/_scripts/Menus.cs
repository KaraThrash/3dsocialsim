using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public int cursorTarget, selectedTarget; //where in the ui the cursors should be
    public Transform cursor, selectionHighlight;
    public Transform selectibleElementsParent;

    private Animator anim;
    private string animatorTrigger = "advance";
    void Start()
    {
        anim = GetComponent<Animator>();

        //disable the highlight, reset the cursor location
        Init();
    }

    void Update()
    {
        
    }

    public virtual void Init()
    {
        //disable the highlight, reset the cursor location
        SetCursorActive(cursor,true);
        SetCursorActive(selectionHighlight, false);

        MoveCursor(0, 0);
    }

    public virtual Item GetSelection()
    {

        return null;
    }
    public virtual void SlotButtonPressed(InventorySlotUI _slot)
    {
       
    }

    public virtual void MoveCursor(int _xdir, int _ydir)
    {
        //wrap around the top row to the bottom row, wrap off the side left to right ascending by count
        cursorTarget += (_xdir + (_ydir * 10) );
        if (cursorTarget >= selectibleElementsParent.childCount)
        { cursorTarget = cursorTarget % selectibleElementsParent.childCount; }
        if (cursorTarget < 0)
        { cursorTarget = cursorTarget + selectibleElementsParent.childCount; }

        SetCursorLocation(cursor, selectibleElementsParent.GetChild(cursorTarget).position);

    }



    public virtual void SetCursorLocation(Transform _cursor, Vector3 _pos)
    {
        _cursor.position = _pos;
    }

    public virtual void SetCursorActive(Transform _cursor, bool _enabled)
    {
        _cursor.gameObject.SetActive(_enabled);
    }


    public void MenuAnimation()
    {
        if (anim == null)
        { anim = GetComponent<Animator>(); }

        if (anim != null)
        { anim.SetTrigger(animatorTrigger); }
    }



}
