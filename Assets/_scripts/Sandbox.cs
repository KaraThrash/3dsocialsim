using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sandbox : MonoBehaviour
{
    public GameObject obj1;
    public Transform newParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
