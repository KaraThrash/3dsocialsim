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
            //MakeArea();
            //LoadMapFromBlueprint();
            MakeGroundGrid();
        }
        if (clean)
        {
            clean = false;
            CleanArea();
        }

    }


    //testing --- using raycasts to place, to have more dynamic enviroment and not need the grid of squares
    public void MakeGroundGrid()
    {
        int xpos = -20, zpos = -width;

        while (zpos < width)
        {
            xpos = -length;
            while (xpos < length)
            {
                RaycastHit hit;
                if (Physics.Raycast(squareParent.position  + new Vector3(xpos,5, zpos), Vector3.down, out hit, 6.0f))
                {
                    if (hit.transform.tag.Equals("rock"))
                    { Instantiate(cornerfence, hit.point, squareParent.rotation); }
                    

                }
           
                xpos++;
            }
            zpos++;
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
                if ( (_width == 0 && _length == 0 ) || (_length == 0 && _width + 1 == width) || (_width == 0 && _length + 1 == length))
                {
                    //corners
                    clone.GetComponent<MeshRenderer>().material = fencemat;
                    clone.GetComponent<TerrainSquare>().terrainStatus = "fence";

                    clone = Instantiate(cornerfence, new Vector3(squareParent.position.x + _width, squareParent.position.y, squareParent.position.z + _length), squareParent.rotation);
                    clone.transform.parent = objectParent;
                }
                else if (_length == Mathf.RoundToInt(length / 2) || _width == Mathf.RoundToInt(width / 2))
                {
                    clone.GetComponent<MeshRenderer>().material = roadmat;
                    clone.GetComponent<TerrainSquare>().terrainStatus = "road";
                }
                else if ((_width == 0 && _length == 0) || (_length == 0 && _width + 1 == width) || (_width == 0 && _length + 1 == length))
                {
                    //corners
                    clone.GetComponent<MeshRenderer>().material = fencemat;
                    clone.GetComponent<TerrainSquare>().terrainStatus = "fence";

                    clone = Instantiate(cornerfence, new Vector3(squareParent.position.x + _width, squareParent.position.y, squareParent.position.z + _length), squareParent.rotation);
                    clone.transform.parent = objectParent;
                }


                else if (_width == 0 || _width + 1 == width)
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
                    clone.transform.parent = objectParent;
                }
               
                else {
                    float rnd = Random.Range(0, 1.0f);
                    if (rnd > 0.01f + morewater)
                    { 
                        morewater -= 0.1f;
                        if (morewater < 0.1f) { morewater = 0.1f; }
                    }
                    else
                    { 
                       clone.GetComponent<MeshRenderer>().material = watermat; clone.GetComponent<TerrainSquare>().terrainStatus = "water";
                        morewater += 0.2f;
                        if (morewater > 0.75f) { morewater = 0.75f; }

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
    public  void LoadMapFromBlueprint()
    {

        if (Resources.Load<TextAsset>("MapSheet") == null)
        {

            Debug.Log("No MapSheet found");

  
            return;
        }


        // string text = File.ReadAllText("./Resources/DialogueSpreadsheet.txt");
        //Load a text file (Assets/Resources/....)
        string text = Resources.Load<TextAsset>("MapSheet").ToString();
        string[] strValues = text.Split('\n');


        int count = 0;
        while (count < strValues.Length)
        {
            string[] tempstring = strValues[count].Split(',');
            //if this is the dialogue for the specified character add it to the master dictionary
            //each entry should be a minimum of 4 elements: name, type, mood, body
            if (tempstring.Length > 0)
            {



                for (int i = 0; i < tempstring.Length; i++)
                {
                    if (tempstring[i] != "c" && tempstring[i] != "v" && tempstring[i] != "h" && tempstring[i] != "w" && tempstring[i] != "g" && tempstring[i] != "s")
                    { }
                    else { 
                        GameObject clone = Instantiate(square, new Vector3(squareParent.position.x + i, squareParent.position.y, squareParent.position.z - count), squareParent.rotation);
                        clone.transform.parent = squareParent;
                        //make sure no blank lines are mistakenly added due to text formatting 
                        if (tempstring[i] == "g")
                        {
                            clone.GetComponent<MeshRenderer>().material = grassmat[(int)Random.Range(0, grassmat.Count)]; clone.GetComponent<TerrainSquare>().terrainStatus = "default";

                        }
                        else if (tempstring[i].Equals("w"))
                        {
                            clone.GetComponent<MeshRenderer>().material = watermat; clone.GetComponent<TerrainSquare>().terrainStatus = "water";

                        }
                        else if (tempstring[i].Equals("h"))
                        {
                            clone.GetComponent<MeshRenderer>().material = fencemat;
                            clone.GetComponent<TerrainSquare>().terrainStatus = "fence";
                            clone = Instantiate(hortfence, new Vector3(squareParent.position.x + i, squareParent.position.y, squareParent.position.z - count), squareParent.rotation);
                            clone.transform.parent = objectParent;

                        }
                        else if (tempstring[i] == "v")
                        {
                            clone.GetComponent<MeshRenderer>().material = fencemat;
                            clone.GetComponent<TerrainSquare>().terrainStatus = "fence";
                            clone = Instantiate(vertfence, new Vector3(squareParent.position.x + i, squareParent.position.y, squareParent.position.z - count), squareParent.rotation);
                            clone.transform.parent = objectParent;

                        }
                        else if (tempstring[i].Equals("c"))
                        {
                            clone.GetComponent<MeshRenderer>().material = fencemat;
                            clone.GetComponent<TerrainSquare>().terrainStatus = "fence";
                            clone = Instantiate(cornerfence, new Vector3(squareParent.position.x + i, squareParent.position.y, squareParent.position.z - count), squareParent.rotation);
                            clone.transform.parent = objectParent;

                        }
                        else if (tempstring[i].Equals("s"))
                        {
                            clone.GetComponent<MeshRenderer>().material = roadmat;
                            clone.GetComponent<TerrainSquare>().terrainStatus = "road";
                        }
                    }

                }


            }



            count++;

        }


    }


}
