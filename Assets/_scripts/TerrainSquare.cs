using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSquare : MonoBehaviour
{
    public TerrainManager terrainManager;
    public string terrainType, terrainStatus;
    public GameObject holePrefab,currentTerrain,currentObject;
    public Item item;
    public Material dirt,grass,sidewalk,water;
    public Mesh cube,hole;
    public bool walkable;

    void Start()
    {
        if (terrainStatus.Equals("water"))
        { walkable = false; GetComponent<BoxCollider>().size = new Vector3(1,2,1); }
        else { walkable = true; }



        //ResetSquare();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
        // SetTerrainShape("hole");

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
          //  if (currentTerrain != null) { Destroy(currentTerrain); }
        }
    }

    public void ResetSquare()
    {
        if(currentObject != null){ Destroy(currentObject); }
        if(item != null){ Destroy(item.gameObject); }
        if(terrainStatus.Equals("building") == false && terrainStatus.Equals("water") == false) { terrainStatus = "default"; }

    }


    public Item GetItem()
    { return item; }

    public void  SetItem(Item _item)
    {  item = _item; }

    public bool Walkable()
    { return walkable; }

    public void TerrainManager(TerrainManager _terrainManager)
    { terrainManager = _terrainManager; }

    public bool SquareOpen()
    {
        return currentTerrain == null;
    }

    public string SquareStatus()
    {
        return terrainStatus;
    }

    public void Fish(Vector3 _pos, GameObject _bob)
    {
        _bob.transform.position = new Vector3(_pos.x,transform.position.y + 0.5f,_pos.z) +( _bob.transform.parent.forward * 0.5f);
    
    }





    public void PlantTree(GameObject _tree)
    {
        //_tree.transform.parent = this.transform;
        _tree.transform.position = transform.position;
        _tree.transform.Rotate(0,Random.Range(-15.0f,15.0f),0);
        currentTerrain = _tree;
        currentObject = _tree;
        terrainStatus = "tree";

    }

    public void ChopTree(GameObject _stump)
    {
        Destroy(currentObject); 
      //  _stump.transform.parent = this.transform;
        _stump.transform.position = transform.position;
        currentTerrain = _stump;
        currentObject = _stump;
        terrainStatus = "stump";

    }




    public bool CanDig()
    { 
        return (terrainStatus == "stump" || terrainStatus == "buried_item" || terrainStatus == "default");
    }
    public void Dig(GameObject _hole)
    {
        if (terrainStatus == "stump" && currentObject != null)
        {
            Destroy(currentObject);
        }
        Debug.Log("dig square");
        currentTerrain = _hole;
        currentObject = _hole;
        terrainStatus = "hole";
    }

    public void PlaceBug(GameObject _bug)
    {
        _bug.transform.parent = this.transform;
        _bug.transform.position = transform.position;
        currentTerrain = _bug;
        currentObject = _bug;
        terrainStatus = "bug";

    }


    public bool HasBug()
    { return terrainStatus == "bug" && currentObject != null; }

    public GameObject CatchBug()
    {
        if (terrainStatus == "bug" && currentObject != null)
        {
            GameObject _obj = currentObject;
            currentObject = null;
            terrainStatus = "default";
            return _obj;
        }
        return null;
    }




    public void Bury(GameObject _obj)
    {
        if (_obj != null)
        {
            currentObject = _obj;
            terrainStatus = "buried_item";
        }
        else 
        {
            if (currentObject != null && terrainStatus == "hole")
            { Destroy(currentObject); }
            currentObject = null;
            
            currentTerrain = null;
            terrainStatus = "default";
        }
        
    }



    public void SetTerrainShape(string _shape)
    {
        if (_shape == "hole" && hole != null)
        {
            currentTerrain = Instantiate(holePrefab,transform.position,transform.rotation);
           // GetComponent<MeshFilter>().mesh = hole;
           // GetComponent<MeshRenderer>().material = dirt;

        }
        else if (_shape == "grass" && cube != null)
        { 
            GetComponent<MeshFilter>().mesh = cube;
            GetComponent<MeshRenderer>().material = grass;

        }
    }
}
