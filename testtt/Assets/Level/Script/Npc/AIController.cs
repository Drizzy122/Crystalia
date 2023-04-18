using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{ 
    public float roamRadius = 10f; // Radius within which the Enemy Skeleton can roam
    public float minRoamTime = 3f; // Minimum time for which the Enemy Skeleton will roam
    public float maxRoamTime = 6f; // Maximum time for which the Enemy Skeleton will roam
    public float idleTime = 2f; // Time for which the Enemy Skeleton will idle between roaming
    public Animator animator; // Animator component for animations
    public Transform[] waypoints; // Waypoints for the Enemy Skeleton to follow
    private int currentWaypoint = 0; // Current waypoint that the Enemy Skeleton is heading towards
    private NavMeshAgent navMeshAgent; // Nav Mesh Agent component for navigation

    private float nextActionTime = 0f; // Time for next action (roaming/idling)

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.autoBraking = false;
        animator.SetBool("IsWalking", true);
        GoToNextWaypoint();
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            return;
        }
        navMeshAgent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + Random.Range(minRoamTime, maxRoamTime);
            if (navMeshAgent.remainingDistance < 0.5f)
            {
                animator.SetBool("IsWalking", false);
                Invoke("Idle", idleTime);
            }
            else
            {
                animator.SetBool("IsWalking", true);
            }
        }
    }

    void Idle()
    {
        animator.SetBool("IsWalking", false);
        Invoke("Roam", idleTime);
    }

    void Roam()
    {
        animator.SetBool("IsWalking", true);
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomDirection, out navMeshHit, roamRadius, -1);
        navMeshAgent.SetDestination(navMeshHit.position);
    }

    void OnDrawGizmosSelected()
    {
        //Roam Radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
        if (waypoints.Length > 1)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }
    }
}
