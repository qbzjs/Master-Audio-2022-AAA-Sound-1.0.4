using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameObject pauseParent;
    private bool paused = false;
    
    
    [SerializeField, Required]
    private AudioMixer masterMixer;

    private static float MIN_VOLUME = 0.0001f, MAX_VOLUME = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !pauseParent.activeSelf;
            pauseParent.SetActive(paused);
        }
    }

    //TODO use when we're making a downloadable build
    public void Quit()
    {
        Application.Quit();
    }
    
    /// <summary>
    /// Sets the master volume, which attenuates both Sound Effects (SFX) and background music (BGM).
    /// </summary>
    /// <param name="volume">Should be between 0 (muted) and 1 (full volume)</param>
    public void SetVolumeMaster(float volume)
    {
        SetVolume(volume, "MasterVolume");
    }
    
    public void SetVolumeBGM(float volume)
    {
        SetVolume(volume, "BGMVolume");
    }
    
    public void SetVolumeSFX(float volume)
    {
        SetVolume(volume, "SFXVolume");
    }
    
    private void SetVolume(float volume, string parameterName)
    {
        volume = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
        volume =  Mathf.Log10(volume) * 20; //-80db is muted, 0 is regular "full" volume
        masterMixer.SetFloat(parameterName, volume);
    }

    /*private float NormalizedToDB(float normalizedValue)
    {
        
    }
    
    private float DBToNormalized(float dbValue)
    {
        
    }*/
    
    
}
