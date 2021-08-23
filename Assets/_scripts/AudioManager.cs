using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public bool checkForMusicClouds, checkForWindGusts;
    public Transform cloudParent,cloudAudioObject;
    public float masterVolumeModifier, mainVolumeModifier, cloudVolumeModifier; // 0<->1 sound setting for user volume controls
    public float fadeInTime;

    public AudioCloud cloudInWorldSource;
    public AudioSource mainSource, cloudSource; //for non diagetic sound effects
    public AudioSource  worldEffectsSourceA, worldEffectsSourceB; //for  diagetic sound effects
    public AudioSource  effectsSourceA, effectsSourceB; //for NON diagetic sound effects

    private bool worldSourceAlternate, effectSourceAlternate; //for toggling between the two world sources to not cut off a soundclip
    private float fadeTimer=-1;//multiply the cloud volume by this to have control over the main volume while also fading it in

    public float silenceTimer=-1;
    public float greenChance, yellowChance, redChance;

    public float intervalLength, intervalTimer, maxTimeWithoutCloud;
    public float windGustRadius,windGustVolumn;


    public List<AudioClip> green, yellow, red;
    public List<AudioClip> stepsGrass,stepsStone, stepsDirt;
    public List<AudioClip> wind;

    private GroundTypes currentGround;
    private int groundCount;

    public  float movementAngle; //0-360, to change the footfall sounds when changing directions
    public  float degreeToChangeSound = 45.0f;

    public  int step0, step1; //loop the sounds for movement in the same direction and same ground type



    

  




    void Start()
    {
     
        if (cloudInWorldSource != null)
        { 
            cloudInWorldSource.StopPlaying();

        }

    }

    // Update is called once per frame
    void Update()
    {
        
      //  if (fadeTimer != -1) { FadeClip(); }

       // TrackSilenceTime();

    }

   

    //to catch questions. alert and other elements that arent 'moods'
    public AudioClip GetSoundFX(string _clip)
    {
        string clipToLoad = "soundfx/";

        clipToLoad += _clip;


        return Resources.Load<AudioClip>(clipToLoad);

    }

    public void PlaySoundEffect(string _clip)
    {
        AudioSource source = EffectSource();

        source.clip = GetSoundFX(_clip);
        source.Play();

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
                if (checkForMusicClouds && RollToSpawnCloud() == false && wind != null && wind.Count > 0)
                {
                    if (checkForWindGusts)
                    { SpawnWindGust(); }
                }
                intervalTimer = 0;
            }


        }
     


    }

    public void SpawnWindGust()
    {
        PlayWorldEffect(wind[(int)Random.Range(0, wind.Count)], GameManager.instance.player.transform.position + new Vector3(Random.Range(-windGustRadius, windGustRadius), 0, Random.Range(-windGustRadius,windGustRadius)), windGustVolumn);
    }

    public bool RollToSpawnCloud()
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
            return true;
        }
        return false;
    }



    public AudioSource EffectSource()
    {
        effectSourceAlternate = !effectSourceAlternate;
        if (effectSourceAlternate)
        {
            return effectsSourceB;
        }
        return effectsSourceA;
    }

    public AudioSource WorldEffectSource()
    {
        worldSourceAlternate = !worldSourceAlternate;
        if (worldSourceAlternate)
        {
            return worldEffectsSourceB;
        }
        return worldEffectsSourceA;
    }


    public void PlayWorldEffect(AudioClip _clip)
    {
        AudioSource source = WorldEffectSource();

        SetSourceClip(source, _clip);
        SetSourceVolume(source, 1);

        fadeTimer = -1;
        source.loop = true;
        source.Play();
        source.loop = false;
    }

    public void PlayWorldEffect(AudioClip _clip,Vector3 _pos)
    {
        AudioSource source = WorldEffectSource();

        SetSourceClip(source, _clip);
        SetSourceVolume(source, 1);

        fadeTimer = -1;
        source.loop = true;
        source.Play();
        source.loop = false;

        source.transform.position = _pos;
    }

    public void PlayWorldEffect(AudioClip _clip, Vector3 _pos,float _volumn)
    {
        AudioSource source = WorldEffectSource();

        SetSourceClip(source, _clip);
        SetSourceVolume(source, _volumn);

        fadeTimer = -1;
        source.loop = true;
        source.Play();
        source.loop = false;

        source.transform.position = _pos;
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



    public void PlayFootStep(Transform _actor, AudioSource _source)
    {


        AudioClip clip = null;

        int layerMask = 1 << 8;

        RaycastHit hit;

        // casting against the ground layer
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









    public void SpawnCloudsForDay(int _green,int _yellow,int _red)
    { 
        

    }

}
