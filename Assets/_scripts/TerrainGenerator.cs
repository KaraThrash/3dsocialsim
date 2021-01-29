using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Transform terrainParent;
    public Material grass, dirt, sidewalk, water;
    public GameObject terrain5x5,bridge;
    // Start is called before the first frame update
    void Start()
    {
       // MakeBlankTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeBlankTerrain()
    {
        int row = 0;
        int col = 0;
        while (row < 10)
        {
            col = 0;
            while (col < 10)
            {
                GameObject clone = Instantiate(terrain5x5,transform.position + new Vector3(col * 5,0,row * 5),transform.rotation);
                clone.transform.parent = terrainParent;
                col++;
            }
            row++;
        }
    }

}
