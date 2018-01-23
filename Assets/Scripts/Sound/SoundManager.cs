using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance
    {
        get
        {
            // If an instance of this script doesn't exist already
            if (m_instance == null)
            {
                // Creates a GameObject based of this class
                GameObject prefab = (GameObject)Resources.Load("SingletonPrefabs/SoundManager");
                // Instantiates the created GameObject
                GameObject created = Instantiate(prefab);
                // Prevents the object from being destroyed when changing scenes
                DontDestroyOnLoad(created);
                // Assigns an instance of this script
                m_instance = created.GetComponent<SoundManager>();
                m_instance.m_Source = created.GetComponent<AudioSource>();
                m_instance.UpdateVolume();
            }

            return m_instance;
        }
    }

    private static SoundManager m_instance;

    private AudioSource m_Source;

    public void UpdateVolume()
    {
        if (m_Source.volume <= 0)
        {
            m_Source.mute = true;
        }
        else
            m_Source.mute = false;
    }

    public void PlaySound(AudioClip clip, float volMod = 1)
    {
        m_Source.pitch = 1;
        m_Source.PlayOneShot(clip, m_Source.volume * volMod);
    }
    public void PlayPitched(AudioClip clip, float minPitch, float maxPitch, float volMod = 1)
    {
        m_Source.pitch = Random.Range(minPitch, maxPitch);
        m_Source.PlayOneShot(clip, m_Source.volume * volMod);
    }
    public void PlayDelayed(AudioClip clip, float delay)
    {
        m_Source.clip = clip;
        m_Source.PlayDelayed(delay);
    }
}