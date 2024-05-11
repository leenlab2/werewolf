using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
        if (focus && SceneManager.GetActiveScene().name == GameLoader.instance._gameSceneName)
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
}
