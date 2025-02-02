using UnityEngine;

public class SceneObject
{
    public SceneObject()
    { }

    public SceneAction actionType;

    public Vector3 startPos, targetPos;

    public int linesOfDialogue, phase;
    public float distanceIncrement;
    public float primarySpeed;
}

public static class SceneActions
{
    public static Transform player;

    public static float speed = 1, rotSpeed = 1, maxAngle;

    public static Item relevantItem;

    private static bool _onChange = true;

    // private
    // Villager villager;

    public static void HavePlayerFollow(Transform _actor, Vector3 _location, Player _player, float _speed)
    {
        _actor.GetComponent<Villager>().SetNavMeshDestination(_location);

        speed = 5; rotSpeed = 3;

        if (OnCamera(_actor.position))
        {
            _actor.GetComponent<Villager>().SetNavMeshSpeed(_player.GetComponent<Rigidbody>().velocity.magnitude + 1);
        }
        else { _actor.GetComponent<Villager>().SetNavMeshSpeed(0); }
    }

    public static void HavePlayerFollow(Transform _actor, Transform _location, Player _player, float _speed)
    {
        _actor.GetComponent<Villager>().SetNavMeshDestination(_location.position);

        speed = 5; rotSpeed = 3;

        if (OnCamera(_actor.position))
        {
            _actor.GetComponent<Villager>().SetNavMeshSpeed(_player.GetComponent<Rigidbody>().velocity.magnitude + 1);
        }
        else { _actor.GetComponent<Villager>().SetNavMeshSpeed(0); }
    }

    public static void LeadPlayer(Transform _actor, Vector3 _location, Player _player, float _speed = 5)
    {
        if (_player.nav != null)
        {
            if (_player.nav.enabled == false)
            { _player.nav.enabled = true; }

            _player.SetNavDestination(_actor.transform.position + (_actor.transform.position - _location).normalized);
            _player.SetNavMeshSpeed(_speed + Vector3.Distance(_player.transform.position, _player.GetNavDestination()));
        }

        // _player.SetNavLeadObject((_actor.position
        // -
        // _actor.forward), _speed);

        // _player.Walk((_actor.position
        // -
        // _actor.forward)
        // -
        // _player.transform.position, _speed);

        // _player.transform.LookAt(_actor);

        _actor.GetComponent<Villager>().SetNavMeshSpeed(_speed);
        _actor.GetComponent<Villager>().SetNavMeshDestination(_location);
        _actor.GetComponent<Villager>().SetAnimatorParameter("speed", _actor.GetComponent<Villager>().GetNavmeshVelocity().magnitude);
    }

    public static bool OnCamera(Vector3 _pos)
    {
        Vector3 screenPoint = GameManager.instance.cam.GetComponent<Camera>().WorldToViewportPoint(_pos);

        if (screenPoint.z > 0 && screenPoint.x > 0.02f && screenPoint.x < 0.98f && screenPoint.y > 0.02f && screenPoint.y < 0.98f)
        { return true; }

        return false;
    }

    public static void TrailPlayer(Transform _actor, Transform _target, float _speed)
    {
        // GetComponent<Renderer>().isVisible

        // Vector3
        // screenPoint
        // = GameManager.instance.cam.GetComponent<Camera>().WorldToViewportPoint(_actor.position);

        if (OnCamera(_actor.position))
        {
            RotateToFace(_actor, _target);
        }
        else
        {
            if (RotateToFace(_actor, _target) < maxAngle)
            {
                _actor.position = Vector3.MoveTowards(_actor.position, _actor.position + _actor.forward, _speed * Time.deltaTime);
            }
        }
    }

    public static void TrailPlayer(Villager _villager, Transform _player, float _speed)
    {
        Vector3 screenPoint = GameManager.instance.cam.GetComponent<Camera>().WorldToViewportPoint(_villager.GetNavMeshDestination());

        _villager.SetNavMeshDestination(_player.position);

        float angle = Vector3.Angle((_villager.GetNavMeshNextPosition() - _villager.transform.position), _villager.transform.forward);

        screenPoint = GameManager.instance.cam.GetComponent<Camera>().WorldToViewportPoint(_villager.transform.position);

        // if
        // (screenPoint.z
        // > 0 &&
        // screenPoint.x
        // > 0 &&
        // screenPoint.x
        // < 1 &&
        // screenPoint.y
        // > 0 &&
        // screenPoint.y
        // < 1)
        if (OnCamera(_villager.transform.position))
        {
            _villager.SetNavMeshSpeed(0);
            _villager.SetAnimatorParameter("walking", false);

            if (_villager.animatedHead != null && Quaternion.Angle(_villager.head.rotation, _villager.animatedHead.rotation) > 30)
            {
                RotateToFace(_villager.transform, _player);
            }
        }
        else
        {
            float newSpeed = Mathf.Lerp(_villager.GetNavMeshSpeed(), 1, Time.deltaTime);
            _villager.SetNavMeshSpeed(newSpeed);
        }
    }

    public static void ReplaceNotice(Villager _villager)
    {
        // if
        // (villager
        // == null)
        // {
        // villager
        // =
        // GetComponent<Villager>(); }

        if (relevantItem == null) { return; }

        if (relevantItem.notice.activeSelf)
        { _villager.Act(); _onChange = true; }
        else
        {
            if (_onChange)
            {
                _villager.SetNavMeshDestination(relevantItem.transform.position + new Vector3(0, 0, -1));
                _villager.SetAnimatorParameter("walking", true);
                _onChange = false;
                _villager.ThoughtBubble(Mood.angry, -1);
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
                    _villager.ThoughtBubble(Mood.angry, 0);
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

    public static float RotateToFace(Transform _actor, Vector3 _facetarget)
    {
        Vector3 targetYCorrected = new Vector3(_facetarget.x, _actor.position.y, _facetarget.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetYCorrected - _actor.position);

        _actor.rotation = Quaternion.Slerp(_actor.rotation, targetRotation, rotSpeed * Time.deltaTime);
        return Vector3.Angle((targetYCorrected - _actor.position), _actor.forward);
    }
}