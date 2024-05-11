using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(PlayerHP))]
public class PlayerController : MonoBehaviour
{
    private Transform spawnPoint;

    public static PlayerController instance { get; private set; }
    private PlayerHP healthPoints;
    private PlayerMovement playerMovement;
    [HideInInspector] public Inventory inventory;

    [HideInInspector] public bool isSneaking = false;
    [HideInInspector] public bool glitchedHP = false;
    [HideInInspector] public bool appetiteEnabled = true;
    [HideInInspector] public bool axeEnabled = true;

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
        inventory = GetComponent<Inventory>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();

        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        if (next.name == GameLoader.instance._gameSceneName)
        {
            spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
            InitPlayerState(GameState.numDeaths);
        }
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

    private GameObject FindClosestEnemy()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100))
        {
            if (hit.transform.gameObject.GetComponent<EnemyAI>())
            {
                return hit.transform.gameObject;
            }
        }

        return null;
    }

    public void AttackAttempt(int damage)
    {
        GameObject enemy = FindClosestEnemy();
        if (enemy != null)
        {
            enemy.GetComponent<HealthPoints>().TakeDamage(damage);
        }
    }
}
