using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public Camera player;
    public GameObject weapon;
    public Melee katana;
    private float time;
    public Animator action;

    // Start is called before the first frame update
    void Start()
    {
        action = weapon.GetComponent<Animator>();
        katana = weapon.GetComponentInChildren<Melee>();
    }

    // Update is called once per frame
    // Player uses the left mouse button to perform a melee attack,
    // Enemies take damage if they are hit.
    // if they hit the sphere at the end of the level it completes the level. 
    void Update()
    {

        time += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && time >= katana.MeleeCooldown)
        {           
            Attack();           
        }
    }

    void Attack()
    {

        action.Play("MeleeAnimation");

        Ray ray = player.ScreenPointToRay(Input.mousePosition);

        RaycastHit attack;

        if (Physics.Raycast(ray, out attack, katana.range))
        {
            if (attack.collider.tag == "Enemy")
            {
                EnemyHealth health = attack.collider.GetComponent<EnemyHealth>();
                health.Health(katana.damage);
            }

            if (attack.collider.tag == "WinGame")
            {
                FindObjectOfType<GameManager>().EndGame();
            }
        }

        action.Play("Idle");
    }
}
