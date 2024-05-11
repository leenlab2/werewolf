using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public static GameLoader instance;

    [SerializeField] private string _menuSceneName;
    [SerializeField] public string _gameSceneName;
    [SerializeField] private string _gameOverSceneName;

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

        StartCoroutine(LoadSceneAndActivate(_menuSceneName));

        PlayerHP.OnPlayerDeath += HandleDeath;
    }

    private void OnDestroy()
    {
        PlayerHP.OnPlayerDeath -= HandleDeath;
    }

    public void StartGame()
    {
        StartCoroutine(LoadSceneAndActivate(_gameSceneName));
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadSceneAndActivate(_menuSceneName));
    }

    public static void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    private void HandleDeath()
    {
        if (GameState.numDeaths < 5)
        {
            GameState.OnPlayerDeath();
            StartCoroutine(ResetSequence());
        }
        else
        {
            EndGame();
        }
    }

    private IEnumerator ResetSequence()
    {
        yield return LoadSceneAndActivate(_gameOverSceneName);

        yield return new WaitForSeconds(3);

        StartCoroutine(LoadSceneAndActivate(_gameSceneName));
    }

    private void EndGame()
    {
        StartCoroutine(LoadSceneAndActivate(_menuSceneName));
    }

    public IEnumerator LoadSceneAndActivate(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene activeScene = SceneManager.GetActiveScene();
        if (activeScene != gameObject.scene)
        {
            StartCoroutine(UnLoadSceneAsync(activeScene.name));
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }

    public static IEnumerator UnLoadSceneAsync(string scene)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);

        // Wait until the asynchronous scene fully unloads
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }
}
