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

    public void TrailPlayer(Villager _villager)
    {
        Vector3 screenPoint = GameManager.instance.cam.GetComponent<Camera>().WorldToViewportPoint(_villager.GetNavMeshDestination());
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
        }

            _villager.SetNavMeshDestination(player.position);


        float angle = Vector3.Angle((_villager.GetNavMeshNextPosition() - _villager.transform.position), _villager.transform.forward);
        
        screenPoint = GameManager.instance.cam.GetComponent<Camera>().WorldToViewportPoint(_villager.transform.position);

     
            if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
            {
                _villager.SetNavMeshSpeed(0);

       
                if (Quaternion.Angle(_villager.head.rotation, _villager.animatedHead.rotation) > 30)
                {
                    RotateToFace(_villager.transform, player);

                }

              }
            else 
            {
                    _villager.SetNavMeshSpeed(1);

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
