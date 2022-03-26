using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTerrainSector : MonoBehaviour
{
    public bool on,clean;
    public int width, length;
    public Material  roadmat,watermat,fencemat,grassMaterial;
    public List<Material> grassmat;
    public List<GameObject> trees,otherObjs;
    public GameObject square, hortfence, vertfence, cornerfence;

    public Transform squareParent, objectParent,treeParent;
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
        int xpos = -20, zpos = 0;

        while (zpos < width)
        {
            xpos = 0;
            while (xpos < length)
            {
                RaycastHit hit;
                if (Physics.Raycast(squareParent.position  + new Vector3(xpos,5, zpos), Vector3.down, out hit, 16.0f))
                {
                

                    if (Vector3.Angle(hit.normal, Vector3.up) < 5)
                    {
                        if (hit.transform.tag.Equals("rock"))
                        {
                            //  Instantiate(cornerfence, hit.point, squareParent.rotation); 
                        }
                        else if (hit.transform.tag.Equals("grass"))
                        {
                            float rnd = Random.Range(0, 1000);
                            if (rnd > 1 && rnd < 100)
                            {
                                GameObject clone;

                                //if (rnd < 5 && trees.Count > 1) { clone = Instantiate(trees[0], hit.point,transform.rotation); }
                                //else if ((rnd > 50 && rnd < 60) && trees.Count >= 2) { clone = Instantiate(trees[1], hit.point,transform.rotation); }
                                //else if ((rnd > 5 && rnd < 20) && trees.Count >= 3) { clone = Instantiate(trees[2], hit.point,transform.rotation); }
                                //else if ((rnd > 20 && rnd < 45) && trees.Count > 3) { clone = Instantiate(trees[3], hit.point,transform.rotation); }
                                //else
                                //{ clone = Instantiate(trees[0], hit.point + new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(-0.25f, 0.25f)), transform.rotation); }

                                clone = Instantiate(trees[(int)Random.Range(0, trees.Count)], hit.point + new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(-0.25f, 0.25f)), transform.rotation);

                                //clone.transform.position -= Vector3.up * 0.5f;
                                clone.transform.parent = treeParent;
                                // el.GetComponent<TerrainSquare>().PlantTree(clone);


                            }
                            else if ((rnd > 300 && rnd < 340))
                            {
                                GameObject clone;

                                //if (rnd < 305 && trees.Count > 1) { clone = Instantiate(otherObjs[0], hit.point,transform.rotation); }
                                //else if ((rnd > 305 && rnd < 260) && trees.Count >= 2) { clone = Instantiate(otherObjs[1], hit.point,transform.rotation); }
                                //else if ((rnd > 25 && rnd < 220) && trees.Count >= 3) { clone = Instantiate(otherObjs[2], hit.point,transform.rotation); }
                                //else if ((rnd > 220 && rnd < 245) && trees.Count > 3) { clone = Instantiate(otherObjs[3], hit.point,transform.rotation); }
                                //else
                                //{ clone = Instantiate(otherObjs[0], hit.point + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)),transform.rotation); }

                                clone = Instantiate(otherObjs[(int)Random.Range(0,otherObjs.Count)], hit.point + new Vector3(Random.Range(-0.5f, 0.5f),0,Random.Range(-0.5f, 0.5f)), transform.rotation);

                                clone.transform.position -= Vector3.up * 0.05f;
                                clone.transform.Rotate(0, Random.Range(-45.0f,45.0f), 0);
                              //  clone.transform.Rotate(0, 270, 0);
                                clone.transform.parent = objectParent;

                            }
                        }
                    }
                    

                }
           
                xpos += 3;
            }
            zpos += 2;
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

        //if (Resources.Load<TextAsset>("MapSheet") == null)
        //{

        //    Debug.Log("No MapSheet found");

  
        //    return;
        //}


        //// string text = File.ReadAllText("./Resources/DialogueSpreadsheet.txt");
        ////Load a text file (Assets/Resources/....)
        //string text = Resources.Load<TextAsset>("MapSheet").ToString();
        //string[] strValues = text.Split('\n');


        //int count = 0;
        //while (count < strValues.Length)
        //{
        //    string[] tempstring = strValues[count].Split(',');
        //    //if this is the dialogue for the specified character add it to the master dictionary
        //    //each entry should be a minimum of 4 elements: name, type, mood, body
        //    if (tempstring.Length > 0)
        //    {



        //        for (int i = 0; i < tempstring.Length; i++)
        //        {
        //            if (tempstring[i] != "c" && tempstring[i] != "v" && tempstring[i] != "h" && tempstring[i] != "w" && tempstring[i] != "g" && tempstring[i] != "s")
        //            { }
        //            else { 
        //                GameObject clone = Instantiate(square, new Vector3(squareParent.position.x + i, squareParent.position.y, squareParent.position.z - count), squareParent.rotation);
        //                clone.transform.parent = squareParent;
        //                //make sure no blank lines are mistakenly added due to text formatting 
        //                if (tempstring[i] == "g")
        //                {
        //                    clone.GetComponent<MeshRenderer>().material = grassmat[(int)Random.Range(0, grassmat.Count)]; clone.GetComponent<TerrainSquare>().terrainStatus = "default";

        //                }
        //                else if (tempstring[i].Equals("w"))
        //                {
        //                    clone.GetComponent<MeshRenderer>().material = watermat; clone.GetComponent<TerrainSquare>().terrainStatus = "water";

        //                }
        //                else if (tempstring[i].Equals("h"))
        //                {
        //                    clone.GetComponent<MeshRenderer>().material = fencemat;
        //                    clone.GetComponent<TerrainSquare>().terrainStatus = "fence";
        //                    clone = Instantiate(hortfence, new Vector3(squareParent.position.x + i, squareParent.position.y, squareParent.position.z - count), squareParent.rotation);
        //                    clone.transform.parent = objectParent;

        //                }
        //                else if (tempstring[i] == "v")
        //                {
        //                    clone.GetComponent<MeshRenderer>().material = fencemat;
        //                    clone.GetComponent<TerrainSquare>().terrainStatus = "fence";
        //                    clone = Instantiate(vertfence, new Vector3(squareParent.position.x + i, squareParent.position.y, squareParent.position.z - count), squareParent.rotation);
        //                    clone.transform.parent = objectParent;

        //                }
        //                else if (tempstring[i].Equals("c"))
        //                {
        //                    clone.GetComponent<MeshRenderer>().material = fencemat;
        //                    clone.GetComponent<TerrainSquare>().terrainStatus = "fence";
        //                    clone = Instantiate(cornerfence, new Vector3(squareParent.position.x + i, squareParent.position.y, squareParent.position.z - count), squareParent.rotation);
        //                    clone.transform.parent = objectParent;

        //                }
        //                else if (tempstring[i].Equals("s"))
        //                {
        //                    clone.GetComponent<MeshRenderer>().material = roadmat;
        //                    clone.GetComponent<TerrainSquare>().terrainStatus = "road";
        //                }
        //            }

        //        }


        //    }



        //    count++;

        //}


    }


}
