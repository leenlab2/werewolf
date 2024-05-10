using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }
    private HealthPoints healthPoints;
    private Inventory inventory;

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

        healthPoints = GetComponent<HealthPoints>();
        inventory = GetComponent<Inventory>();
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
