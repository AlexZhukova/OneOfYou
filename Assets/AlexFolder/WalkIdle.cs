using UnityEngine;
using UnityEngine.AI; // Required for NavMeshAgent

public class CompanionAnimationController : MonoBehaviour
{
    private Animator animator;   // Reference to Animator component
    private NavMeshAgent agent;  // Reference to NavMeshAgent

    void Start()
    {
        animator = GetComponent<Animator>();  // Get the Animator component
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent
    }

    void Update()
    {
        // Get the current speed of the NavMeshAgent
        float Speed = agent.velocity.magnitude;

        if (Speed < 0.1f)
        {
            // Idle animation
            animator.SetFloat("Speed", 0);
        }
        else
        {
            // Walking animation
            animator.SetFloat("Speed", 1);
        }
    }
}