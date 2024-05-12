using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public static GameLoader instance;

    [SerializeField] public string _menuSceneName;
    [SerializeField] public string _gameSceneName;
    [SerializeField] public string _gameOverSceneName;
    [SerializeField] private GameObject HUD;

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
        Phone.OnPlayerCall += HandleVictory;
    }

    private void OnDestroy()
    {
        PlayerHP.OnPlayerDeath -= HandleDeath;
    }

    public void StartGame()
    {
        StartCoroutine(LoadSceneAndActivate(_gameSceneName));
        HUD.SetActive(true);
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
        Debug.Log("Handling player death");
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

    private void HandleVictory()
    {
        Debug.Log("The Player tried to escape!!!");
    }

    private IEnumerator ResetSequence()
    {
        HUD.SetActive(false);
        yield return LoadSceneAndActivate(_gameOverSceneName);

        yield return new WaitForSeconds(5);

        StartCoroutine(LoadSceneAndActivate(_gameSceneName));
        HUD.SetActive(true);

        Debug.Log("Reset sequence complete");
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
