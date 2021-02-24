using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castFireBall : MonoBehaviour
{

    public float force = 40f;
    public GameObject fireBallPrefab;
    public Transform fireBallSpawn;

    // Update is called once per frame

    // Cast a fire ball by clicking the right mouse button.

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cast();
        }        
    }

    void Cast()
    {
        GameObject fireBall = Instantiate(fireBallPrefab, transform.position, transform.rotation);
        Rigidbody rigidbody = fireBall.GetComponent<Rigidbody>();

        rigidbody.AddForce(transform.forward * force, ForceMode.VelocityChange);
    }
}
