using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    public GameObject cloudPrefab3D,cloudPrefabWorld;
    public Transform cloudParent,cloudAudioObject;
    public float masterVolumeModifier, mainVolumeModifier, cloudVolumeModifier; // 0<->1 sound setting for user volume controls
    public float fadeInTime;

    public AudioCloud cloudInWorldSource;
    public AudioSource mainSource, cloudSource, worldEffectsSource; //for non diagetic sound effects

    private float fadeTimer=-1;//multiply the cloud volume by this to have control over the main volume while also fading it in

    public float silenceTimer=-1;
    public float greenChance, yellowChance, redChance;

    public float intervalLength, intervalTimer, maxTimeWithoutCloud;


    public List<AudioClip> green, yellow, red;
    public List<AudioClip> stepsGrass,stepsStone, stepsDirt, activeStepList;

    private GroundTypes currentGround;
    private int groundCount;

    public  float movementAngle; //0-360, to change the footfall sounds when changing directions
    public  float degreeToChangeSound = 45.0f;

    public  int step0, step1; //loop the sounds for movement in the same direction and same ground type



    public void PlayFootStep(Transform _actor,AudioSource _source)
    {


        AudioClip clip = null;

        //_source.clip = FootSound(_actor);
        //_source.Play();

        // groundCount = 0;

        int layerMask = 1 << 8;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_actor.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.tag.Equals("dirt"))
            { currentGround = GroundTypes.dirt; }
            else if (hit.transform.tag.Equals("stone") || hit.transform.tag.Equals("rock"))
            { currentGround = GroundTypes.stone; }
              else if (hit.transform.tag.Equals("grass"))
            { currentGround = GroundTypes.grass; }
        }


        if (currentGround == GroundTypes.grass)
        {
            if (stepsGrass.Count > 0)
            {
                if (groundCount >= stepsGrass.Count)
                { groundCount = 0; }
                clip = stepsGrass[groundCount];

            }

        }
        else if (currentGround == GroundTypes.stone)
        {
            if (stepsGrass.Count > 0)
            {
                if (groundCount >= stepsStone.Count)
                { groundCount = 0; }
                clip = stepsStone[groundCount];

            }

        }
        else if (currentGround == GroundTypes.dirt)
        {
            if (stepsDirt.Count > 0)
            {
                if (groundCount >= stepsDirt.Count)
                { groundCount = 0; }
                clip = stepsDirt[groundCount];

            }

        }
        else
        {
            if (stepsStone.Count > 0)
            {
                if (groundCount >= stepsStone.Count)
                { groundCount = 0; }
                clip = stepsStone[groundCount];

            }


        }


        groundCount++;



        _source.clip = clip;
        _source.Play();

    }

    public  AudioClip FootSound(Transform _actor)
    {

        float angle = _actor.eulerAngles.y ;

        GroundTypes ground = currentGround;

        // This casts rays only against colliders in layer 8.
        int layerMask = 1 << 8;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_actor.position + Vector3.up, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.tag.Equals("dirt"))
            { ground = GroundTypes.dirt; }
            else if (hit.transform.tag.Equals("stone") || hit.transform.tag.Equals("rock"))
            { ground = GroundTypes.stone; }
        }

        if (currentGround != ground || Mathf.Abs(angle - movementAngle) > degreeToChangeSound)
        {
            currentGround = ground;

            if (Mathf.Abs(angle - movementAngle) > degreeToChangeSound)
            { movementAngle = angle; }

            if (ground == GroundTypes.grass && stepsGrass != null && stepsGrass.Count > 1)
            { SetFootPattern(stepsGrass); }
            else if (ground == GroundTypes.stone && stepsStone != null && stepsStone.Count > 1)
            { SetFootPattern(stepsStone);  }
            else if (ground == GroundTypes.dirt && stepsDirt != null && stepsDirt.Count > 1)
            { SetFootPattern(stepsDirt);  }
        }

        int hold = step0;
        step0 = step1;
        

        if (ground == GroundTypes.grass && stepsGrass != null && stepsGrass.Count > hold)
        {
            step1 = (int)Random.Range(0, stepsGrass.Count);
            return stepsGrass[hold];
        }
        else if (ground == GroundTypes.stone && stepsStone != null && stepsStone.Count > hold)
        { step1 = (int)Random.Range(0, stepsStone.Count); return stepsStone[hold]; }
        else if (ground == GroundTypes.dirt && stepsDirt != null && stepsDirt.Count > hold)
        { step1 = (int)Random.Range(0, stepsDirt.Count); return stepsDirt[hold]; }


        return null;
    }


    public  void SetFootPattern(List<AudioClip> _clips)
    {
        //when stepping on a new material type switch to the appropriate foot step sounds
        step0 = (int)Random.Range(0, _clips.Count);
        step1 = step0 + 1;

        if (step1 > _clips.Count)
        { step1 = 0; }


    }




    void Start()
    {
        maxTimeWithoutCloud = 120.0f;
        intervalLength = 20.0f;
        cloudInWorldSource.StopPlaying();

    }

    // Update is called once per frame
    void Update()
    {
        
      //  if (fadeTimer != -1) { FadeClip(); }

        TrackSilenceTime();

    }

    public void TrackSilenceTime()
    {
        //Keep track of the time that no cloud music has been playing
        if ((cloudInWorldSource != null && cloudInWorldSource.AudioSource().isPlaying == false) )
        {
            silenceTimer += Time.deltaTime;
            intervalTimer += Time.deltaTime;

            if (intervalTimer >= intervalLength)
            {
                RollToSpawnCloud();
                intervalTimer = 0;
            }


        }
     


    }

    public void RollToSpawnCloud()
    {
        float rnd = Random.Range(-maxTimeWithoutCloud, silenceTimer);
        if (rnd >= 0)
        {
            string cloudColor = "yellow";
            rnd = Random.Range(0.0f,1.0f);
            if (rnd <= greenChance) { cloudColor = "green"; }
            else if (rnd >= (yellowChance + greenChance)) { cloudColor = "red"; }

            AudioClip newclip = GetClip(cloudColor);

            cloudInWorldSource.SetType(newclip);
            cloudInWorldSource.StartPlaying();

            cloudInWorldSource.transform.position = GameManager.instance.GetPlayer().transform.position;


            silenceTimer = 0;
        }

    }





    public void SpawnCloud(Vector3 _pos, string _color, int _clip)
    {

        GameObject clone = Instantiate(cloudPrefab3D, _pos, cloudPrefab3D.transform.rotation);
        clone.GetComponent<AudioCloud>().SetType(_color,_clip);

    }

    public void SpawnCloud(Vector3 _pos, string _color)
    {

        GameObject clone = Instantiate(cloudPrefab3D, _pos, cloudPrefab3D.transform.rotation);

        AudioClip newclip = GetClip(_color);

        clone.GetComponent<AudioCloud>().SetType(newclip);



    }


    public void PlayWorldEffect(AudioClip _clip)
    {
        SetSourceClip(worldEffectsSource, _clip);
        SetSourceVolume(worldEffectsSource, 1);

        fadeTimer = -1;
        worldEffectsSource.loop = true;
        worldEffectsSource.Play();
        worldEffectsSource.loop = false;
    }


    public void FadeClip()
    {
        
        fadeTimer += Time.deltaTime;

        SetSourceVolume(cloudSource, cloudVolumeModifier * (fadeTimer / fadeInTime));

        if (fadeTimer >= fadeInTime) { fadeTimer = -1; }
    } 



    public void EnterCloud(string _color, int _clip = 0)
    {

        //dont interrupt something playing
        if (cloudSource.isPlaying == true) { return; }
        SetSourceClip(cloudSource,GetClip(_color, _clip));
        SetSourceVolume(cloudSource,0);

        fadeTimer = 0;

        cloudSource.Play();
    }

    public void Enter3dCloud(string _color, int _clip = 0)
    {
        SetSourceClip(cloudSource, GetClip(_color, _clip));
        SetSourceVolume(cloudSource, 0);

        fadeTimer = 0;

        cloudSource.Play();
    }


    public void Set3dCloud(AudioCloud _cloud,string _color)
    {
        _cloud.AudioSource().Stop();
        _cloud.SetType(_color);
        _cloud.SetType( GetClip(_color));

        //SetSourceClip(_cloud.AudioSource(), GetClip(_color));
        SetSourceVolume(_cloud.AudioSource(), 0);

        fadeTimer = 0;

    }

    public void SetSourceVolume(AudioSource _source,float _volume)
    {
        _source.volume = _volume;
    }

    public void SetSourceClip(AudioSource _source,AudioClip _clip)
    {
        _source.clip = _clip;
    }


    public AudioClip GetClip(string _color,int _clip=0)
    {
        AudioClip clipToPlay = null;

        if (_color.Equals("green") && green != null && green.Count > _clip) 
        {
            clipToPlay = green[_clip];
          //  green.RemoveAt(_clip);
        }
        else if (_color.Equals("yellow") && yellow != null && yellow.Count > _clip)
        {
            clipToPlay = yellow[_clip];
         //   yellow.RemoveAt(_clip);
        }
        else if (_color.Equals("red") && red != null && red.Count > _clip) 
        { 
            clipToPlay = red[_clip];
           // red.RemoveAt(_clip);
        }
        else { 

             if (yellow != null && yellow.Count > _clip) 
            { 
                clipToPlay = yellow[_clip];
              //  yellow.RemoveAt(_clip);
            } 
        }

       

        return clipToPlay;
    }
    //(int)Random.Range(0,green.Count)
    public AudioClip GetClip(string _color)
    {
        AudioClip clipToPlay = null;

        if (_color.Equals("green") && green != null && green.Count > 0)
        {
            int rnd = (int)Random.Range(0, green.Count);
            clipToPlay = green[rnd];
          //  green.RemoveAt(rnd);
        }
        else if (_color.Equals("yellow") && yellow != null && yellow.Count > 0)
        {
            int rnd = (int)Random.Range(0, yellow.Count);
            clipToPlay = yellow[rnd];
          //  yellow.RemoveAt(rnd);
        }
        else if (_color.Equals("red") && red != null && red.Count > 0)
        {
            int rnd = (int)Random.Range(0, red.Count);
            clipToPlay = red[rnd];
           // red.RemoveAt(rnd);
        }
        else
        {
            
            if (yellow != null && yellow.Count > 0)
            {
                clipToPlay = yellow[yellow.Count - 1];
               // yellow.RemoveAt(yellow.Count - 1);
            }
        }



        return clipToPlay;
    }




    public void SpawnCloudsForDay(int _green,int _yellow,int _red)
    { 
        

    }

}
