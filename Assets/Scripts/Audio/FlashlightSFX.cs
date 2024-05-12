using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightSFX : ObjectSfxSource
{
    [SerializeField] private AudioClip chargingSfx;

    protected override void Start()
    {
        base.Start();
        Flashlight.OnFlashlightCharge += PlayChargingSfx;
    }

    private void OnDestroy()
    {
        Flashlight.OnFlashlightCharge -= PlayChargingSfx;
    }

    private void PlayChargingSfx(bool charging)
    {
        if (charging)
        {
            _audioSource.clip = chargingSfx;
            _audioSource.loop = true;
            _audioSource.Play();
        }
        else
        {
            _audioSource.loop = false;
            _audioSource.Stop();
        }
    }
}
