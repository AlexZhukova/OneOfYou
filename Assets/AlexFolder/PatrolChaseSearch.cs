using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardAI : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints; // Patrol waypoints
    [SerializeField] private float sightRange = 8f;    // Detection distance
    [SerializeField] private float attackRange = 1.5f; // Attack distance
    [SerializeField] private float searchTime = 3f;    // Seconds to linger where the player vanished

    private enum Mode { Patrol, Chase, Search }
    private Mode mode = Mode.Patrol;

    private NavMeshAgent agent;
    private Transform player;
    private int patrolIndex = 0;
    private Vector3 lastKnownPosition;
    private float searchTimer;
    public float stoppingDistance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Don't crash when no Player is placed (tag search can return null)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    void Update()
    {
        if (player == null || patrolPoints.Length == 0) return;

        bool canSee = Vector3.Distance(transform.position, player.position) <= sightRange;
        bool arrived = !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

        switch (mode)
        {
            case Mode.Patrol:
                if (canSee) { mode = Mode.Chase; break; }
                if (arrived)
                {
                    patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                    agent.SetDestination(patrolPoints[patrolIndex].position);
                }
                break;

            case Mode.Chase:
                if (canSee)
                {
                    // Keep updating "last known position" while we can still see them
                    lastKnownPosition = player.position;
                    agent.stoppingDistance = attackRange;
                    agent.SetDestination(player.position);
                }
                else
                {
                    // Lost them -> head to the last known position and start searching
                    agent.stoppingDistance = stoppingDistance;
                    agent.SetDestination(lastKnownPosition);
                    searchTimer = searchTime;
                    mode = Mode.Search;
                }
                break;

            case Mode.Search:
                if (canSee) { mode = Mode.Chase; break; }
                if (arrived) searchTimer -= Time.deltaTime;  // Count down only after reaching the spot
                if (searchTimer <= 0f)
                {
                    agent.SetDestination(patrolPoints[patrolIndex].position);
                    mode = Mode.Patrol;
                }
                break;
        }
    }
}