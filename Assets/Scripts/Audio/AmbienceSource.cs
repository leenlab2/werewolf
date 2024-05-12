using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmbienceSource : SFXCollectionRandomizer
{
    private bool isGameScene = false;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name == GameLoader.instance._gameSceneName)
        {
            isGameScene = true;
            StartCoroutine(LoopAmbience());
        }
        else
        {
            audioSource.Stop();
            isGameScene = false;
        }
    }

    private IEnumerator LoopAmbience()
    {
        while (isGameScene)
        {
            PlayRandomSFX();
            if (audioSource.clip == null)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(audioSource.clip.length);
            }
        }
    }
}
