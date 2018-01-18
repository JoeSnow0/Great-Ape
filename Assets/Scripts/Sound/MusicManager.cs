using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance
    {
        get
        {
            // If an instance of this script doesn't exist already
            if (m_instance == null)
            {
                // Creates a GameObject based of this class
                GameObject prefab = (GameObject)Resources.Load("SingletonPrefabs/MusicManager");
                // Instantiates the created GameObject
                GameObject created = Instantiate(prefab);
                // Prevents the object from being destroyed when changing scenes
                DontDestroyOnLoad(created);
                // Assigns an instance of this script
                m_instance = created.GetComponent<MusicManager>();
                m_instance.m_Source = created.GetComponent<AudioSource>();
                m_instance.UpdateVolume();
            }

            return m_instance;
        }
    }

    private static MusicManager m_instance;

    [System.NonSerialized]
    public bool isPlaying = false;

    [System.NonSerialized]
    public AudioSource m_Source;

    private List<AudioClip> playlist = new List<AudioClip>();
    bool m_playingList = false;

    float m_clipTimer = 0;
    int m_listIndex = 0;

    private void Update()
    {
        if (!m_playingList)
            return;

        if (m_clipTimer <= 0)
        {
            if (++m_listIndex >= playlist.Count)
                m_listIndex = 0;

            m_Source.clip = playlist[m_listIndex];
            m_clipTimer = m_Source.clip.length;
            m_Source.Play();
        }
        else
        {
            m_clipTimer -= Time.deltaTime;
        }
    }

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
        m_Source.clip = clip;
        m_Source.volume = m_Source.volume * volMod;
        m_Source.Play();
        isPlaying = true;
    }

    public void StartPlaylist(List<AudioClip> clips)
    {
        Stop();
        playlist = clips;
        m_playingList = true;
        isPlaying = true;
        m_listIndex = Random.Range(0, clips.Count);
        m_Source.clip = playlist[m_listIndex];
        m_clipTimer = m_Source.clip.length;
        m_Source.Play();
    }

    public void Stop()
    {
        m_Source.Stop();
        isPlaying = false;
        m_playingList = false;
    }
}