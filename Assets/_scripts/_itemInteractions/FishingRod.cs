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

    private float minRange = 0.5f, MaxRange = 3.5f,incrementToCheck = 0.5f;

    public bool FindWater(float _rng)
    {
        if (_rng >= MaxRange)
        {
            return false;
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position + Vector3.up + (transform.forward * _rng), Vector3.down, out hit, 3.5f))
            {
                Debug.Log(hit.point.ToString());

                if (hit.transform.tag.Equals("water"))
                {
                    subItem.transform.position = hit.point;
                    startPos = hit.point;
                    return true;
                }
            }

            return FindWater(_rng + incrementToCheck);


        }
        
  
    }

    public override bool Use(Player _player)
    {
        //recursively check to the max distance for water, if any is found it places the bob at the point and returns true
        if (FindWater(0))
        {
            on = true;
            currentFishChange = Random.Range(0, startPercentRange);
            CheckForFish();


            _player.SetAnimationParameter(_player.Animator(), "fail", false);
            _player.State(PlayerState.fishing);



            _player.PlayAnimation(_player.Animator(), "start_fish");

            return true;

        }
        else
        {
            _player.SetAnimationParameter(_player.Animator(), "fail", true);
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
