using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> playlist;

	void Awake ()
    {
        if (!MusicManager.instance.isPlaying)
            MusicManager.instance.StartPlaylist(playlist);
    }
}
