using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSandbox : MonoBehaviour
{
    public AudioManager audioManager;
    public List<Slider> sliders;
    public List<Text> sliderValues;

    public GameObject uiParent;

    // Start is
    // called before
    // the first
    // frame update
    private void Start()
    {
        ResetToDefault();
    }

    // Update is
    // called once
    // per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        { ToggleUi(); }
    }

    public void UpdateAudio()
    {
        audioManager.greenChance = sliders[0].value * 0.01f;
        audioManager.yellowChance = sliders[1].value * 0.01f;
        audioManager.maxTimeWithoutCloud = sliders[2].value;
        audioManager.intervalLength = sliders[3].value;
        audioManager.windGustRadius = sliders[4].value;
        audioManager.windGustVolumn = sliders[5].value;
        audioManager.fadeInTime = sliders[6].value;

        sliderValues[0].text = sliders[0].value.ToString();
        sliderValues[1].text = sliders[1].value.ToString();
        sliderValues[2].text = sliders[2].value.ToString();
        sliderValues[3].text = sliders[3].value.ToString();
        sliderValues[4].text = sliders[4].value.ToString();
        sliderValues[5].text = sliders[5].value.ToString();
        sliderValues[6].text = sliders[6].value.ToString();
    }

    public void ResetToDefault()
    {
        sliders[0].value = 30;
        sliders[1].value = 40;
        sliders[2].value = 120;
        sliders[3].value = 7;
        sliders[4].value = 5;
        sliders[5].value = 0.1f;
        sliders[6].value = 12;

        UpdateAudio();
    }

    public void StopCurrentMusic()
    {
        audioManager.cloudInWorldSource.StopPlaying();
        audioManager.silenceTimer = 0;
        audioManager.intervalTimer = 0;
    }

    public void ToggleUi()
    {
        if (uiParent != null)
        {
            uiParent.SetActive(!uiParent.activeSelf);
        }
    }
}