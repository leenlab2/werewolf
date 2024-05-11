using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectSfxSource : MonoBehaviour
{
    [Header("SFX Clips")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip usingSound;
    [SerializeField] private AudioClip usedSound;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        Item.OnItemUsed += PlayObjectUseSfx;
    }

    private void PlayObjectPickupSfx()
    {
        if (pickupSound != null)
        {
            _audioSource.clip = pickupSound;
            _audioSource.Play();
        }
    }
    
    private void PlayObjectUseSfx(GameObject obj)
    {
        if (obj != gameObject) return;

        List<AudioClip> clips = new List<AudioClip>();

        if (usingSound != null)
        {
            clips.Add(usingSound);
        }

        if (usedSound != null)
        {
            clips.Add(usedSound);
        }

        StartCoroutine(PlayAudioSequentially(clips));
    }

    IEnumerator PlayAudioSequentially(List<AudioClip> clips)
    {
        for (int i = 0; i < clips.Count; i++)
        {
            while (_audioSource.isPlaying)
            {
                yield return null;
            }

            _audioSource.clip = clips[i];
            _audioSource.Play();            
        }
    }
}
