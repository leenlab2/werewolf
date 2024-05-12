using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Phone : Item
{
    public static event Action OnPlayerCall;
    public AudioClip hangUpSound;

    public override void PickUp()
    {
        base.PickUp();
        StartCoroutine(End());
    }

    private IEnumerator End()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.clip = hangUpSound;
        audioSource.Play();
        yield return new WaitForSeconds(hangUpSound.length);
        OnPlayerCall?.Invoke();
    }
}
