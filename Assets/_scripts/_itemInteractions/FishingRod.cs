using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : Item
{

    public int cycle = 2;
    private int counter = 0;

    private float speed = 0.1f;

    public float  currentFishChange; //the increment that a success on the fish roll increases the current tracking [out of 100]
    private float catchPercent = 15; 
    private float decreasePercent = 5,startPercentRange = 25.0f; // start% range, for how much the meter can be filled to start

    private Vector3 startPos, varience = new Vector3(0,0.1f,0);

    public override bool Use(RaycastHit _hit)
    {
        if (_hit.transform.tag.Equals("water"))
        {
            //subItem.transform.position = new Vector3(_hit.point.x, _hit.transform.position.y, _hit.point.z);
            subItem.transform.position = _hit.point;
            startPos = _hit.point;
            on = true;
            currentFishChange = Random.Range(0, startPercentRange);
            CheckForFish();

            return true;
        }
        

        return false;
    }


    public override void IsOn()
    {
        subItem.transform.position = Vector3.MoveTowards(subItem.transform.position, startPos + varience,Time.deltaTime * speed);

        if (subItem.transform.position == startPos + varience)
        {
            counter++;
            if (counter >= cycle)
            {
                CheckForFish();
                counter = 0;
            }
           
            varience *= -1;
        }


    }



    //check location to determine the option choices for fish to catch
    public void CheckForFish()
    {
        if (varience.y < 0)
        {
            if (Random.Range(-10, 10) < 0)
            {
                currentFishChange -= decreasePercent;
            }
            else
            {
                currentFishChange += catchPercent;
            }
        }
        else
        {
            if (Random.Range(-10, 10) < 0)
            {
                currentFishChange -= 1;
            }
            else
            {
                currentFishChange += catchPercent;
            }
        }

        currentFishChange = Mathf.Clamp(currentFishChange, 0, 100);



        string indicator = "";

        if (currentFishChange >= 90)
        {

            currentFishChange -= 1;
            indicator = "!";

        }
        else 
        {

            if (currentFishChange >= 50)
            {

                indicator = "?";

            }
            else if (currentFishChange < 10)
            {

                indicator = "zzZ";

            }
            else 
            {
                indicator = "...";
            }

        }

        GameManager.instance.UiManager().PlaceWorldText(indicator, startPos, 2);

    }



}
