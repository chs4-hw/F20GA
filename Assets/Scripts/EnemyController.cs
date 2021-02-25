using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyMode enemyMode = EnemyMode.wander;
    Transform target;
    NavMeshAgent agent;
    public LayerMask ground;
    public float lookRadius = 40f;
    public Vector3 walkPointRange;
    bool walkPointSet = false;
    public float walkRange = 10f;
    public float walkLife = 5f;
    float time = 0f;
    public float attackRadius = 4f;
    public float cooldownTime = 1f;
    public int damage = 10;
    bool attack;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    // FSM of the Enemy States
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        switch (enemyMode)
        {
            case EnemyMode.wander:
                Wandering();

                if (distance <= attackRadius)
                {
                    enemyMode = EnemyMode.attack;
                }
                if (distance <= lookRadius && distance >= attackRadius)
                {
                    enemyMode = EnemyMode.chase;
                }
                break;

            case EnemyMode.chase:
                Chase();
                if (distance <= attackRadius)
                {
                    enemyMode = EnemyMode.attack;
                }
                if (distance >= lookRadius)
                {
                    enemyMode = EnemyMode.wander;
                }
                break;

            case EnemyMode.attack:
                Attack();
                if (distance >= attackRadius && distance <= lookRadius)
                {
                    enemyMode = EnemyMode.chase;
                }
                if (distance >= lookRadius)
                {
                    enemyMode = EnemyMode.wander;
                }
                break;

            default:
                break;
        }
    }

    // Wandering

    void Wandering()
    {

        if (!walkPointSet) SearchPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPointRange);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPointRange;

        time += Time.deltaTime;

        if (time >= walkLife)
        {
            walkPointSet = false;
            time = 0f;

            // Debug.Log("AI Wandering");
        }

    }
    
    // AI Search for a point on the world to move to when in wander mode
    void SearchPoint()
    {
        float randomZ = Random.Range(-walkRange - agent.stoppingDistance, walkRange + agent.stoppingDistance);
        float randomX = Random.Range(-walkRange - agent.stoppingDistance, walkRange + agent.stoppingDistance);

        walkPointRange = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPointRange, -transform.up, 2f, ground))
        {
            walkPointSet = true;
        }
    }

    // Chase
    // Chase player when entering the look range
    void Chase()
    {
        agent.SetDestination(target.position);

        FacePlayer();

        // Debug.Log("AI Chase");
    }

    // Attack

    void Attack()
    {
        agent.ResetPath();

        FacePlayer();

        if (!attack)
        {
            attack = true;
            
            PlayerHealth health = target.GetComponent<Collider>().GetComponentInChildren<PlayerHealth>();
            health.TakeDamage(damage);

            Invoke(nameof(CoolDown), cooldownTime);
        }

        // Debug.Log("AI Attack");
    }


    // Enemy cooldown between attacks
    void CoolDown()
    {
        attack = false;
    }

    // Face Player when chasing and attacking
    void FacePlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 1, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Debug enemy sight and attack range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    // FSM states enum
    public enum EnemyMode
    {
        wander,
        chase,
        attack
    }
}
