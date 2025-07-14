using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Jump&Pry/Audio/Library")]

public class AudioLibrary : ScriptableObject
{
    [Serializable]
    public class AudioClipEntry { public string key; public AudioClip clip; }

    [SerializeField] public List<AudioClipEntry> audioClips;
    private Dictionary<string, AudioClip> audioClipDictionary;

    private void Awake()
    {
        if (audioClips == null)
        {
            audioClips = new List<AudioClipEntry>();
        }

        InitializeAudioDictionary();
    }

    public AudioClip GetAudioClip(string key)
    {
        InitializeAudioDictionary();

        if (audioClipDictionary.TryGetValue(key, out AudioClip clip))
        {
            return clip;
        }
        else
        {
            Debug.LogWarning($"Audio clip with key '{key}' not found in the AudioLibrary.");
            return null;
        }
    }

    private void InitializeAudioDictionary()
    {
        if (audioClipDictionary == null || audioClipDictionary.Count == 0)
        {
            audioClipDictionary = audioClips.ToDictionary(entry => entry.key, entry => entry.clip);
        }
    }
}
