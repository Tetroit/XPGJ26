using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGM : MonoBehaviour
{
    AudioSource musicSource;
    [SerializeField]
    AudioClip musicClip;
    private void Awake()
    {
        musicSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Play();
    }
    private void Play()
    {
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
