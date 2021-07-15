
using UnityEngine;


public class Sandbox : MonoBehaviour
{
    public GameObject obj1;
    public Transform newParent,oldParent;
    public bool on;
    // Start is called before the first frame update
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
        
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
