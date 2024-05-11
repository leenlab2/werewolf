using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static int numDeaths = 0;  // relevant to wolf transformation

    public static void OnPlayerDeath()
    {
        numDeaths++;
        Debug.Log("Player has died " + numDeaths + " times.");
    }
}
