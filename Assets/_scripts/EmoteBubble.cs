using UnityEngine;

public class EmoteBubble : MonoBehaviour
{
    public float displayTime, timer;

    public MeshRenderer renderer;
    public Transform target;
    public Camera cam;
    public Transform emoteCanvas;

    // Start is
    // called before
    // the first
    // frame update
    private void Start()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
    }

    // Update is
    // called once
    // per frame
    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(0, 0, 0, 0), Time.deltaTime);
        transform.LookAt(transform.position - new Vector3(0, 0, -10));

        if (timer != -1)
        {
            //Quaternion.identity;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = -1;
                gameObject.SetActive(false);
            }
        }
    }

    public void SetMaterial(Material _mat, float _duration = 1)
    {
        if (renderer == null)
        { return; }

        renderer.material = _mat;

        timer = _duration;
    }

    public void SetMaterial(Material _mat)
    {
        if (renderer == null)
        { return; }

        renderer.material = _mat;

        timer = displayTime;
    }
}