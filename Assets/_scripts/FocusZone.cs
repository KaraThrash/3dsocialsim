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
               // cam.focusing = true;
              //  cam.InFocusZone(new Vector3(transform.position.x - cam.player.transform.position.x, camOffset.y,camOffset.z),camAngle,((radiusToStartChanging - Vector3.Distance(cam.transform.position, transform.position)) / radiusToStartChanging));
                cam.InFocusZone(transform.position + new Vector3(0,2,(2 + Vector3.Distance(transform.position, cam.player.transform.position)) * -1),15,0.2f);
              
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
                cam.focusing = false;
                //  cam.State(CameraState.outside);

            }
        }


    }

}
