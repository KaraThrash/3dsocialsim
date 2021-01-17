using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GameManager gameManager;
    public Transform mapParent;

    public GameObject holePrefab,stumpPrefab;
    public List<GameObject> trees;
    public Dictionary<Vector3, TerrainSquare> map;

    void Start()
    {
        map = new Dictionary<Vector3, TerrainSquare>();
        foreach (Transform el in mapParent)
        {
            if (el.GetComponent<TerrainSquare>() != null)
            {
                el.GetComponent<TerrainSquare>().TerrainManager(GetComponent<TerrainManager>());
                map.Add(new Vector3(Mathf.FloorToInt(el.position.x), Mathf.CeilToInt(el.position.y), Mathf.FloorToInt(el.position.z)), el.GetComponent<TerrainSquare>());

                float rnd = Random.Range(0,200);
                if (rnd < 5 && trees.Count > 1) { el.GetComponent<TerrainSquare>().PlantTree(Instantiate(trees[0],el.position,el.rotation)); }
                else if (rnd < 10 && trees.Count > 2) { el.GetComponent<TerrainSquare>().PlantTree(Instantiate(trees[1], el.position, el.rotation)); }
                else if (rnd < 20 && trees.Count > 3) { el.GetComponent<TerrainSquare>().PlantTree(Instantiate(trees[2], el.position, el.rotation)); }
            }
        }
    }

    void Update()
    {
        
    }

    public TerrainSquare GetMapSquare(Vector3 _square)
    {
        Vector3 squarePos = new Vector3(Mathf.RoundToInt(_square.x), Mathf.FloorToInt(_square.y), Mathf.RoundToInt(_square.z));
        if (map.ContainsKey(squarePos) == false) { return null; }
        return map[squarePos];
    }


    public bool Dig(Vector3 _square)
    {
 
        TerrainSquare terrainSquare = GetMapSquare(_square);
        if (terrainSquare == null) { return false; }

        if (terrainSquare.CanDig())
        {
            GameObject clone = Instantiate(holePrefab, terrainSquare.transform.position, terrainSquare.transform.rotation);

            terrainSquare.Dig(clone);
            return true;

        }
        else 
        {
            if (terrainSquare.SquareStatus() == "hole")
            {
                terrainSquare.Bury(null);
                return true;

            }
                
        }
       
        return false;
    }

    public bool Fish(Vector3 _square,GameObject _bob)
    {

        TerrainSquare terrainSquare = GetMapSquare(_square);
        if (terrainSquare == null) { return false; }

        if (terrainSquare.SquareStatus() == "water")
        {

           terrainSquare.Fish(_square, _bob);
            return true;

        }

        return false;

    }


    public bool Chop(Vector3 _square)
    {

        TerrainSquare terrainSquare = GetMapSquare(_square);

        if (terrainSquare == null) { return false; }

        if (terrainSquare.SquareStatus() == "tree")
        {

            GameObject clone = Instantiate(stumpPrefab, terrainSquare.transform.position, terrainSquare.transform.rotation);
            terrainSquare.ChopTree(clone);
            return true;

        }
        else
        {
          

        }
        return false;

    }






}
