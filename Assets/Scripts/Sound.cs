using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class Sound : MonoBehaviour
{
   // public AudioSource audio;
    public Sprite audioOn;
    public Sprite audioOff;
    public Slider _slider;
    public bool first = true;
    public static int pos = 0;
    [Inject]
    public void Construct(SettingsInstaller settingsInstaller)
    {
        _slider = settingsInstaller.slider;
    }
    void Awake()
    {
        if (first)
        {
            PlayerPrefs.SetInt("sound", 0);
            PlayerPrefs.Save();
            first = false;
        }
        _slider.value = PlayerPrefs.GetInt("sound");
        _slider.value = pos;
        if (pos == 0) _slider.handleRect.GetComponent<Image>().sprite = audioOn;
        else _slider.handleRect.GetComponent<Image>().sprite = audioOff;
    }
        public void SliderControl()
    {
        PlayerPrefs.SetInt("sound", Convert.ToInt32(_slider.value));
        PlayerPrefs.Save();
        if (_slider.value == 1)
        {
            AudioManager.audio.volume = 0;
            _slider.handleRect.GetComponent<Image>().sprite = audioOff;
            pos = 1;
        }
        else
        {
            AudioManager.audio.volume = 1;
            _slider.handleRect.GetComponent<Image>().sprite = audioOn;
            pos = 0;
        }
    }
}
