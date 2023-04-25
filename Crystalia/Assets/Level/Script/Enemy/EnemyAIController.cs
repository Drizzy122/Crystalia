using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    [Header ("Roam and seek")]
    //Roam and seek Variables
    public float roamRadius = 10f; // the radius in which the AI can roam
    public float seekRadius = 20f; // the radius in which the AI will start seeking the player
    public float wanderDelay = 5f; // the delay between wandering to a new location
    private float wanderTimer = 0f; // the timer for the delay between wandering to a new location
    private bool isRunning = false;

    [Header("Waypoints")]
    private int currentWaypoint = 0; // the index of the current waypoint the AI is heading towards
    public Transform[] waypoints; // an array of waypoints for the AI to follow

    [Header("Attack and Damage")]
    //Attack and damage Variables
    public float attackRadius = 2f; // the radius in which the AI will attack the player
    [SerializeField] private float damage; // the amount of damage the AI will do to the player
    public float attackDelay = 5f; // the delay between attacks
    [SerializeField] private float attackTimer = 0f; // the timer for the delay between attacks
    

    [Header("AI Components")]
    public Animator animator; // the animator component for the AI
    private NavMeshAgent agent; // the NavMeshAgent component for the AI
    private Transform player; // the transform of the player
    private AIState state = AIState.Roaming;
 
    private enum AIState
    {
        Roaming,
        Seeking,
        Attacking,
        Dead,
    }
    public void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator.SetBool("isRunning", false);
    }

    void Update()
    {
        switch (state)
        {
            case AIState.Roaming:
                Roam();
                break;

            case AIState.Seeking:
                Seek();
                break;

            case AIState.Attacking:
                Attack();
                break;
        }

        attackTimer -= Time.deltaTime;

        if (state != AIState.Attacking && Vector3.Distance(transform.position, player.position) <= attackRadius)
        {
            state = AIState.Attacking;
            //attackTimer = 0f; // reset the attack timer
        }
        else if (state != AIState.Seeking && Vector3.Distance(transform.position, player.position) > attackRadius && Vector3.Distance(transform.position, player.position) <= seekRadius)
        {
            state = AIState.Seeking;
        }
        else if (state != AIState.Roaming && Vector3.Distance(transform.position, player.position) > seekRadius)
        {
            state = AIState.Roaming;
        }
    }


    public void Roam()
    {
        if (waypoints.Length > 0) // if there are waypoints
        {
            if (agent.remainingDistance < 0.5f) // if the AI has reached the current waypoint
            {
                currentWaypoint = Random.Range(0, waypoints.Length); // choose a new waypoint at random
                agent.SetDestination(waypoints[currentWaypoint].position); // set the AI's destination to the new waypoint
                animator.SetFloat("Speed", 1f); // play the walk animation
            }
        }
        else // if there are no waypoints
        {
            if (wanderTimer <= 0f)
            {
                Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
                Vector3 finalPosition = hit.position;
                agent.SetDestination(finalPosition);
                animator.SetFloat("Speed", 1f); // play the walk animation
                                                //isRunning = false;
                wanderTimer = wanderDelay;
            }
            else
            {
                wanderTimer -= Time.deltaTime;
            }

            if (Vector3.Distance(transform.position, player.position) < seekRadius)
            {
                state = AIState.Seeking;
            }
        }
    }

    public void Seek()
    {
        agent.SetDestination(player.position);
        animator.SetFloat("Speed", 0f); // stop the walk animation
        isRunning = true; // play the run animation
        animator.SetBool("isRunning", isRunning); // set the "isRunning" parameter of the animator

        if (Vector3.Distance(transform.position, player.position) < attackRadius)
        {
            state = AIState.Attacking;
            isRunning = false; // stop the run animation
            animator.SetBool("isRunning", isRunning); // set the "isRunning" parameter of the animator
        }
        else if (Vector3.Distance(transform.position, player.position) > seekRadius)
        {
            state = AIState.Roaming;
            isRunning = false; // stop the run animation
            animator.SetBool("isRunning", isRunning); // set the "isRunning" parameter of the animator
        }
    }


    public void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (attackTimer <= 0f)
        {
            animator.SetBool("isAttacking", true); // set the "isAttacking" parameter of the animator
            player.GetComponent<PlayerHealth>().TakeDamage(damage); // deal damage to the player
            attackTimer = attackDelay;
        }
 
        if (Vector3.Distance(transform.position, player.position) > attackRadius)
        {
            animator.SetBool("isAttacking", false); // set the "isAttacking" parameter of the animator to false
            state = AIState.Seeking;
        }

        animator.SetFloat("Speed", 0f); // stop the walk animation
        isRunning = false; // stop the run animation
    }

    

    // Draw the roam and seek radius as Gizmos
    private void OnDrawGizmosSelected()
    {
        // Draw the roam radius
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, roamRadius);

        // Draw the seek radius
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, seekRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}

