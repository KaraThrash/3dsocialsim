using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostWoods : MonoBehaviour
{
    //moving between locations rearrange everything except these
    //since they would be the only two visible
    public LostWoodsArea currentArea, previousArea;
    public int current,previous;
    public Vector3 areaOffset; // for placing squares adjacent to each other
    public Transform sections,forestLayouts;

    public string pathTaken, targetPath0, targetpath1;


    // Start is called before the first frame update
    void Start()
    {
        RandomizeAreas();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void EnterNewArea(int _area,Collider other)
    {
        previous = current;
        current = _area;

        LostWoodsArea prev = null;
        LostWoodsArea cur = null;

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
                   
                }

            }
        }

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
