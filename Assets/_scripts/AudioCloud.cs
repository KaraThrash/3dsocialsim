using UnityEngine;

public class AudioCloud : MonoBehaviour
{
    public AudioManager audioManager;
    public AudioClip clip;
    private AudioSource source;
    public string cloudColor;
    public int clipByNumber;
    public float fadeInTime;
    public bool areaBasedSound;

    public float followSpeed;
    public Transform followThis;
    private float fadeTimer = -1;

    // Start is
    // called before
    // the first
    // frame update
    private void Start()
    {
        fadeTimer = -1;
        if (fadeInTime == 0) { fadeInTime = 1; }

        if (followThis != null)
        {
            transform.position = new Vector3(followThis.position.x, transform.position.y, followThis.position.z);
        }
        source = GetComponent<AudioSource>();
    }

    // Update is
    // called once
    // per frame
    private void Update()
    {
        if (fadeTimer != -1) { FadeClip(); }

        if (clip != null && source.isPlaying == false)
        {
            // clip
            // =
            // null;
            // source.clip
            // = clip;
        }
        if (followThis != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(followThis.position.x, transform.position.y, followThis.position.z), Time.deltaTime * followSpeed);
        }
    }

    public void SetType(AudioClip _clip)
    {
        clip = _clip;
        AudioSource().clip = clip;
    }

    public void SetType(string _color, int _clip)
    {
        clipByNumber = _clip;
        cloudColor = _color;
    }

    public void SetType(string _color)
    {
        cloudColor = _color;
    }

    public void FadeClip()
    {
        fadeTimer += Time.deltaTime;
        source.volume = 1 * (fadeTimer / fadeInTime);

        if (fadeTimer >= fadeInTime) { fadeTimer = -1; GetComponent<AudioSource>().volume = 1; }
    }

    public void StartPlaying()
    {
        fadeTimer = 0;
        source.volume = 0;
        if (source != null)
        {
            source.Play();
        }
    }

    public void StopPlaying()
    {
        if (source != null)
        {
            source.Stop();
        }
    }

    public AudioSource AudioSource()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
        return source;
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (audioManager != null )
        //{
        //    if (clip == null)
        //    {
        //        //whether the sound should be based on the cloud's location or not
        //        if (areaBasedSound)
        //        {
        //            //get an audio clip from the manager when the player first enters the cloud
        //            clip = audioManager.GetClip(cloudColor);
        //            fadeTimer = 0;

        // source.clip
        // = clip;
        // AudioSource().Play();
        // } else {
        // audioManager.EnterCloud(cloudColor, clipByNumber);

        // }

        // } else {
        // if
        // (areaBasedSound)
        // { if
        // (AudioSource().isPlaying
        // == false)
        // { //get
        // an audio
        // clip from
        // the
        // manager
        // when the
        // player
        // first
        // enters
        // the cloud
        // fadeTimer
        // = 0;

        // AudioSource().Play(); }

        // } else {
        // audioManager.EnterCloud(cloudColor, clipByNumber);

        // } }

        //}
    }
}