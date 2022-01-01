using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//for tracking  which object to spawn and whether the object should be respawned or not
//e.g.: respawn bugs and check trees to grow from stumps


public class WorldObject : MonoBehaviour
{
    public GameObject prefab;
    public Item worldItem;

    public bool respawn;
    [Range(0,1.0f)]
    public float spawnChance;

    public void OnEnable()
    {
        if (respawn && worldItem == null && prefab != null)
        {
            if (Random.Range(0,100) <= (spawnChance * 100))
            {
                SpawnItem();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SpawnItem()
    {
        GameObject clone = Instantiate(prefab,transform.position,transform.rotation);
        if (clone.GetComponent<Item>() != null)
        {
            worldItem = prefab.GetComponent<Item>();
        }
    }


}
