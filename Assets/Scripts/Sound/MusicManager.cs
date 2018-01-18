using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [System.NonSerialized]
    public static AudioSource m_Source;
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        m_Source = GetComponent<AudioSource>();
        UpdateVolume();
    }

    public static void UpdateVolume()
    {
        if (m_Source.volume <= 0)
        {
            m_Source.mute = true;
        }
        else
            m_Source.mute = false;
    }

    public static void PlaySound(AudioClip clip, float volMod = 1)
    {
        m_Source.clip = clip;
        m_Source.volume = m_Source.volume * volMod;
        m_Source.Play();
    }

    public static void Stop()
    {
        m_Source.Stop();
    }
}