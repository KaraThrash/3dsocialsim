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
                cam.camOffset = Vector3.Lerp(cam.camOffset,camOffset,changeSpeed * Time.deltaTime * ((radiusToStartChanging - Vector3.Distance(cam.transform.position,transform.position)) / radiusToStartChanging) );
                cam.cameraAngle = Mathf.Lerp(cam.cameraAngle, camAngle, changeSpeed * Time.deltaTime * ((radiusToStartChanging - Vector3.Distance(cam.transform.position,transform.position)) / radiusToStartChanging) );
            }
        }


    }
}
