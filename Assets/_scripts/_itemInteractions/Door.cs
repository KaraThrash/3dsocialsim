using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Item
{
    public GameObject interiorObj;
    public GameManager gameManager;

    public override void Interact(GameManager _gameManager) 
    {
        gameManager = _gameManager;
        gameManager.EnterBuilding(interiorObj);
    }

    public void OnTriggerExit(Collider other)
    {
        gameManager.LeaveBuilding();
    }

}
