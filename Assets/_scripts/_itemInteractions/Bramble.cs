using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bramble : Item
{
    public List<AudioClip> rustle;
    public float rustleTime = 0.5f,rustleSpeed = 1;
    private float timer;
    public Vector3 rustleVariance; // the area around local zero that the brambles rustle to mimick being walked in
    private Vector3 currentRustle;
    protected float movementMagnitude;

    public override void IsOn()
    {
        timer += Time.deltaTime;

        if (currentRustle != SubObject().localScale && timer < rustleTime)
        {
            SubObject().localScale = Vector3.Lerp(SubObject().localScale, currentRustle, Time.deltaTime * rustleSpeed * movementMagnitude);
        }
        else 
        {
            if (timer >= rustleTime)
            {
                timer = 0;
                currentRustle = new Vector3(1 - rustleVariance.x, 1 - rustleVariance.y, 1 - rustleVariance.z);
                return;
            }

            timer = 0;
            if (SubObject().localScale.x == 1)
            { currentRustle = new Vector3(1 - rustleVariance.x, 1, 1); }
            else if (SubObject().localScale.y == 1)
            { currentRustle = new Vector3(SubObject().localScale.x, 1 - rustleVariance.y, 1); }
            else if (SubObject().localScale.z == 1)
            { currentRustle = new Vector3(1, SubObject().localScale.y, 1 - rustleVariance.z); }
            else { currentRustle = Vector3.one; }

            if (rustle != null && rustle.Count > 0)
            {
                GameManager.instance.AudioManager().PlayWorldEffect(rustle[rustle.Count - 1],transform.position,movementMagnitude * 0.1f);
                rustle.Insert(0, rustle[rustle.Count - 1]);
                rustle.RemoveAt(rustle.Count - 1);
            }
        }

        
    }

    public override void On(bool _on)
    {
        currentRustle = Vector3.one;
        SubObject().localScale = Vector3.one;
        timer = rustleTime;
        on = _on;

    }

    public override void TriggerEnter(Collider col)
    {
        if (col.transform.GetComponent<Villager>() || col.GetComponent<Player>())
        {
            currentRustle = Vector3.one;
            SubObject().localScale = Vector3.one;
        }
    }

    public override void TriggerStay(Collider col)
    {
        if (col.transform.GetComponent<Villager>() || col.GetComponent<Player>())
        {
            movementMagnitude = col.GetComponent<Rigidbody>().velocity.magnitude;
            IsOn();
        }
    }
}
