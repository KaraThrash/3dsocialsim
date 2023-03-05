using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// HOW THE DOORS WORK
/// 
/// The door should have another door it is 'connected to' and an 'exitobj' for where the player should exit
/// 
/// When interacting with a door the gamemanager calls the event init to start transitioning to the new location
/// the new map chunk is loaded, the screen blacked out
/// then the player is teleported, their animator reset and the old chunk is unloaded
/// 
///  "Door" is the term used for any transitional space even if there is no physical representation
/// </summary>
/// 
public class Door : Item
{
    public Door connectedDoor;
    public GameObject interiorObj,connectedArea,wall;

    public GameManager gameManager;

    public Transform exitObj;// where the player should go when using this door

    public bool interior;
    public bool exit;

    public string _camSetting;

    public override bool Interact(Player _player ) 
    {

        GM().EnterDoor(this);


        return true;
    }

    public override void TriggerEnter(Collider other)
    {
       

        if (other.GetComponent<Player>() != null && connectedArea != null)
        {
            GM().EnterDoor(this);
            return;

            if (exit)
            {
                GM().ExitArea(interiorObj, connectedArea, _camSetting);

            }
            else 
            {
                GM().EnterArea(interiorObj, connectedArea, _camSetting);
            }

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

    public Vector3 ExitPosition()
    {
        

        if (connectedDoor != null)
        {
            if (connectedDoor.exitObj != null)
            {
                return connectedDoor.exitObj.position;
            }
            return connectedDoor.transform.position;
        }

        if (exitObj != null) { return exitObj.position; }

        return transform.position;
    }

    public Quaternion ExitRotation()
    {


        if (connectedDoor != null)
        {
            if (connectedDoor.exitObj != null)
            {
                return connectedDoor.exitObj.rotation;
            }
            return connectedDoor.transform.rotation;
        }

        if (exitObj != null) { return exitObj.rotation; }

        return transform.rotation;
    }



    public WorldLocation ExitLocation()
    {
        if (connectedDoor != null) { return connectedDoor.Location(); }

        return WorldLocation.none;
    }

}
