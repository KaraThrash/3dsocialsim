using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostWoods : MonoBehaviour
{
    //moving between locations rearrange everything except these
    //since they would be the only two visible
    public LostWoodsArea currentArea, previousArea;
    public AudioCloud audioCloud,cloud0,clouda;
    public int current,previous;
    public Vector3 areaOffset; // for placing squares adjacent to each other
    public Transform sections,forestLayouts;
    public Transform entrance,exit;
    public Transform overWorldNorth, overWorldSouth;
    public Door exitArea;
    public string pathTaken, targetPath0, targetpath1;
    public int distanceToLeave,totalDistanceTraveled,sequence; //how many correct transitions need to happen



    // Start is called before the first frame update
    void Start()
    {
        RandomizeAreas();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetOverWorldExit(bool _north)
    {
        if (_north)
        {
            exitArea.exitObj = overWorldNorth;
            exitArea.connectedArea = overWorldNorth.gameObject;
        }
        else 
        {
            exitArea.connectedArea = overWorldSouth.gameObject;
        }
    }

    public void StartLostWoods()
    {

        foreach (Transform el in forestLayouts)
        {
            if (el.GetComponent<LostWoodsArea>() != null)
            {

                el.GetComponent<LostWoodsArea>().BreakConnection(forestLayouts);

            }
        }
        foreach (Transform el in sections)
        {
            if (el.GetComponent<LostWoodsArea>() != null)
            {

                el.GetComponent<LostWoodsArea>().BreakConnection(forestLayouts);

            }
        }

        LostWoodsArea cur = entrance.GetComponent<LostWoodsArea>();

        cur.transform.parent = sections;
        cur.transform.position = sections.position;


        if (cur.north == null)
        {
            cur.north = forestLayouts.GetChild(0).GetComponent<LostWoodsArea>();
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().south = cur;

            forestLayouts.GetChild(0).position = cur.transform.position + new Vector3(0, 0, 35.0f);
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().SetConnection(sections);

        }
        if (cur.south == null)
        {
            cur.south = forestLayouts.GetChild(0).GetComponent<LostWoodsArea>();
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().north = cur;

            forestLayouts.GetChild(0).position = cur.transform.position + new Vector3(0, 0, -35.0f);
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().SetConnection(sections);


        }
        if (cur.east == null)
        {
            cur.east = forestLayouts.GetChild(0).GetComponent<LostWoodsArea>();
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().west = cur;

            forestLayouts.GetChild(0).position = cur.transform.position + new Vector3(35.0f, 0, 0);
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().SetConnection(sections);


        }
        if (cur.west == null)
        {
            cur.west = forestLayouts.GetChild(0).GetComponent<LostWoodsArea>();
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().east = cur;

            forestLayouts.GetChild(0).position = cur.transform.position + new Vector3(-35.0f, 0, 0);
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().SetConnection(sections);



        }

    }


    public void EnterNewArea(LostWoodsArea _area)
    {
        if (currentArea != null) { previousArea = currentArea; }
        currentArea = _area;

        foreach (Transform el in sections)
        {
            if (el.GetComponent<LostWoodsArea>() != null)
            {
                if (el.GetComponent<LostWoodsArea>() != previousArea && el.GetComponent<LostWoodsArea>() != currentArea)
                {
                    el.gameObject.SetActive(false);
                }
            }
        }


    }

    public void EnterNewArea(int _area,Collider other,string _dir="north")
    {

       


        previous = current;
        current = _area;

        LostWoodsArea prev = null;
        LostWoodsArea cur = null;

        if (audioCloud == cloud0) { audioCloud = clouda; }
        else { audioCloud = cloud0; }

        foreach (Transform el in sections)
        {
            if (el.GetComponent<LostWoodsArea>() != null)
            {

                if (el.GetComponent<LostWoodsArea>().id == previous)
                {
                    prev = el.GetComponent<LostWoodsArea>();
                }
                else if (el.GetComponent<LostWoodsArea>().id == current)
                {
                    cur = el.GetComponent<LostWoodsArea>();
                 //   audioCloud.transform.position = el.position;
                }

            }
        }

        if (prev == null || cur == null) { return; }

        

        //if the escape condition is reached dont undo it
        if (pathTaken.Equals(targetPath0) == false && sequence < distanceToLeave)
        {
            if (targetPath0[sequence].Equals(_dir[0]))
            {
                sequence++;

                if (audioCloud.AudioSource().isPlaying == false || audioCloud.cloudColor.Equals("green") == false)
                {
                   
                      //  GameManager.instance.AudioManager().Set3dCloud(audioCloud, "green");
                    
                }


            }
            else 
            {
                //when going the wrong direction set the clouds to yellow, and then red if still going the wrong way
                //if (audioCloud.AudioSource().isPlaying == false || audioCloud.cloudColor.Equals("green") == false)
                //{
                //    if (audioCloud.AudioSource().isPlaying == false || audioCloud.cloudColor.Equals("yellow") == false)
                //    {
                //       // GameManager.instance.AudioManager().Set3dCloud(audioCloud, "yellow");
                //    }
                //    else 
                //    {
                //     //   GameManager.instance.AudioManager().Set3dCloud(audioCloud, "red");
                //    }
                //}
            }

            if (pathTaken.Length >= distanceToLeave) { pathTaken = pathTaken.Substring(1, pathTaken.Length - 1); }

            pathTaken += _dir[0];
        }



        


        //break all the adjacent connections from the previous area that do not lead to the current one
        //they are added back to the group to reset the new adjacent areas of the endless forest
        if (prev.north != null && prev.north.id != current)
        {
            prev.north.BreakConnection(forestLayouts);
            prev.north = null;

        }
        if (prev.south != null && prev.south.id != current)
        {
            prev.south.BreakConnection(forestLayouts);
            prev.south = null;

        }
        if (prev.east != null && prev.east.id != current)
        {
            prev.east.BreakConnection(forestLayouts);
            prev.east = null;

        }
        if (prev.west != null && prev.west.id != current)
        {
            prev.west.BreakConnection(forestLayouts);
            prev.west = null;
        }

        if (cur.north == null)
        {
            //if condition to exit reached always have the exit on the north
            if (pathTaken.Equals(targetPath0))
            {
                cur.north = exit.GetComponent<LostWoodsArea>();
                exit.GetComponent<LostWoodsArea>().south = cur;

                exit.position = cur.transform.position + new Vector3(0, 0, 35.0f);
                exit.GetComponent<LostWoodsArea>().SetConnection(sections);
            }
            else 
            {
                cur.north = forestLayouts.GetChild(0).GetComponent<LostWoodsArea>();
                forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().south = cur;

                forestLayouts.GetChild(0).position = cur.transform.position + new Vector3(0, 0, 35.0f);
                forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().SetConnection(sections);
            }

            

        }
        if (cur.south == null)
        {
            cur.south = forestLayouts.GetChild(0).GetComponent<LostWoodsArea>();
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().north = cur;

            forestLayouts.GetChild(0).position = cur.transform.position + new Vector3(0, 0, -35.0f);
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().SetConnection(sections);


        }
        if (cur.east == null)
        {
            cur.east = forestLayouts.GetChild(0).GetComponent<LostWoodsArea>();
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().west = cur;

            forestLayouts.GetChild(0).position = cur.transform.position + new Vector3(35.0f, 0, 0);
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().SetConnection(sections);


        }
        if (cur.west == null)
        {
            cur.west = forestLayouts.GetChild(0).GetComponent<LostWoodsArea>();
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().east = cur;

            forestLayouts.GetChild(0).position = cur.transform.position + new Vector3(-35.0f, 0, 0);
            forestLayouts.GetChild(0).GetComponent<LostWoodsArea>().SetConnection(sections);



        }


        //RandomizeAreas();

    }



    //public void EnterNewArea(int _area, Collider other)
    //{
    //    previous = current;
    //    current = _area;

    //    foreach (Transform el in sections)
    //    {
    //        if (el.GetComponent<LostWoodsArea>() != null)
    //        {
    //            if (el.GetComponent<LostWoodsArea>().id != previous && el.GetComponent<LostWoodsArea>().id != current && el.GetComponent<LostWoodsArea>().treeLayout != null)
    //            {
    //                el.GetComponent<LostWoodsArea>().treeLayout.parent = forestLayouts;
    //                el.GetComponent<LostWoodsArea>().treeLayout.position = forestLayouts.position;
    //                el.GetComponent<LostWoodsArea>().treeLayout = null;
    //            }
    //        }
    //    }
    //    RandomizeAreas();

    //}







    public void RandomizeAreas()
    {
        //foreach (Transform el in sections)
        //{
        //    if (el.GetComponent<LostWoodsArea>() != null)
        //    {
        //        if (el.GetComponent<LostWoodsArea>().treeLayout == null)
        //        {
        //            el.GetComponent<LostWoodsArea>().treeLayout = forestLayouts.GetChild(0);
        //            el.GetComponent<LostWoodsArea>().treeLayout.parent = el;
        //            el.GetComponent<LostWoodsArea>().treeLayout.position = el.position;
                    
        //        }
        //    }
        //}
    }

}
