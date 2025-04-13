using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicAndSFX : MonoBehaviour
{
    // Script to play a variety of game music and sfx that 
    // are treated as if in 2D space (no rolloff based on distance). 
    private AudioSource audiosource; 
    [SerializeField] private AudioClip mainMenuMusic; 
    [SerializeField] private AudioClip inGameMusic;
    [SerializeField] private AudioClip timesUpMusic; 
    [SerializeField] private AudioClip positiveGameOverSFX; 
    [SerializeField] private AudioClip negativeGameOverSFX; 

    void Awake()
    {
        audiosource = GetComponent<AudioSource>(); if (audiosource==null) Debug.LogError("Audiosource null");
    }

    public void PlayMainMenuMusic() { 
        Debug.Log("Play main menu music called"); 
        audiosource.volume = 0.5f; 
        ChangeClipAndPlay(mainMenuMusic); 
        audiosource.volume = 0.5f; 
    }

    public void PlayInGameMusic() { 
        Debug.Log("PlayInGameMusic called"); 
        audiosource.volume = 0.2f; 
        ChangeClipAndPlay(inGameMusic); 
        audiosource.volume = 0.2f; 
        audiosource.loop = true; 
        Debug.Log("audio source vol: " + audiosource.volume);
    }   

    public void PlayTimesUpMusic() { 
        audiosource.volume = 1f; 
        audiosource.loop = false; 
       ChangeClipAndPlay(timesUpMusic); 
       audiosource.volume = 1f; 
        audiosource.loop = false; 
    }

    public void PlayPositiveGameOverSFX() { 
        Debug.Log("Positive game over music called");
        ChangeClipAndPlay(positiveGameOverSFX); 
        StartCoroutine(PlayMusicAfterGameOver(positiveGameOverSFX)); 
    }

    public void PlayNegativeGameOverSFX() { 
        Debug.Log("Negative game over music called");
        ChangeClipAndPlay(negativeGameOverSFX); 
        StartCoroutine(PlayMusicAfterGameOver(negativeGameOverSFX)); 
    }

    IEnumerator PlayMusicAfterGameOver(AudioClip clip) { 
        Debug.Log("Play music after game over called");
        Debug.Log(clip.length + " is clip length"); 
        yield return new WaitForSecondsRealtime(clip.length);
        Debug.Log("Done waiting"); 
        audiosource.Stop(); 
        Debug.Log("Stopped audio"); 
        
        PlayMainMenuMusic(); 
    }
    void ChangeClipAndPlay(AudioClip clip) { 
        audiosource.clip = clip; 
        audiosource.Play();
    }

    public void StopMusic() { 
        audiosource.Stop(); 
    }

}
