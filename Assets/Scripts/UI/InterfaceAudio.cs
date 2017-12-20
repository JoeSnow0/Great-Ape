using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceAudio : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClip;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;


    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip[0];
	}
    private void Update()
    {
        audioSource.clip = audioClip[0];
    }

    public void PlayAudioClip(int i)
    {
        if (i > audioClip.Length)
            i = audioClip.Length - 1;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(audioClip[i]);
    }
}
