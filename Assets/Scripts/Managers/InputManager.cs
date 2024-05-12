using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene newScene)
    {
        if (newScene.name == GameLoader.instance._gameSceneName)
        {
            Debug.Log("Game Scene Loaded");
            playerInput.actions.FindActionMap("Player").Enable();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            string activeScene = SceneManager.GetActiveScene().name;
            if (activeScene == GameLoader.instance._gameSceneName)
            {
                HideCursor();
            }
        }
        else
        {
            ShowCursor();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        ShowCursor();
        playerInput.actions.FindActionMap("Player").Disable();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        HideCursor();
        playerInput.actions.FindActionMap("Player").Enable();
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
