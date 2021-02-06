using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //


    public Transform floorParent,doorParent,wallParent;

    public Material floorMat,wallFace;
    // Start is called before the first frame update
    void Start()
    {
        SetFloorMat(floorMat);
        SetWallMat(wallFace);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFloorMat(Material _floorMat)
    {
        foreach (Transform el in floorParent)
        {
            if (el.GetComponent<MeshRenderer>() != null)
            {
                el.GetComponent<MeshRenderer>().material = _floorMat;
            }
        }
    }

    public void SetWallMat( Material _wallFace)
    {
   
        foreach (Transform el in wallParent)
        {
            if (el.GetComponent<MeshRenderer>() != null)
            {
                el.GetComponent<MeshRenderer>().material = _wallFace;
            }
        }

        foreach (Transform el in doorParent)
        {
            if (el.GetComponent<Door>() != null)
            {
                el.GetComponent<Door>().SetWallMat(_wallFace) ;
            }
        }
    }

}
