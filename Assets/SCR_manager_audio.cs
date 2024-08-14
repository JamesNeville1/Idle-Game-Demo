using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using UnityEngine;

public class SCR_manager_audio : MonoBehaviour {

    [SerializedDictionary("ID", "Clips")] [SerializeField]
    private SerializedDictionary<string, AudioClip[]> sfxs = new SerializedDictionary<string, AudioClip[]>(); //Hold audio clips

    [SerializeField] private AudioSource sfxSource; //Audio source, in external scene to reduce strain on game
    
    #region Set Instance
    public static SCR_manager_audio instance { private set; get; }

    private void Awake() {
        instance = this;
    }
    #endregion
    #region Play
    public void PlayOneEffect(string toPlay, float volume = 1f) //Play single effect
    {
        sfxSource.PlayOneShot(sfxs[toPlay][0], volume);
    }
    public void PlayRandomEffect(string toPlay, float volume = 1f) //Play single effect in array, randomly selected
    {
        int rand = UnityEngine.Random.Range(0, sfxs[toPlay].Length);
        sfxSource.PlayOneShot(sfxs[toPlay][rand], volume);
    }
    #endregion
}
