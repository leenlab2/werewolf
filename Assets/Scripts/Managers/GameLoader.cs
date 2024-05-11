using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public static GameLoader instance;
    [SerializeField] private string _mainScene;

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
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        StartCoroutine(LoadYourAsyncScene(_mainScene));
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus && SceneManager.GetActiveScene().name == _mainScene)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public static IEnumerator LoadYourAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
