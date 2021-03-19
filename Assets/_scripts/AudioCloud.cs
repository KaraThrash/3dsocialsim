using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCloud : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip clip;
    public string cloudColor;
    public int clipByNumber;
    public float fadeInTime;
    public bool areaBasedSound;

    public float followSpeed;
    public Transform followThis;
    private float fadeTimer = -1;
    // Start is called before the first frame update
    void Start()
    {
        if (followThis != null)
        {
            transform.position = new Vector3(followThis.position.x, transform.position.y, followThis.position.z);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (fadeTimer != -1) { FadeClip(); }


        if (clip != null && GetComponent<AudioSource>().isPlaying == false)
        {
            clip = null;
            GetComponent<AudioSource>().clip = clip;
        }
        if (followThis != null)
        { 
            transform.position = Vector3.MoveTowards(transform.position,new Vector3(followThis.position.x,transform.position.y,followThis.position.z),Time.deltaTime * followSpeed); 
        
        }
    }


    public void FadeClip()
    {

        fadeTimer += Time.deltaTime;
        GetComponent<AudioSource>().volume = 1 * (fadeTimer / fadeInTime);

        if (fadeTimer >= fadeInTime) { fadeTimer = -1; GetComponent<AudioSource>().volume = 1; }
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
                fadeTimer = 0;

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
