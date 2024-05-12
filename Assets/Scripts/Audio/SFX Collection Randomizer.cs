using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SFXCollectionRandomizer : MonoBehaviour
{
    [System.Serializable]
    public class SFXCollection
    {
        public List<AudioClip> sfxCollection = new List<AudioClip>();
        public string surfaceTag;
    }

    [SerializeField] public List<SFXCollection> sfxCollections = new List<SFXCollection>();

    protected AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public AudioClip GetRandomSFX(SFXCollection match)
    {
        if (match != null)
        {
            List<AudioClip> sfxCollection = match.sfxCollection;

            if (sfxCollection.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, sfxCollection.Count);
                return sfxCollection[randomIndex];
            }
        }

        return null;
    }

    public void PlayRandomSFX()
    {
        string currentSurfaceTag = GetCurrentSurfaceTag();
        SFXCollection match = sfxCollections.Find(x => x.surfaceTag == currentSurfaceTag);
        AudioClip audioClip = GetRandomSFX(match);

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    protected string GetCurrentSurfaceTag()
    {
        int layerMask = LayerMask.GetMask("Surface");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, layerMask))
        {
            return hit.collider.tag;
        }
        return "";
    }
}
