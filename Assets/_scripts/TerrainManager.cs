using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GameManager gameManager;
    public Transform mapParent;

    public GameObject holePrefab,stumpPrefab,butterfly;
    public List<GameObject> trees;
    public Dictionary<Vector3, TerrainSquare> map;

    void Start()
    {
        map = new Dictionary<Vector3, TerrainSquare>();
        //random roll to spawn trees to get a sense of the world layout/interations. Final version would have a saved state
        // RandomizeTrees(mapParent);
        ContructDictionary(mapParent);
    }

    void Update()
    {
        
    }

    public TerrainSquare GetMapSquare(Vector3 _square)
    {
        Vector3 squarePos = new Vector3(Mathf.RoundToInt(_square.x), 0, Mathf.RoundToInt(_square.z));
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
        Debug.Log("fish 0");
        if (terrainSquare == null) { return false; }
        Debug.Log("fish 1");
        if (terrainSquare.SquareStatus().Equals("water") )
        {
            Debug.Log("fish 2");
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

    public GameObject Catch(Vector3 _square)
    {

        TerrainSquare terrainSquare = GetMapSquare(_square);
        if (terrainSquare == null) { return null; }

        if (terrainSquare.HasBug())
        {
          
            return terrainSquare.CatchBug();

        }
      

        return null;
    }


    public void ContructDictionary(Transform _mapParent)
    {
        foreach (Transform el in _mapParent)
        {
            Transform el2 = el;
            if (el.GetComponent<TerrainSquare>() != null)
            {
                el.GetComponent<TerrainSquare>().TerrainManager(GetComponent<TerrainManager>());
                map.Add(new Vector3(Mathf.FloorToInt(el.position.x), Mathf.CeilToInt(el.position.y), Mathf.FloorToInt(el.position.z)), el.GetComponent<TerrainSquare>());

      
            }
            else
            {
                if (el.childCount > 0) { ContructDictionary(el); }
            }
        }
    }

    public void RandomizeTrees(Transform _mapParent)
    {
        foreach (Transform el in _mapParent)
        {
            if (el.GetComponent<TerrainSquare>() != null)
            {
                el.GetComponent<TerrainSquare>().TerrainManager(GetComponent<TerrainManager>());
                map.Add(new Vector3(Mathf.FloorToInt(el.position.x), Mathf.CeilToInt(el.position.y), Mathf.FloorToInt(el.position.z)), el.GetComponent<TerrainSquare>());
                if (el.GetComponent<TerrainSquare>().SquareStatus().Equals("default"))
                {
                    float rnd = Random.Range(0, 300);
                    if (rnd < 5 && trees.Count > 1) { el.GetComponent<TerrainSquare>().PlantTree(Instantiate(trees[0], el.position, el.rotation)); }
                    else if ((rnd > 50 && rnd < 60) && trees.Count >= 2) { el.GetComponent<TerrainSquare>().PlantTree(Instantiate(trees[1], el.position, el.rotation)); }
                    else if ((rnd > 100 && rnd < 110) && trees.Count >= 3) { el.GetComponent<TerrainSquare>().PlantTree(Instantiate(trees[2], el.position, el.rotation)); }
                    else if ((rnd > 160 && rnd < 170) && trees.Count >= 3) { el.GetComponent<TerrainSquare>().PlantTree(Instantiate(trees[3], el.position, el.rotation)); }
                    else if ((rnd > 210 && rnd < 220))
                    {

                        el.GetComponent<TerrainSquare>().PlaceBug(Instantiate(butterfly, el.position, el.rotation));
                    }
                }
            }
            else 
            {
                if (el.childCount > 0) { RandomizeTrees(el); }
            }
        }
    }



}
