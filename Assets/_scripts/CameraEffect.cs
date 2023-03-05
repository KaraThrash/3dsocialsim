using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public float speed;

    // Start is
    // called before
    // the first
    // frame update
    private void Start()
    {
    }

    // Update is
    // called once
    // per frame
    private void Update()
    {
    }

    public virtual void StartEffect()
    { }

    public virtual void EndEffect()
    { }

    public virtual void Effect(Vector3 _moveDir)
    { }
}