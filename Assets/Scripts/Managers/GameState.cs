using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static int numDeaths = 0;  // relevant to wolf transformation
    public static GameState instance { get; private set; }

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

    public void OnPlayerDeath()
    {
        numDeaths++;
    }
}
