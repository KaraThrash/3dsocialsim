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
    public List<AudioClip> stepsGrass,stepsStone,activeStepList;

    private GroundTypes currentGround;
    private int groundCount;


    public void PlayFootStep(AudioSource _source, GroundTypes _groundType)
    {


        AudioClip clip = null; 

      

            groundCount = 0;

            if (_groundType == GroundTypes.grass)
            {
                if (stepsGrass.Count > 0)
                {
                    if (groundCount >= stepsGrass.Count)
                    { groundCount = 0; }
                    clip = stepsGrass[groundCount];

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
