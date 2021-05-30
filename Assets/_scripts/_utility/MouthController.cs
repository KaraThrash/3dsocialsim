using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : MonoBehaviour
{
    public SkinnedMeshRenderer mouthRenderer;

    public Material open, sad, angry, closed, neutral;

    public MouthPattern pattern;

    public float speakTime, mouthInterval;//time to hold a mouth frame
    public float mouthTimer,intervalTimer;
    public int count;
    public List<Material> mouths;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) { SetMouthPattern(MouthPattern.angry); }
        if (Input.GetKeyDown(KeyCode.N)) { SetMouthPattern(MouthPattern.sad); }
        if (Input.GetKeyDown(KeyCode.B)) { SetMouthPattern(MouthPattern.happy); }



        if (mouthTimer != -1)
        {

            mouthTimer -= Time.deltaTime;
            intervalTimer -= Time.deltaTime;

            if (mouthTimer <= 0)
            {
                mouthTimer = -1;
            }
            else 
            {
                if (intervalTimer <= 0)
                {
                    MoveMouth();
                    intervalTimer = mouthInterval;
                }
            }

           

        }

    }




    public void MoveMouth()
    {
        count++;
        if (count >= mouths.Count) { count = 0; }

        Material[] mats = new Material[2] ;
        mats[0] = mouthRenderer.materials[0];
        mats[1] = mouths[count];
        mouthRenderer.materials = mats;
        
    }

    public void SetMouthPattern(MouthPattern _pattern)
    {
       

        mouthTimer = speakTime;
        intervalTimer = mouthInterval;

        count = 0;

        if (pattern == _pattern)
        { return; }

        mouths = new List<Material>();
        pattern = _pattern;

        if (_pattern == MouthPattern.angry)
        {
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(angry);
            mouths.Add(closed);
            mouths.Add(angry);
        }else if (_pattern == MouthPattern.sad)
        {
            mouths.Add(sad);
            mouths.Add(closed);
            mouths.Add(sad);
            mouths.Add(closed);
            mouths.Add(sad);
        }
        else 
        {
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(neutral);
            mouths.Add(closed);
        }






    }


}
