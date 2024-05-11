using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class UISfxSource : MonoBehaviour, ISelectHandler
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject == gameObject)
        {
            _audioSource.Play();
        }
    }
}
