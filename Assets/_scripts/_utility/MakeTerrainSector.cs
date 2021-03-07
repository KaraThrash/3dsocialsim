using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTerrainSector : MonoBehaviour
{
    public bool on,clean;
    public int width, length;
    public Material  roadmat,watermat,fencemat;
    public List<Material> grassmat;
    public GameObject square, hortfence, vertfence, cornerfence;

    public Transform squareParent, objectParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            on = false;
            MakeArea();
        }
        if (clean)
        {
            clean = false;
            CleanArea();
        }

    }

    public void CleanArea()
    {
        foreach (Transform el in squareParent)
        { Destroy(el.gameObject); }
        foreach (Transform el in objectParent)
        { Destroy(el.gameObject); }
    }

    public void MakeArea()
    {
        int _length = 0, _width = 0, morewaterlength=-1,waterCount = 0;
        float morewater ;
        while (_length < length)
        {
            _width = 0;

            while (_width < width)
            {
                waterCount = 0;
                morewater = 0;
                if (_width == morewater) { morewater += 0.35f; }
                GameObject clone = Instantiate(square, new Vector3(squareParent.position.x + _width, squareParent.position.y, squareParent.position.z + _length), squareParent.rotation);
                clone.transform.parent = squareParent;

                if (_width == 0 || _width + 1 == width)
                {
                    
                    clone.GetComponent<MeshRenderer>().material = fencemat;
                    clone.GetComponent<TerrainSquare>().terrainStatus = "fence";

                    clone = Instantiate(vertfence, new Vector3(squareParent.position.x + _width, squareParent.position.y, squareParent.position.z + _length), squareParent.rotation);
                    clone.transform.parent = objectParent;
                }
                else if (_length == 0 || _length + 1 == length)
                {
                    clone.GetComponent<MeshRenderer>().material = fencemat;
                    clone.GetComponent<TerrainSquare>().terrainStatus = "fence";
                    clone = Instantiate(hortfence, new Vector3(squareParent.position.x + _width, squareParent.position.y, squareParent.position.z + _length), squareParent.rotation);
                    clone.transform.parent = objectParent;
                }
                else if (_length == Mathf.RoundToInt(length / 2) || _width == Mathf.RoundToInt(width / 2))
                {
                    clone.GetComponent<MeshRenderer>().material = roadmat;
                    clone.GetComponent<TerrainSquare>().terrainStatus = "road";
                }
                else { 
                    
                    if (Random.Range(0.01f, 1.0f) > 0.1f + morewater)
                    { 
                    clone.GetComponent<MeshRenderer>().material = grassmat[(int)Random.Range(0,grassmat.Count)]; clone.GetComponent<TerrainSquare>().terrainStatus = "default";
                        morewater -= 0.1f;
                        if (morewater < 0) { morewater = 0; }
                    }
                    else
                    { 
                       clone.GetComponent<MeshRenderer>().material = watermat; clone.GetComponent<TerrainSquare>().terrainStatus = "water";
                        morewater += 0.45f;
                        if (morewater > 0.85f) { morewater = 0.85f; }

                        waterCount++;
                        if (morewaterlength == -1) { morewaterlength = _width; }
                        
                    }
                }

                _width++;
            }
            if (waterCount == 0) { morewaterlength = -1; }
            _length++;
        }
    
    }


}
