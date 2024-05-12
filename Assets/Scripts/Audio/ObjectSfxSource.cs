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
    [SerializeField] private AudioClip errorSound;

    protected AudioSource _audioSource;
    [SerializeField]  private SFXCollectionRandomizer _sfxCollectionRandomizer;

    protected virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _sfxCollectionRandomizer = GetComponent<SFXCollectionRandomizer>();

        Item.OnItemUsed += PlayObjectUseSfx;
        Item.OnItemPickedUp += PlayObjectPickupSfx;
        Item.OnItemError += PlayErrorSfx;
    }

    private void OnDestroy()
    {
        Item.OnItemUsed -= PlayObjectUseSfx;
        Item.OnItemPickedUp -= PlayObjectPickupSfx;
    }

    private void PlayErrorSfx(GameObject obj)
    {
        if (errorSound != null && obj == gameObject)
        {
            _audioSource.clip = errorSound;
            _audioSource.Play();
        }
    }

    private void PlayObjectPickupSfx(GameObject obj)
    {
        if (pickupSound != null && obj == gameObject)
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
        } else if (_sfxCollectionRandomizer != null)
        {
            AudioClip audioClip = _sfxCollectionRandomizer.GetRandomSFX(_sfxCollectionRandomizer.sfxCollections[0]);
            Debug.Log("Playing random sfx: " + audioClip.name);
            clips.Add(audioClip);
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
