using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcAIController : MonoBehaviour, IDataPersistence
{

    public float roamRadius = 10f; // the radius in which the AI can roam
    public float wanderDelay = 5f; // the delay between wandering to a new location
    public Animator animator; // the animator component for the AI
    public Transform[] waypoints; // an array of waypoints for the AI to follow

    private NavMeshAgent agent; // the NavMeshAgent component for the AI
    private Vector3 roamPosition; // the position the AI is currently roaming towards
    private int currentWaypoint = 0; // the index of the current waypoint the AI is heading towards
    private float wanderTimer = 0f; // the timer for the delay between wandering to a new location

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        roamPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
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
                roamPosition = hit.position;
                agent.SetDestination(roamPosition);
                animator.SetFloat("Speed", 1f); // play the walk animation
                wanderTimer = wanderDelay;
            }
            else
            {
                wanderTimer -= Time.deltaTime;
            }
        }
    }

    public void LoadData(GameData data)
    {
        
        this.transform.position = data.npcPosition;

    }
    public void SaveData(GameData data)
    {
        
        data.npcPosition = this.transform.position;

    }

    // Draw the roam radius as Gizmos
    private void OnDrawGizmosSelected()
    {
        // Draw the roam radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, roamRadius);

        // Draw the waypoints
        if (waypoints != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.25f);
                if (i < waypoints.Length - 1)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
            }
        }
    }
}