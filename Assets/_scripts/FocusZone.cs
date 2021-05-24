using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusZone : MonoBehaviour
{
    public Vector3 camOffset;
    public float camAngle;

    public float changeSpeed,radiusToStartChanging;

    public CameraControls cam;
  
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (cam == null) 
            {
                cam = FindObjectOfType<CameraControls>();
            }

            if (cam != null)
            {
                cam.InFocusZone(new Vector3(transform.position.x - cam.player.transform.position.x, camOffset.y,camOffset.z),camAngle,((radiusToStartChanging - Vector3.Distance(cam.transform.position, transform.position)) / radiusToStartChanging));
              
            }
        }


    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (cam == null)
            {
                cam = FindObjectOfType<CameraControls>();
            }

            if (cam != null)
            {
                cam.State(CameraState.outside);

            }
        }


    }

}
