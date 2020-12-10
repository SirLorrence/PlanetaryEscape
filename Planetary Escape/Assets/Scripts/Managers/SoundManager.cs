using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static AudioSource audioSource;

    public List<AudioSource> audioSources;


    public AudioClip bullet;
    public AudioClip laser;
    public AudioClip pointPickUp;
    public AudioClip healthpickUp;
    public AudioClip death;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // audioSource = GetComponent<AudioSource>();
        StartCoroutine(CreateAudioSources());
    }

    public static void NotifyAudio(AudioClip audioClip)
    {
        // Instance.StartCoroutine(Instance.SoundPlaySound(audioClip));

        var sfxSource = getSource();
        sfxSource.clip = audioClip;
        sfxSource.Play();

    }
    // IEnumerator SoundPlaySound(AudioClip clip)
    // {
    //     
    //     var SFXSource = Instance.gameObject.AddComponent<AudioSource>();
    //     SFXSource.clip = clip;
    //     SFXSource.Play();
    //     yield return new WaitUntil(() => !SFXSource.isPlaying);
    //     Destroy(SFXSource);
    //     print("Done");
    //     yield return null;
    // }


   static AudioSource getSource()
    {
        foreach (var source in Instance.audioSources)
        {
            if (source != source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }

    IEnumerator CreateAudioSources()
    {
        for (int i = 0; i <= 25; i++)
        {
            audioSources.Add(gameObject.AddComponent<AudioSource>());
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}