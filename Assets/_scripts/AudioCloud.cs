using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCloud : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip clip;
    public string cloudColor;
    public int clipByNumber;
    public bool areaBasedSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (clip != null && GetComponent<AudioSource>().isPlaying == false)
        {
            clip = null;
            GetComponent<AudioSource>().clip = clip;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (audioManager != null && clip == null)
        {

            //whether the sound should be based on the cloud's location or not
            if (areaBasedSound)
            {
                //get an audio clip from the manager when the player first enters the cloud
                clip = audioManager.GetClip(cloudColor);
                GetComponent<AudioSource>().clip = clip;
                GetComponent<AudioSource>().Play();
            }
            else
            { 
                 audioManager.EnterCloud(cloudColor,clipByNumber);

            }

          
        }
    }

}
