using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Assignables")]
    public SoundInfo[] sounds;

    public void PlaySound(string name)
    {
        foreach (var sound in sounds)
            if (name == sound.name) { sound.audio.Play(); break; }
    }

    public void PlaySound(string name, Vector3 position)
    {
        foreach (var sound in sounds)
            if (name == sound.name) { sound.audio.Play(); break; }
    }
}

public struct SoundInfo
{
    public string name;
    public AudioSource audio;

}
