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

    private Villager villager;
    // Start is called before the first frame update
    void Start()
    {
        villager = GetComponent<Villager>();
    }








    public bool isNeutral;
    public float blinkTimer;

    //similar to blinking periodically set the mouth to their mood
    public void IdleMouthTimer()
    {
        blinkTimer -= Time.deltaTime;

        if (blinkTimer <= 0)
        {
            if (isNeutral)
            {
                isNeutral = false;
                SetMouth(villager.CurrentMood());
                

                blinkTimer = UnityEngine.Random.Range(0.2f, 0.5f);

            }
            else
            {
                isNeutral = true;
                SetMouth(Mood.neutral);

                blinkTimer = UnityEngine.Random.Range(1.0f, 6.0f);
            }



        }


    }




    // Update is called once per frame
    void Update()
    {

        if (mouthRenderer == null) { return; }

        if (Input.GetKeyDown(KeyCode.M)) { SetMouthPattern(MouthPattern.angry,12); }
        if (Input.GetKeyDown(KeyCode.N)) { SetMouthPattern(MouthPattern.sad); }
        if (Input.GetKeyDown(KeyCode.B)) { SetMouthPattern(MouthPattern.happy); }



        if (mouthTimer != -1)
        {

            mouthTimer -= Time.deltaTime;
            intervalTimer -= Time.deltaTime;

            if (mouthTimer <= 0)
            {
                mouthTimer = -1;

                SetMouth(mouthRenderer.materials[0], closed);
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
        else 
        {
            if (villager != null)
            {
                IdleMouthTimer();
            }
        }

    }


    public void MoveMouth()
    {
        if (mouthRenderer == null) { return; }

        count++;
        if (count >= mouths.Count) { count = 0; }

        SetMouth(mouthRenderer.materials[0], mouths[count]);

        
    }

    public void SetMouth(Mood _mood)
    {
 

        //for setting the mouth to the mood when not talking
        if (_mood == Mood.neutral)
        { SetMouth(mouthRenderer.materials[0], closed); }
        else if (_mood == Mood.sad)
        { SetMouth(mouthRenderer.materials[0], sad); }
        else if (_mood == Mood.angry)
        { SetMouth(mouthRenderer.materials[0], angry); }
        else if (_mood == Mood.happy)
        { SetMouth(mouthRenderer.materials[0], open); }
    }


    public void SetMouth(Material _head,Material _mouth)
    {
       

        Material[] mats = new Material[2];
        mats[0] = _head;
        mats[1] = _mouth;
        mouthRenderer.materials = mats;

    }

    public void SetMouthPattern(MouthPattern _pattern, float _length = 1)
    {
        if (mouthRenderer == null) { return; }

        if (pattern == _pattern && mouthTimer != -1)
        { return; }

        mouthTimer = _length;
        intervalTimer = mouthInterval + UnityEngine.Random.Range(0.01f, 0.1f);

        count = 0;

      

        mouths = new List<Material>();
        pattern = _pattern;

        if (_pattern == MouthPattern.angry)
        {
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(angry);
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(neutral);
            mouths.Add(closed);
            mouths.Add(angry);
        }else if (_pattern == MouthPattern.sad)
        {
            mouths.Add(sad);
            mouths.Add(closed);
            mouths.Add(sad);
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(neutral);
            mouths.Add(closed);
            mouths.Add(sad);
        }
        else if (_pattern == MouthPattern.neutral)
        {
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(neutral);
            mouths.Add(closed);
            mouths.Add(sad);
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(neutral);
        }
         else
        {
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(neutral);
            mouths.Add(closed);
            mouths.Add(sad);
            mouths.Add(open);
            mouths.Add(closed);
            mouths.Add(neutral);
        }





    }


}
