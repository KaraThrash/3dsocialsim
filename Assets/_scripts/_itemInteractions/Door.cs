using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Item
{
    public GameObject interiorObj,connectedArea,wall;
    public GameManager gameManager;

    public Transform exitObj;// where the player should go when using this door

    public bool interior;
    public bool exit;
    public string _camSetting;

    public override bool Interact(GameManager _gameManager) 
    {
        gameManager = _gameManager;

        if (exit)
        {
            gameManager.ExitArea(interiorObj, connectedArea, _camSetting);

        }
        else
        {
            gameManager.EnterArea(interiorObj, connectedArea, _camSetting);
        }

        return true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (gameManager == null) { gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }

        if (other.GetComponent<Player>() != null && connectedArea != null)
        {

            if (exit)
            {
                gameManager.ExitArea(interiorObj, connectedArea, _camSetting);

            }
            else 
            {
                gameManager.EnterArea(interiorObj, connectedArea, _camSetting);
            }

            //if (exit)
            //{
            //    gameManager.LeaveBuilding();
            //}
            //else
            //{
            //    if (interiorObj != null)
            //    {
            //        gameManager.EnterArea(interiorObj, connectedArea, _camSetting);

            //    }
            //    else
            //    {
            //        gameManager.EnterRoom(connectedArea);
            //    }

            //}

        }
    }

    public void SetWallMat(Material _wallMat)
    {
        //interior doors are open voids, exterior doors open/animate
        if (!interior) { return; }

        //if this door leads somewhere hide the wall
        if (connectedArea != null)
        {
            if (wall != null)
            {
                wall.gameObject.SetActive(false);
            }
        }
        else 
        {
            if (wall != null)
            {
                wall.GetComponent<MeshRenderer>().material = _wallMat;
                wall.gameObject.SetActive(true);
            }
        }
          
        
    }


}
