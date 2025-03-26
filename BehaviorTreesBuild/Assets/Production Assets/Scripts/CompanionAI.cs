using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : MonoBehaviour
{
    public Transform player;
    public float followDistance = 3f;
    public float attackRange = 2f;
    public float detectionRadius = 10f;
    public LayerMask enemyLayer;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isInCombat = false;
    private Transform currentEnemy;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isInCombat)
        {
            AttackEnemy();
        }
        else
        {
            FollowPlayer();
            DetectCombat();
        }

        UpdateAnimations();
    }

    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > followDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }

    void DetectCombat()
    {
        Collider[] enemies = Physics.OverlapSphere(player.position, detectionRadius, enemyLayer);
        if (enemies.Length > 0)
        {
            isInCombat = true;
            currentEnemy = enemies[0].transform;
        }
    }

    void AttackEnemy()
    {
        if (currentEnemy == null)
        {
            isInCombat = false;
            return;
        }

        float distanceToEnemy = Vector3.Distance(transform.position, currentEnemy.position);
        if (distanceToEnemy > attackRange)
        {
            agent.SetDestination(currentEnemy.position);
        }
        else
        {
            agent.SetDestination(transform.position);
            animator.SetTrigger("Attack"); // Attack animation trigger
        }
    }

    void UpdateAnimations()
    {
        if (isInCombat)
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", agent.velocity.magnitude > 0.1f);
        }
    }
}