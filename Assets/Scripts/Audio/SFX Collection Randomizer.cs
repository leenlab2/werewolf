using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class SFXCollection
{
    public List<AudioClip> sfxCollection = new List<AudioClip>();
    public string surfaceTag;
}

[RequireComponent(typeof(AudioSource))]
public class SFXCollectionRandomizer : MonoBehaviour
{
    [SerializeField] private List<SFXCollection> sfxCollections = new List<SFXCollection>();

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomSFX()
    {
        string currentSurfaceTag = GetCurrentSurfaceTag();
        SFXCollection match = sfxCollections.Find(x => x.surfaceTag == currentSurfaceTag);
        if (match != null)
        {
            List<AudioClip> sfxCollection = match.sfxCollection;

            if (sfxCollection.Count > 0)
            {
                int randomIndex = Random.Range(0, sfxCollection.Count);
                audioSource.PlayOneShot(sfxCollection[randomIndex]);
            }
        }
    }

    private string GetCurrentSurfaceTag()
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
