using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _gameOverMusic;
    [SerializeField] private AudioClip[] backgroundMusicByStage = new AudioClip[6];
    [SerializeField] private AudioClip[] combatMusicByStage = new AudioClip[6];

    public static MusicSource instance;

    private AudioSource _audioSource;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;

        SceneManager.activeSceneChanged += ChangedActiveScene;
        EnemyAI.OnCombatEnter += EnterCombat;
        EnemyAI.OnCombatExit += ExitCombat;
    }

    public void PlayMusic(AudioClip music)
    {
        _audioSource.clip = music;
        _audioSource.Play();
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

    public void PauseMusic()
    {
        _audioSource.Pause();
    }

    public void UnpauseMusic()
    {
        _audioSource.UnPause();
    }

    private void ChangedActiveScene(Scene current, Scene newScene)
    {
        if (newScene.name == GameLoader.instance._menuSceneName)
        {
            PlayMusic(_menuMusic);
        } else if (newScene.name == GameLoader.instance._gameSceneName)
        {
            PlayBackgroundMusic(GameState.numDeaths);
        } else if (newScene.name == GameLoader.instance._gameOverSceneName)
        {
            PlayMusic(_gameOverMusic);
        }
    }

    private void EnterCombat()
    {
        PlayCombatMusic(GameState.numDeaths);
    }

    private void ExitCombat()
    {
        PlayBackgroundMusic(GameState.numDeaths);
    }
}
