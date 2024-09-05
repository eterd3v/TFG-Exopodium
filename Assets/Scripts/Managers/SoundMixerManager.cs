using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{

    [SerializeField]
    private AudioMixer audioMixer;

    public string musicVolumeParameter;
    public string sfxVolumeParameter;

    public void SetMusicVolume(float level){
        audioMixer.SetFloat(musicVolumeParameter,toDecibels(level));
    }

    public void SetSFXVolume(float level){
        audioMixer.SetFloat(sfxVolumeParameter,toDecibels(level));
    }

    private float toDecibels(float level){
        return 20.0f * Mathf.Log10(level);
    }

}
