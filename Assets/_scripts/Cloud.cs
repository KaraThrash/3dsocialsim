using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed, leftEdge, rightEdge, maxy, miny, maxz, minz, randomVarienceRange;

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
        Float();
    }

    public void Float()
    {
        transform.Translate(new Vector3(1, 0, 0) * speed * 0.02f * Time.deltaTime);

        if (transform.localPosition.x < leftEdge)
        {
            transform.localPosition = new Vector3(rightEdge, transform.localPosition.y, transform.localPosition.z);
        }
    }
}