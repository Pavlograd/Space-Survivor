using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SFXs
{
    public string s_name;
    public AudioClip[] s_clips;
}

// Script to play a random SFX among a list
// Can be used for example for shoots or hits
// Also Handle multiple SFX at a time in case you want to merge them
public class SFXsManager : MonoBehaviour
{
    [Header("SFXs")]
    [SerializeField] List<SFXs> m_sfx_s = new List<SFXs>();

    [Header("Components")]
    [SerializeField] AudioSource m_source;

    public void Play(string name)
    {
        // Choose a random clip
        AudioClip[] s_clips = m_sfx_s.Find((x) => x.s_name == name).s_clips;

        m_source.clip = s_clips[UnityEngine.Random.Range(0, s_clips.Length)];

        m_source.Play();
    }
}
