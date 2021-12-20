using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{
    public bool on;
    public int rows, columns;
    public Transform terrainParent,enviromentpieces,interactablepieces;
    public Material grass, dirt, sidewalk, water;
    public GameObject terrain5x5,tile2,sidewalkTile,waterTile,bridgeempty, empty;
    // Start is called before the first frame update
    void Start()
    {
       // MakeBlankTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {

            FillTerrain();
               // MakeBlankTerrain();
            on = false;
        }
    }




    public void FillTerrain()
    {
        GameObject cloneparent = Instantiate(empty, transform.position, transform.rotation);
        int col = 0;
        int row = 0;
        while (row < rows)
        {
            col = 0;
            while (col < columns)
            {
                if (col < 6 || col > columns - 5 || row < 6 || row > rows - 5)
                {
   
                }
                else if (col == 20 || row == 5 || row == 40)
                {
                   
                }
                else if (col % 2 == 0)
                {
                    if (Random.Range(0, 1.0f) < 0.05f)
                    {
                        GameObject clone = Instantiate(enviromentpieces.transform.GetChild((int)Random.Range(0, enviromentpieces.transform.childCount)).gameObject, transform.position + new Vector3(col, 0, row), transform.rotation);
                        clone.transform.parent = cloneparent.GetComponent<TerrainChunk>().enviroment;
                        clone.transform.Rotate(0, Random.Range(-15, 15), 0);
                    }
                   
                }
                else
                {
                    if (Random.Range(0, 1.0f) < 0.05f)
                    {
                        GameObject clone = Instantiate(interactablepieces.transform.GetChild((int)Random.Range(0, interactablepieces.transform.childCount)).gameObject, transform.position + new Vector3(col, 0, row), transform.rotation);
                        clone.transform.parent = cloneparent.GetComponent<TerrainChunk>().items;
                        clone.transform.Rotate(0, Random.Range(-15, 15), 0);
                    }
                }

                col++;
            }
            row++;
        }
    }


    public void MakeBlankTerrain()
    {
        GameObject cloneparent = Instantiate(empty, transform.position , transform.rotation) ;
        int col = 0;
        int row = 0;
        while (row < rows)
        {
            col = 0;
            while (col < columns)
            {
                if (col < 6 || col > columns - 5 || row < 6 || row > rows - 5)
                {
                    GameObject clone = Instantiate(waterTile, transform.position + new Vector3(col, 0, row), transform.rotation);
                    clone.transform.parent = cloneparent.transform;
                }
                else if (col == 20 || row == 5 || row == 40)
                {
                    GameObject clone = Instantiate(sidewalkTile, transform.position + new Vector3(col, 0, row), transform.rotation);
                    clone.transform.parent = cloneparent.transform;
                }
                else if (col % 2 == 0)
                {
                    GameObject clone = Instantiate(terrain5x5, transform.position + new Vector3(col, 0, row), transform.rotation);
                    clone.transform.parent = cloneparent.transform;
                }
                else 
                {
                    GameObject clone = Instantiate(tile2, transform.position + new Vector3(col, 0, row), transform.rotation);
                    clone.transform.parent = cloneparent.transform;
                }
                
                col++;
            }
            row++;
        }
    }

}
