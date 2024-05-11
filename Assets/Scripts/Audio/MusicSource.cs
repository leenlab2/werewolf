using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundMusicByStage = new AudioClip[5];
    [SerializeField] private AudioClip[] combatMusicByStage = new AudioClip[5];

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
    }

    public void PlayBackgroundMusic(int stage)
    {
        _audioSource.clip = backgroundMusicByStage[stage];
        _audioSource.Play();
    }

    public void PlayCombatMusic(int stage)
    {
        _audioSource.clip = combatMusicByStage[stage];
        _audioSource.Play();
    }
}
