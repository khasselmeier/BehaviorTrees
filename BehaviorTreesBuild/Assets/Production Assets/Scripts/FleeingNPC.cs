using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class FleeingNPC : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float fleeDistance = 10f;
    public float wanderRadius = 5f;
    public float idleTime = 2f;
    public LayerMask threatLayer;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isFleeing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(Wander());
    }

    void Update()
    {
        if (!isFleeing)
        {
            DetectThreat();
        }

        UpdateAnimations();
    }

    void DetectThreat()
    {
        Collider[] threats = Physics.OverlapSphere(transform.position, detectionRadius, threatLayer);
        if (threats.Length > 0)
        {
            Transform nearestThreat = threats[0].transform;
            Vector3 fleeDirection = (transform.position - nearestThreat.position).normalized;
            Vector3 fleeTarget = transform.position + (fleeDirection * fleeDistance);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
            {
                StopAllCoroutines();
                StartCoroutine(Flee(hit.position));
            }
        }
    }

    IEnumerator Flee(Vector3 fleeLocation)
    {
        isFleeing = true;
        agent.SetDestination(fleeLocation);

        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(idleTime);
        isFleeing = false;
        StartCoroutine(Wander());
    }

    IEnumerator Wander()
    {
        while (!isFleeing)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            yield return new WaitForSeconds(idleTime);
        }
    }

    void UpdateAnimations()
    {
        animator.SetBool("IsWalking", !isFleeing && agent.velocity.magnitude > 0.1f);
        animator.SetBool("IsRunning", isFleeing);
    }
}