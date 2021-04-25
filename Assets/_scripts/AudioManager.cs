using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject cloudPrefab3D,cloudPrefabWorld;
    public Transform cloudParent;
    public float masterVolumeModifier, mainVolumeModifier, cloudVolumeModifier; // 0<->1 sound setting for user volume controls
    public float fadeInTime;

    public AudioSource mainSource, cloudSource, worldEffectsSource; //for non diagetic sound effects

    private float fadeTimer=-1;//multiply the cloud volume by this to have control over the main volume while also fading it in


    public List<AudioClip> green, yellow, red;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (fadeTimer != -1) { FadeClip(); }

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
        if (_color.Equals("green") && green.Count > _clip) { return green[_clip]; }
        if (_color.Equals("yellow") && yellow.Count > _clip) { return yellow[_clip]; }
        if (_color.Equals("red") && red.Count > _clip) { return red[_clip]; }

        return null;
    }

    public AudioClip GetClip(string _color)
    {
        if (_color.Equals("green") && green.Count > 0) { return green[(int)Random.Range(0,green.Count)]; }
        if (_color.Equals("yellow") && yellow.Count > 0) { return yellow[(int)Random.Range(0, yellow.Count)]; }
        if (_color.Equals("red") && red.Count > 0) { return red[(int)Random.Range(0, red.Count)]; }

        return null;
    }

    public void SpawnCloudsForDay(int _green,int _yellow,int _red)
    { 
        

    }

}
