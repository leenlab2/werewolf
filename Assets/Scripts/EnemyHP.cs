using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : HealthPoints
{

    public override void Die()
    {
        base.Die();
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(gameObject, 1);
    }
}

