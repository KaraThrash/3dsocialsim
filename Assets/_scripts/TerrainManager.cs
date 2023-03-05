using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GameManager gameManager;
    public Transform mapParent, treeParent, interiorMapParent;

    public bool inside;

    public GameObject holePrefab, stumpPrefab, butterfly;
    public List<GameObject> trees;
    public Dictionary<Vector3, TerrainSquare> map, interiorMap;

    private void Start()
    {
        map = new Dictionary<Vector3, TerrainSquare>();

        //random roll to spawn trees to get a sense of the world layout/interations. Final version would have a saved state
        // RandomizeTrees(mapParent);

        // ContructDictionary(mapParent);
    }

    private void Update()
    {
    }

    public TerrainSquare GetMapSquare(Vector3 _square)
    {
        if (map == null)
        {
            map = new Dictionary<Vector3, TerrainSquare>();
            ContructDictionary(mapParent);
        }

        Vector3 squarePos = new Vector3(Mathf.RoundToInt(_square.x), 0, Mathf.RoundToInt(_square.z));

        // Debug.Log("getmapsquare:
        // " + squarePos.ToString());
        if (inside)
        {
            //interior map should be set when entering the building
            if (interiorMap == null || interiorMap.ContainsKey(squarePos) == false) { return null; }
            return interiorMap[squarePos];
        }
        else
        {
            if (map == null || map.ContainsKey(squarePos) == false)
            {
                // Debug.Log("NO
                // square
                // "
                // + squarePos.ToString());
                return null;
            }
            return map[squarePos];
        }
    }

    public void EnterBuilding(GameObject _interiorObj, GameObject _connectedArea)
    {
        interiorMap = new Dictionary<Vector3, TerrainSquare>();
        //hide the outside, show the inside
        // mapParent.gameObject.SetActive(false);
        _interiorObj.SetActive(true);
        _connectedArea.SetActive(true);

        interiorMapParent = _interiorObj.transform;

        //make a dictionary for this area to interact with the ground. Interior areas are small so we can do this on enter instead of saving it
        MakeMapOfArea(interiorMapParent);
    }

    public void LeaveBuilding()
    {
    }

    public void RoomChange(GameObject _connectedArea)
    {
        //the interior map contains all rooms, transition through them for camera manipulation
    }

    public void MakeMapOfArea(Transform _insideObj)
    {
        foreach (Transform el in _insideObj)
        {
            if (el.GetComponent<TerrainSquare>() != null)
            {
                el.GetComponent<TerrainSquare>().TerrainManager(GetComponent<TerrainManager>());
                interiorMap.Add(new Vector3(Mathf.FloorToInt(el.position.x), Mathf.CeilToInt(el.position.y), Mathf.FloorToInt(el.position.z)), el.GetComponent<TerrainSquare>());
            }
            else
            {
                if (el.childCount > 0) { MakeMapOfArea(el); }
            }
        }
    }

    public bool Dig(Vector3 _square)
    {
        Instantiate(holePrefab, _square, holePrefab.transform.rotation);
        return true;
        TerrainSquare terrainSquare = GetMapSquare(_square);

        if (terrainSquare == null) { return false; }

        //NOTE: keep a list of buried items and check against the dig location
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

    public void Dig(Player _player, RaycastHit _hit)
    {
        //RaycastHit hit;

        //if (Physics.SphereCast(_player.transform.position + (Vector3.up * 0.5f), 0.2f, _player.transform.TransformDirection(Vector3.forward), out hit, 0.3f))
        //{
        if (_hit.transform.tag == "grass")
        {
            GameObject clone = Instantiate(holePrefab, _hit.point, holePrefab.transform.rotation);
        }
        else if (_hit.transform.GetComponent<Hole>() != null)
        {
            if (_player.heldItem != null && _player.heldItem.GetComponent<Item>() != null && _player.heldItem.GetComponent<Item>().buryable)
            {
                _hit.transform.GetComponent<Hole>().Bury(_player.heldItem.GetComponent<Item>());
            }
        }
        else if (_hit.transform.GetComponent<Tree>() != null && _hit.transform.GetComponent<Tree>().JustStump() && _player.heldItem != null && _player.heldItem.GetComponent<Item>().itemName.Equals("shovel"))
        {
            GameObject clone = Instantiate(holePrefab, _hit.transform.position, holePrefab.transform.rotation);
            Destroy(_hit.transform.gameObject);
        }

        //}
    }

    public bool Fish(Vector3 _square, GameObject _bob)
    {
        TerrainSquare terrainSquare = GetMapSquare(_square);
        Debug.Log("fish 0");
        if (terrainSquare == null) { return false; }
        Debug.Log("fish 1");
        if (terrainSquare.SquareStatus().Equals("water"))
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
                    float rnd = Random.Range(0, 1000);
                    if (rnd > 1 && rnd < 60)
                    {
                        GameObject clone;

                        if (rnd < 5 && trees.Count > 1) { clone = Instantiate(trees[0], el.position, el.rotation); }
                        else if ((rnd > 50 && rnd < 60) && trees.Count >= 2) { clone = Instantiate(trees[1], el.position, el.rotation); }
                        else if ((rnd > 5 && rnd < 20) && trees.Count >= 3) { clone = Instantiate(trees[2], el.position, el.rotation); }
                        else if ((rnd > 20 && rnd < 45) && trees.Count > 3) { clone = Instantiate(trees[3], el.position, el.rotation); }
                        else
                        { clone = Instantiate(trees[0], el.position, el.rotation); }
                        clone.transform.parent = treeParent;
                        el.GetComponent<TerrainSquare>().PlantTree(clone);
                    }
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