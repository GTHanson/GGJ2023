using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [HideInInspector]
    public readonly int VeggieLayer = 12;

    [SerializeField]
    private GameObject hitParticlesPrefab;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Player player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = FindFirstObjectByType<Player>();
        StartCoroutine(MovementLoop());
    }

    private float speedLastTime = 0;
    private IEnumerator MovementLoop()
    {
        if(agent == null || agent.isOnNavMesh == false)
        {
            Destroy(gameObject);
            yield break;
        }

        if (speedLastTime == 0 && agent.velocity.magnitude == 0)
        {
            agent.SetDestination(RandomNavmeshLocation(Random.Range(200, 1000)));
        }

        speedLastTime = agent.velocity.magnitude;

        // wait half a second = 1 second of stopped time
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(MovementLoop());
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == VeggieLayer)
        {
            agent.enabled = false;
            rb.isKinematic = false;
            rb.AddForce(collision.relativeVelocity, ForceMode.Impulse);

            Instantiate(hitParticlesPrefab, collision.GetContact(0).point, Quaternion.identity);

            // veggies dont have gravity until they hit 
            collision.rigidbody.useGravity = true;

            // veg
            VeggieBase veg = collision.transform.GetComponentInChildren<VeggieBase>();
            player.Money += veg.Value;
        }
    }
}
