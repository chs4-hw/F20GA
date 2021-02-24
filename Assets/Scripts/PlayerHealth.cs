using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth = 0;
    public int damage;
    public Health healthBar;
    public float healthRegenertion;
    private float time;

    public Camera player;
    public float safeRadius = 100f;
    bool safe = true;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);       
    }

    // Update is called once per frame
    // Health slowly regrenrates while not in 'range'(combat) of enemy, and updates the health bar 
    void Update()
    {
        if (currentHealth <= 0)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
        if (currentHealth != maxHealth)
        {
            RegenerateHealth();
        }
    }

    void RegenerateHealth()
    {
        Ray ray = player.ScreenPointToRay(Input.mousePosition);

        RaycastHit other;

        if (Physics.Raycast(ray, out other, safeRadius))
        {
            if (other.collider.tag != "Enemy")
            {

                if (currentHealth < maxHealth)
                {
                    time += Time.deltaTime;

                    if (time >= healthRegenertion)
                    {
                        currentHealth = currentHealth + 1;
                        time = 0f;
                    }

                    healthBar.SetHealth(currentHealth);
                }
            }
        }
    }

    // Enemy attack damage effect
    public void TakeDamage(int value)
    {
        int damage = value;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, safeRadius);
    }
}
