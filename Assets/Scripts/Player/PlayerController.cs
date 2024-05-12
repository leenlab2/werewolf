using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(PlayerHP))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] public Transform attachTransform;
    private Transform spawnPoint;

    public static PlayerController instance { get; private set; }
    private PlayerHP healthPoints;
    private PlayerMovement playerMovement;
    [HideInInspector] public Inventory inventory;

    [HideInInspector] public bool isSneaking = false;
    [HideInInspector] public bool glitchedHP = false;
    [HideInInspector] public bool appetiteEnabled = true;
    [HideInInspector] public bool axeEnabled = true;
    [HideInInspector] public bool isWerewolf = false;
    [SerializeField] public Item claws;

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
        } else
        {
            inventory.enabled = false;
            healthPoints.enabled = false;
            GetComponent<InteractableDetector>().enabled = false;
        }
    }

    public void InitPlayerState(int numDeaths)
    {
        inventory.enabled = true;
        healthPoints.enabled = true;
        GetComponent<InteractableDetector>().enabled = true;

        healthPoints.Heal(healthPoints._maxHealth);
        transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        playerMovement.ChangeBaseSpeed(numDeaths / 5f + 1f);

        glitchedHP = numDeaths >= 1;
        appetiteEnabled = numDeaths < 2;
        axeEnabled = numDeaths < 4;
        isWerewolf = numDeaths >= 4;

        if(isWerewolf)
        {
            inventory.AddItem(claws);
        }
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
        Debug.Log("Trying to attack!");
        GameObject enemy = FindClosestEnemy();
        if (enemy != null)
        {
            Debug.Log("HIT!");
            enemy.GetComponent<HealthPoints>().TakeDamage(damage);
        }
    }

    void Update()
    {

    }
}
