using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{

    [SerializeField]
    private AudioMixer audioMixer;

    public string masterVolumeParameter;
    public string musicVolumeParameter;
    public string sfxVolumeParameter;
    public string dialogueVolumeParameter;

    public void SetMasterVolume(float level){
        audioMixer.SetFloat(masterVolumeParameter,toDecibels(level));
    }

    public void SetMusicVolume(float level){
        audioMixer.SetFloat(musicVolumeParameter,toDecibels(level));
    }

    public void SetSFXVolume(float level){
        audioMixer.SetFloat(sfxVolumeParameter,toDecibels(level));
    }

    public void SetDialogueVolume(float level){
        audioMixer.SetFloat(dialogueVolumeParameter,toDecibels(level));
    }

    private float toDecibels(float level){
        return 20.0f * Mathf.Log10(level);
    }

}
