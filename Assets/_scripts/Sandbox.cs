
using UnityEngine;


public class Sandbox : MonoBehaviour
{
    public GameObject obj1,obj2,obj3;
    public Transform newParent,oldParent;

    public bool on;

    public Vector3 range;
    public Vector3 obj1Target, obj2Target, obj3Target;
    public float speed,rotSpeed;

    // Start is called before the first frame update
    void Start()
    {

        //obj1Target = new Vector3(0, Random.Range(-range.y, range.y), 0);
        //obj2Target = new Vector3(0, Random.Range(-range.y, range.y), 0);
        //obj3Target = new Vector3(0, Random.Range(-range.y, range.y), 0);

    }

    // Update is called once per frame
    void Update()
    {

        //if (on )
        //{
        //    if (obj3 != null)
        //    {
        //        AmbientFlies();
        //    }
        //}

    }



    public void AmbientFlies()
    {
        obj1.transform.localPosition = Vector3.MoveTowards(obj1.transform.localPosition, obj1Target,speed * Time.deltaTime);
        if (Vector3.Distance(obj1.transform.localPosition, obj1Target) == 0)
        {
            obj1Target = new Vector3(0, Random.Range(-range.y, range.y),0);
        }

        obj2.transform.localPosition = Vector3.MoveTowards(obj2.transform.localPosition, obj2Target, speed * Time.deltaTime * 0.3f);
        if (Vector3.Distance(obj2.transform.localPosition, obj2Target) == 0)
        {
            obj2Target = new Vector3(0, Random.Range(-range.y, range.y), 0);
        }

        obj3.transform.localPosition = Vector3.MoveTowards(obj3.transform.localPosition, obj3Target, speed * Time.deltaTime * 0.6f);
        if (Vector3.Distance(obj3.transform.localPosition, obj3Target) == 0)
        {
            obj3Target = new Vector3(0, Random.Range(-range.y, range.y), 0);
        }

        transform.Rotate(0,rotSpeed * Time.deltaTime,0);
    }




    public void BuildFence()
    {
        int count = 0;
        while (count < 50)
        {
            GameObject clone = Instantiate(obj1,obj1.transform.position + new Vector3(0,0,count),obj1.transform.rotation);
            clone.transform.parent = newParent;
            count++;
        }
    }


    public void PopulateMenuSpots()
    {
        int count = 0;
        int count2 = 0;
        while (count < 5)
        {
            count2 = 0;
            while (count2 < 10)
            {
                GameObject clone = Instantiate(obj1, obj1.transform.position, obj1.transform.rotation);
                clone.transform.parent = newParent;

                clone.transform.localPosition = new Vector3(30 * count2, 30 * count, 0);
                count2++;
            }
            count++;
        }
    }

}
