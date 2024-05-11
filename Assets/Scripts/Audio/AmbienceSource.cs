using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSource : MonoBehaviour
{
    [SerializeField] private AudioClip forestAmbience;
    [SerializeField] private AudioClip streetAmbience;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }
}
