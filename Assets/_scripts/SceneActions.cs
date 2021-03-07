using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneActions : MonoBehaviour
{
    public Transform player;
    public float speed, rotSpeed,maxAngle;

    public void TrailPlayer(Transform _actor)
    {
      //  GetComponent<Renderer>().isVisible
     
            Vector3 screenPoint = GameManager.instance.cam.GetComponent<Camera>().WorldToViewportPoint(_actor.position);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            RotateToFace(_actor,player);
        }
        else 
        {
            if (RotateToFace(_actor,player) < maxAngle)
            {
                _actor.position = Vector3.MoveTowards(_actor.position, _actor.position + _actor.forward, speed * Time.deltaTime);
            }
        }
    }

    public float RotateToFace(Transform _actor, Transform _facetarget)
    {
        Vector3 targetYCorrected = new Vector3(_facetarget.position.x, _actor.position.y, _facetarget.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetYCorrected - _actor.position);

        _actor.rotation = Quaternion.Slerp(_actor.rotation, targetRotation, rotSpeed * Time.deltaTime);
        return Vector3.Angle((targetYCorrected - _actor.position), _actor.forward);

    }


}
