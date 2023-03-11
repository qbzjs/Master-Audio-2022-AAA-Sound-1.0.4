using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public List<AudioClip> trackList;
    private AudioSource musicSource;
    
    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (musicSource.isPlaying == false)
        {
            musicSource.clip = trackList.PickRandom();
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
