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
    private Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = FindFirstObjectByType<Player>();
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(MovementLoop());
        RandomAnim();
    }

    private IEnumerator MovementLoop()
    {
        yield break;
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
            rb.isKinematic = false;
            rb.AddForce(collision.relativeVelocity * 10, ForceMode.Impulse);

            Instantiate(hitParticlesPrefab, collision.GetContact(0).point, Quaternion.identity);

            // veggies dont have gravity until they hit 
            collision.rigidbody.useGravity = true;

            // veg
            VeggieBase veg = collision.transform.GetComponentInChildren<VeggieBase>();
            player.Money += veg.Value;
        }
    }

    private void RandomAnim()
    {
        StartCoroutine(AnimTime());
        int randomNumber = Random.Range(1, 4);
        anim.SetTrigger("anim" + randomNumber);
    }

    private IEnumerator AnimTime()
    {
        int randomNumber = Random.Range(4, 10);
        yield return new WaitForSeconds((float)randomNumber);
        RandomAnim();
    }
}
