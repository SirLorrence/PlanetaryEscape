using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Settings")]
    public bool debug = false;

    [Header("Assignables")]
    public AudioTrack[] tracks;

    private Hashtable _audioTable; //Relation between Audio Types (Key) and Audio Tracks (Value)
    private Hashtable _jobTable; //Relation between Audio Types (Key) and Jobs (Value)

    #region Unity Functions

    #region Singleton
    //Singleton Instantiation
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            Configure();
        }

        DontDestroyOnLoad(this);
    }
    #endregion

    private void OnDisable()
    {

    }
    #endregion

    #region Public Functions
    public void PlayAudio(AudioTypes audioType)
    {
        AddJob(job: new AudioJob(action: AudioAction.Start, type: audioType));
    }
    public void StopAudio(AudioTypes audioType)
    {
        AddJob(job: new AudioJob(action: AudioAction.Stop, type: audioType));
    }
    public void RestartAudio(AudioTypes audioType)
    {
        AddJob(job: new AudioJob(action: AudioAction.Restart, type: audioType));
    }
    #endregion

    #region Private Functions
    private void Configure()
    {
        _audioTable = new Hashtable();
        _jobTable = new Hashtable();
        PopulateAudioTable();
    }

    private void Dispose()
    {
        foreach (DictionaryEntry entry in _jobTable)
        {
            IEnumerator job = (IEnumerator)entry.Value;
            StopCoroutine(job);
        }
    }

    private void PopulateAudioTable()
    {
        foreach (var track in tracks)
        {
            foreach (var audioObject in track.audio)
            {
                if (_audioTable.ContainsKey(audioObject.type)) { LogWarning(msg: "Trying to register audio [" + audioObject.type + "] that has already been registered."); break; }

                _audioTable.Add(audioObject.type, track);
                Log(msg: "Registering Audio [" + audioObject.type + "].");
            }
        }
    }

    private void AddJob(AudioJob job)
    {
        RemoveConflictingJobs(type: job.type);

        IEnumerator jobRunner = RunAudioJob(job: job);
        _jobTable.Add(job.type, jobRunner);
        StartCoroutine(jobRunner);
        Log("Starting Job on [" + job.type + "] with operation: " + job.action);
    }

    private IEnumerator RunAudioJob(AudioJob job)
    {
        AudioTrack track = (AudioTrack)_audioTable[job.type];
        track.source.clip = GetAudioClipFromTrack(type: job.type, track: track);

        switch (job.action)
        {
            case AudioAction.Start:
                track.source.Play();
                break;
            case AudioAction.Restart:
                track.source.Stop();
                track.source.Play();
                break;
            case AudioAction.Stop:
                track.source.Stop();
                break;
        }

        _jobTable.Remove(job.type);
        Log("Job Count: " + _jobTable.Count); ;

        yield return null;
    }

    private AudioClip GetAudioClipFromTrack(AudioTypes type, AudioTrack track)
    {
        foreach (AudioObject obj in track.audio)
        {
            if (obj.type == type)
            {
                return obj.clip;
            }
        }
        return null;
    }

    private void RemoveConflictingJobs(AudioTypes type)
    {
        if (_jobTable.ContainsKey(type))
            RemoveJob(type: type);

        AudioTypes conflictAudio = AudioTypes.None;

        foreach (DictionaryEntry entry in _jobTable)
        {
            AudioTypes audioType = (AudioTypes)entry.Key;
            AudioTrack currentAudioTrack = (AudioTrack)_audioTable[audioType];
            AudioTrack audioTrackNeeded = (AudioTrack)_audioTable[type];

            if (currentAudioTrack.source == audioTrackNeeded.source)
            {
                //Can't Remove Element while Iterating thru
                conflictAudio = audioType;
            }
        }

        if (conflictAudio != AudioTypes.None)
        {
            RemoveJob(type: conflictAudio);
        }
    }

    private void RemoveJob(AudioTypes type)
    {
        if (!_jobTable.ContainsKey(type))
        {
            LogWarning("Trying to stop a job [" + type + "] that is not running.");
            return;
        }
        IEnumerator runningJob = (IEnumerator)_jobTable[type];
        StopCoroutine(runningJob);
        _jobTable.Remove(type);
    }

    private void Log(string msg)
    {
        if (!debug) return;

        Debug.Log("[SOUNDMANAGER]: " + msg);
    }
    private void LogWarning(string msg)
    {
        if (!debug) return;

        Debug.LogWarning("[SOUNDMANAGER]: " + msg);
    }
    #endregion
}

#region Structs
[System.Serializable]
public struct AudioTrack
{
    public AudioSource source;
    public AudioObject[] audio;
}

[System.Serializable]
public struct AudioObject
{
    public AudioTypes type;
    public AudioClip clip;
}
[System.Serializable]
public struct AudioJob
{
    public AudioAction action;
    public AudioTypes type;

    public AudioJob(AudioAction action, AudioTypes type)
    {
        this.action = action;
        this.type = type;
    }
}
public enum AudioAction
{
    Start,
    Stop,
    Restart
}
public enum AudioTypes
{
    None,
    MENU_TRACK,
    GAME_TRACK,
    SFX_ZOMBIE_GROAN,
    SFX_ZOMBIE_GROAN2,
    SFX_ZOMBIE_GROAN3,
    SFX_RIFLE_SHOOT,
    SFX_RIFLE_RELOAD,
    SFX_PISTOL_SHOOT,
    SFX_PISTOL_RELOAD,
    SFX_FOOTSTEP1,
    SFX_FOOTSTEP2
}
#endregion
