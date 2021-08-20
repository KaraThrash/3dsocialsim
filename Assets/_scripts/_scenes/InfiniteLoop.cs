using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteLoop : MonoBehaviour
{
    public YarnProgram scriptToLoad;
    public float speed, treespeed,sunspeed;
    public Transform sun,physicsProps;
    public Transform bus,roadParent,reserve;
    public Transform road0,road1,road2,road3,road4;

    public int intervalCount,busLinesCount=10;
    public float pushForce,timer, physicsInterval;
    // Start is called before the first frame update
    void Start()
    {
        if (scriptToLoad != null)
        {
            GameManager.instance.dialogueRunner.Add(scriptToLoad);
            GameManager.instance.dialogueRunner.StartDialogue("bus9");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveRoad();

        if (sun != null)
        {
            sun.Rotate(0, sunspeed * Time.deltaTime,0);
        }
    }

    public void MoveRoad()
    {
        roadParent.position -= new Vector3(0,0,speed * Time.deltaTime);

        //road1.GetChild(0).localPosition = Vector3.MoveTowards(road1.GetChild(0).localPosition,new Vector3(road1.GetChild(0).localPosition.x, 0, road1.GetChild(0).localPosition.z),speed * treespeed * Time.deltaTime);
        //road2.GetChild(0).localPosition = Vector3.MoveTowards(road2.GetChild(0).localPosition,new Vector3(road2.GetChild(0).localPosition.x, 0, road2.GetChild(0).localPosition.z),speed * treespeed *  Time.deltaTime);
        //road3.GetChild(0).localPosition = Vector3.MoveTowards(road3.GetChild(0).localPosition,new Vector3(road3.GetChild(0).localPosition.x, 10, road3.GetChild(0).localPosition.z),speed * treespeed *  Time.deltaTime);

        if (roadParent.position.z <= -60)
        {
            Loop();


        }

        timer += Time.deltaTime;
        if (physicsProps != null && timer > physicsInterval)
        {
            intervalCount++;

            timer = 0;
            foreach (Transform el in physicsProps)
            {
                if (el.GetComponent<Rigidbody>() != null)
                {
                    el.GetComponent<Rigidbody>().AddTorque(Vector3.up * pushForce);
                }
            }
            if (intervalCount >= 1)
            {
                intervalCount = 0;

                //string randoScript = "bus" + Random.Range(0, busLinesCount).ToString();
                //Debug.Log("randoScript: >> " + randoScript);
                //if (GameManager.instance.dialogueRunner.IsDialogueRunning == false)
                //{ 
                //        GameManager.instance.dialogueRunner.StartDialogue(randoScript);

                //}
            }

        }

    }

    public void Loop()
    {
        UnParentRoadPieces();

        ReAssignActiveRoad();

        roadParent.position += new Vector3(0,0,60);
      //  road3.GetChild(0).localPosition = new Vector3(road1.GetChild(0).localPosition.x, 25, road1.GetChild(0).localPosition.z);
        ParentRoadPieces();
    }


    public void ReAssignActiveRoad()
    {
        road0.position = reserve.position;
        road0.parent = reserve;

        road0 = road1;
        road1 = road2;
        road2 = road3;
        road3 = road4;

        road4 = reserve.GetChild((int)Random.Range(0,reserve.childCount));
        road4.parent = roadParent;
        road4.localPosition = new Vector3(0,0,180);
    }

    public void UnParentRoadPieces()
    {
        road0.parent = null;
        road1.parent = null;
        road2.parent = null;
        road3.parent = null;
        road4.parent = null;

    }

    public void ParentRoadPieces()
    {
        road0.parent = roadParent;
        road1.parent = roadParent;
        road2.parent = roadParent;
        road3.parent = roadParent;
        road4.parent = roadParent;
    }

}
