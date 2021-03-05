using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
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

    public void FadeClip()
    {
        fadeTimer += Time.deltaTime;

        SetSourceVolume(cloudSource, cloudVolumeModifier * (fadeTimer / fadeInTime));

        if (fadeTimer >= fadeInTime) { fadeTimer = -1; }
    }



    public void EnterCloud(string _color, int _clip = 0)
    {
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


}
