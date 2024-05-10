using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }
    private HealthPoints healthPoints;

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
