using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneObj : MonoBehaviour
{
   




}

public static class SceneActions 
{
    public static Transform player;
    
    public static float speed, rotSpeed, maxAngle;

    public static Item relevantItem;

    private static bool _onChange=true;

    // private Villager villager;




    public static void LeadPlayer(Transform _actor,Transform _location)
    {

        Vector3 screenPoint = GameManager.instance.cam.GetComponent<Camera>().WorldToViewportPoint(_actor.position);

        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            RotateToFace(_actor, player);
        }
        else
        {
            if (RotateToFace(_actor, player) < maxAngle)
            {
                _actor.position = Vector3.MoveTowards(_actor.position, _actor.position + _actor.forward, speed * Time.deltaTime);
            }
        }
    }




    public static void TrailPlayer(Transform _actor)
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



    public static void TrailPlayer(Villager _villager)
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
                _villager.SetAnimatorParameter("walking", false);

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


    public static void ReplaceNotice(Villager _villager)
    {
        // if (villager == null) { villager = GetComponent<Villager>(); }

        if (relevantItem == null) { return; }

        if (relevantItem.notice.activeSelf)
        { _villager.Act(); _onChange = true; }
        else 
        {
            if (_onChange)
            {
                _villager.SetNavMeshDestination(relevantItem.transform.position + new Vector3(0, 0, -1));
                _villager.SetAnimatorParameter("walking",true);
                _onChange = false;
                _villager.ThoughtBubble("angry",-1);
            }

            if (_villager.GetNavMeshDestination() != relevantItem.transform.position + new Vector3(0, 0, -1))
            { _villager.SetNavMeshDestination(relevantItem.transform.position + new Vector3(0, 0, -1)); }

            if (Vector3.Distance(_villager.GetNavMeshDestination(), _villager.transform.position) > 0.2f)
            {
                _villager.SetNavMeshSpeed(_villager.speed);
            }
            else
            {
                _villager.SetAnimatorParameter("walking", false);

                if (RotateToFace(_villager.transform, relevantItem.transform) < 5)
                {
                    _villager.ThoughtBubble("angry", 0);
                    relevantItem.HangNotice();
                }

            }
        
        }

    }








    public static float RotateToFace(Transform _actor, Transform _facetarget)
    {
        Vector3 targetYCorrected = new Vector3(_facetarget.position.x, _actor.position.y, _facetarget.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetYCorrected - _actor.position);

        _actor.rotation = Quaternion.Slerp(_actor.rotation, targetRotation, rotSpeed * Time.deltaTime);
        return Vector3.Angle((targetYCorrected - _actor.position), _actor.forward);

    }


}
