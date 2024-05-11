using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHP))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public static PlayerController instance { get; private set; }
    private PlayerHP healthPoints;
    private PlayerMovement playerMovement;

    public bool isSneaking = false;
    public bool glitchedHP = false;
    public bool appetiteEnabled = true;
    public bool axeEnabled = true;

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

        healthPoints = GetComponent<PlayerHP>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();

        InitPlayerState(GameState.numDeaths);
    }

    private void InitPlayerState(int numDeaths)
    {
        healthPoints.Heal(healthPoints._maxHealth);
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        playerMovement.ChangeBaseSpeed(numDeaths / 5f + 1f);

        glitchedHP = numDeaths >= 1;
        appetiteEnabled = numDeaths < 2;
        axeEnabled = numDeaths < 4;
    }

    public void TakeDamage(int damage)
    {
        healthPoints.TakeDamage(damage);
    }

    public void Heal(int amount)
    {
        healthPoints.Heal(amount);
    }
}
