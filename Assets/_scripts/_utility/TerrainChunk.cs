using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    public Transform items, ground, water,enviroment;
    public WorldLocation worldLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public WorldLocation Location()
    {
        return worldLocation;
    }

    public void Load(bool _load)
    {
        if (ground != null)
        { ground.gameObject.SetActive(_load); }

        if (enviroment != null)
        { enviroment.gameObject.SetActive(_load); }

        if (items != null)
        { items.gameObject.SetActive(_load); }
    }


   
}
