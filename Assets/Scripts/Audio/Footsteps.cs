using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : SFXCollectionRandomizer
{
    private float lastStepTime = 0.0f;

    public void PlayFootsteps(float velocity)
    {
        // Determine whether to play a noise depending on speed
        float timePerStep = 5 / velocity;
        if (Time.time - lastStepTime < timePerStep) { return; }

        // Set time for next step
        lastStepTime = Time.time;

        PlayRandomSFX();
    }

}
