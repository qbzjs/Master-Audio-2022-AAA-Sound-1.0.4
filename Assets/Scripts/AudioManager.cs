using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public List<AudioClip> trackList;
    private AudioSource musicSource;
    private int currMusic = 0;
    
    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (musicSource.isPlaying == false)
        {
            musicSource.clip = trackList[currMusic];
            currMusic = (currMusic + 1) % trackList.Count;
            musicSource.volume = 0;
            LeanTween.value(gameObject, 0, 1, 10)
                .setEaseInOutQuad()
                .setOnUpdate((value) =>
                {
                    musicSource.volume = value;
                });
            musicSource.Play();
        }
    }
}
