using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Events;

public class SCR_master_audio : MonoBehaviour {

    [SerializedDictionary("ID", "Clips")] [SerializeField]
    private SerializedDictionary<string, AudioClip[]> sfxs = new SerializedDictionary<string, AudioClip[]>(); //Hold audio clips

    [SerializeField] private AudioSource sfxSource; //Audio source, in external scene to reduce strain on game
    
    #region Set Instance
    public static SCR_master_audio instance { private set; get; }

    private void Awake() {
        instance = this;
    }
    #endregion
    #region Play
    public void playOneEffect(string toPlay, float volume = 1f) { //Play single effect
        sfxSource.PlayOneShot(sfxs[toPlay][0], volume);
    }
    public void playRandomEffect(string toPlay, float volume = 1f) { //Play single effect in array, randomly selected
        int rand = UnityEngine.Random.Range(0, sfxs[toPlay].Length);
        sfxSource.PlayOneShot(sfxs[toPlay][rand], volume);
    }
    #endregion
    #region Change Volume
    //Change volume - Used in menu
    public void changeSFXVolume(float value) {
        sfxSource.volume = value;
    }
    #endregion
}
