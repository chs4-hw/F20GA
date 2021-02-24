using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    Rigidbody body;
    public float objectLife = 5f;
    float time;    
    bool fireBallContact = false;
    public GameObject contactEffect;
    public float rangedDamage = 5f;
    public float effectRadius = 1f;
    public float force = 500f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime; 

        if (time >= objectLife)
        {
            Destroy(gameObject);
        }

        if (!fireBallContact)
        {
            transform.rotation = Quaternion.LookRotation(body.velocity);           
        }
    }

    // Fireball collision detection on objects within the level
    void OnCollisionEnter(Collision collision)
    {

        if(collision.collider.tag != "WeaponLong" || collision.collider.tag == "Terrain")
        {
            fireBallContact = true;
            Contact();
        }
    }

    // Fireballs explode effecting a small area around it and destroy the fireball once used. 
    // Enemies take damage to health.  
    void Contact()
    {

        body.constraints = RigidbodyConstraints.FreezeAll;

        Instantiate(contactEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, effectRadius);

        foreach (Collider nearby in colliders)
        {
            Rigidbody rigidbody = nearby.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(force, transform.position, effectRadius);

                if (nearby.GetComponent<Collider>().tag == "Enemy")
                {
                    EnemyHealth health = nearby.GetComponent<Collider>().GetComponent<EnemyHealth>();
                    health.Health(rangedDamage);
                }

            }
        }

        Destroy(gameObject);
    }
}
