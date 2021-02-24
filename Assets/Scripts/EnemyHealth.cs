using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    // Enemy Health Status and despawn 
    public float healthStatus = 100f;

    public void Health(float damage)
    {
        healthStatus -= damage;

        if (healthStatus <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
