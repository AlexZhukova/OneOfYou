using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardAI : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints; // Patrol waypoints
    [SerializeField] private float sightRange = 8f;    // Detection distance
    [SerializeField] private float attackRange = 1.5f; // Attack distance
    [SerializeField] private float searchTime = 3f;    // Seconds to linger where the player vanished
    [SerializeField] private float idleTime = 2f;      // Seconds to stay idle
    [SerializeField] private Transform deatharea; // Point to reset the player position after idle
    [SerializeField] public TextMeshProUGUI dialogueText; // Reference to the TextMeshProUGUI component for dialogue display
    //[SerializeField] public RawImage dialogueWindow; // Reference to the RawImage component for the dialogue window
    [SerializeField] public TextMeshProUGUI timerText; 

    private enum Mode { Patrol, Chase, Search, Idle, Wander }
    [SerializeField] private Mode mode = Mode.Patrol;

    private NavMeshAgent agent;
    private Transform player;
    private int patrolIndex = 0;
    private Vector3 lastKnownPosition;
    private float searchTimer;
    private float idleTimer;
    private Animator animator;
    public float stoppingDistance;
    public bool playerDead = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

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
                    animator.SetBool("Patrol", true);
                    animator.SetBool("Chase", false);
                    animator.SetBool("Search", false);
                    animator.SetBool("Idle", false);
                }
                break;

            case Mode.Chase:
                if (canSee)
                {
                    // Keep updating "last known position" while we can still see them
                    lastKnownPosition = player.position;
                    agent.stoppingDistance = attackRange;
                    agent.SetDestination(player.position);
                    animator.SetBool("Patrol", false);
                    animator.SetBool("Chase", true);
                    animator.SetBool("Search", false);
                    animator.SetBool("Idle", false);
                    if (Vector3.Distance(transform.position, player.position) <= attackRange)
                    {
                        // Attack logic here
                        Debug.Log("Dialogue with the player!");
                        mode = Mode.Idle;
                    }
                }
                else
                {
                    // Lost them -> head to the last known position and start searching
                    agent.stoppingDistance = stoppingDistance;
                    agent.SetDestination(lastKnownPosition);
                    searchTimer = searchTime;
                    mode = Mode.Search;
                    animator.SetBool("Patrol", false);
                    animator.SetBool("Chase", false);
                    animator.SetBool("Search", true);
                    animator.SetBool("Idle", false);
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
            case Mode.Idle:
                animator.SetBool("Patrol", false);
                animator.SetBool("Chase", false);
                animator.SetBool("Search", false);
                animator.SetBool("Idle", true);
                timerText.GetComponent<TextMeshProUGUI>().enabled = true;
                dialogueText.GetComponent<TextMeshProUGUI>().enabled = true;
                //dialogueWindow.GetComponent<RawImage>().enabled = true;
                Time.timeScale = 0f; // Pause the game
                idleTimer += Time.unscaledDeltaTime; // Use unscaledDeltaTime to count time while the game is paused
                timerText.text = idleTimer.ToString("F2"); // Display the idle timer with 2 decimal places
                if (idleTimer >= idleTime && playerDead == true)
                {
                    Time.timeScale = 1f; // Resume the game
                    idleTimer = 0f;
                    timerText.GetComponent<TextMeshProUGUI>().enabled = false;
                    dialogueText.GetComponent<TextMeshProUGUI>().enabled = false;

                    // Disable CharacterController to prevent it from interfering with teleport
                    CharacterController characterController = player.GetComponent<CharacterController>();
                    if (characterController != null) characterController.enabled = false;

                    // Teleport player
                    player.position = deatharea.position;

                    // Re-enable CharacterController
                    if (characterController != null) characterController.enabled = true;

                    agent.stoppingDistance = 0.1f;
                    mode = Mode.Patrol;
                }
                if (idleTimer >= idleTime && playerDead == false)
                {
                    Time.timeScale = 1f; // Resume the game
                    idleTimer = 0f;
                    agent.stoppingDistance = stoppingDistance; // Reset stopping distance before returning to wander
                    mode = Mode.Wander; // Switch to Wander mode after idle
                    timerText.GetComponent<TextMeshProUGUI>().enabled = false;
                    dialogueText.GetComponent<TextMeshProUGUI>().enabled = false;
                    //dialogueWindow.GetComponent<RawImage>().enabled = false;
                }
                break;
            case Mode.Wander:
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length; // Patrol points are used for wandering as well, doesn't see the player
                agent.SetDestination(patrolPoints[patrolIndex].position);
                animator.SetBool("Patrol", true);
                animator.SetBool("Chase", false);
                animator.SetBool("Search", false);
                animator.SetBool("Idle", false);
                break;  
        }
    }
}