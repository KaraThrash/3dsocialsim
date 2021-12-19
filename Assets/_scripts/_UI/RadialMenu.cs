using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialMenu : Menus
{
    public Inventory referenceInventory;
    public Transform wheel;
    public Text itemNameText;

    public float turnSpeed,turnAmount;
    private float amountToTurn; //set to turnamount when moving the radial wheel

    public int currentPlaceInList,currentPage;
    private List<InventorySlotUI> inventorySlots;


    private List<Item> itemList;


    // Start is called before the first frame update
    void Start()
    {
        IntializeSlots();
        if (referenceInventory != null)
        { SetItemList(referenceInventory.itemsInPockets); }
        else { SetItemList(GameManager.instance.player.inventory.itemsInPockets); }
        
        UpdateMenuItems();
        DoneTurning();

    }

   

    public void IntializeSlots()
    {
        inventorySlots = new List<InventorySlotUI>();
        int count = 0;
        while (count < wheel.childCount)
        { 
            if (wheel.GetChild(count).GetComponent<InventorySlotUI>() != null)
            {
                inventorySlots.Add(wheel.GetChild(count).GetComponent<InventorySlotUI>());
                wheel.GetChild(count).Rotate(0,0,360 - ( count * turnAmount));
                count ++;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (amountToTurn != 0)
        {
            TurnWheel();
        }
        else 
        {
            if (DebugControls.debugEnabled)
            { 
                //KeyboardControls(); 
            }
        }

       
    }

    public void KeyboardControls()
    {
        if (Input.GetKeyDown(KeyCode.P))
        { ChangeSelection(1); }
        if (Input.GetKeyDown(KeyCode.O))
        { ChangeSelection(-1); }

        if (Input.GetKeyDown(KeyCode.L))
        { ChangePage(1); }
        if (Input.GetKeyDown(KeyCode.K))
        { ChangePage(-1); }
    }



    public override Item GetSelection()
    {
        if (currentPage > 0 && currentPage < referenceInventory.itemsInPockets.Count)
        { return referenceInventory.itemsInPockets[currentPage]; }


        return null;
    }
    public override void SlotButtonPressed(InventorySlotUI _slot)
    {
        //if this is not an empty slot, select the item and then close the menu
        if (_slot.item != null)
        { 
            GameManager.instance.player.SetHeldItem(_slot.item);
            GameManager.instance.ToggleMenu(Menu.radial);
        }

    }

    public void UpdateMenuItems()
    {
        if (inventorySlots == null) { return; }

        int count = 0;

        while (count < inventorySlots.Count )
        {
            if (count + currentPage  < referenceInventory.itemsInPockets.Count && referenceInventory.itemsInPockets[count + currentPage].stackSize != 0)
            {
                inventorySlots[count].SetItem(referenceInventory.itemsInPockets[count + currentPage ]);

            }
            else { inventorySlots[count].SetItem(null); }

            count++;
        }


    }

    public void ChangeSelection(int _dir)
    {
        if (amountToTurn != 0) { return; }

        currentPlaceInList += _dir;

        if (currentPlaceInList < 0) { currentPlaceInList = inventorySlots.Count - 1; }
        if (currentPlaceInList >= inventorySlots.Count) { currentPlaceInList = 0; }

        // need to rotate the inverse of the inventory element change e.g.: +1 rotate negative
        amountToTurn = turnAmount * _dir;
    }

    public void ChangePage(int _dir)
    {
        currentPage += _dir * inventorySlots.Count;

        if (currentPage < 0) { currentPage = (referenceInventory.itemsInPockets.Count - (referenceInventory.itemsInPockets.Count % inventorySlots.Count) ) ; }
        if (currentPage >= referenceInventory.itemsInPockets.Count) { currentPage = 0; }

       
        currentPlaceInList = 0;


        //if the wheel is mid turn, stop it
        amountToTurn = 0;

        UpdateMenuItems();

        wheel.rotation = transform.rotation;
        DoneTurning();

        if (_dir != 0)
        {
            MenuAnimation();
        }
    }

    public void TurnWheel()
    {
        float turn = Time.deltaTime * turnSpeed;

        if (amountToTurn < 0)
        {
            turn *= -1;
            //to not overshoot the rotation
            if (turn < amountToTurn)
            {
                DoneTurning();
                turn = amountToTurn;
            }

            amountToTurn -= turn;
        }
        else 
        {
            //to not overshoot the rotation
            if (turn > amountToTurn)
            {
                DoneTurning();
                turn = amountToTurn;
            }

            amountToTurn -= turn;
        }
      
        wheel.Rotate(0,0, turn);
    }

    public void DoneTurning()
    {
        if (inventorySlots == null) { return; }

        if (currentPlaceInList >= 0 && currentPlaceInList < inventorySlots.Count)
        {
            EventSystem.current.SetSelectedGameObject(inventorySlots[currentPlaceInList].slotButton.gameObject);

            if (inventorySlots[currentPlaceInList] && inventorySlots[currentPlaceInList].item)
            { itemNameText.text = inventorySlots[currentPlaceInList].item.itemName; }
            else { itemNameText.text = ""; }
            


        }
    
    }


    public void SetItemList(List<Item> _itemList)
    {
        itemList = _itemList;
    }



    public override void Init()
    {
        UpdateMenuItems();
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != inventorySlots[currentPlaceInList].slotButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(inventorySlots[currentPlaceInList].slotButton.gameObject);
        }
        else
        {
          //  EventSystem.current.SetSelectedGameObject(inventorySlots[0].slotButton.gameObject);
            //  EventSystem.current.SetSelectedGameObject(inventorySlots[1].slotButton.gameObject);
        }
    }

    public override void MoveCursor(int _xdir, int _ydir)
    {
        if (_xdir != 0) { ChangeSelection(_xdir); }
        else if (_ydir != 0) { ChangePage(_ydir); }


    }



    public override void SetCursorLocation(Transform _cursor, Vector3 _pos)
    {
      //  _cursor.position = _pos;
    }

    public override void SetCursorActive(Transform _cursor, bool _enabled)
    {
     //   _cursor.gameObject.SetActive(_enabled);
    }

}
