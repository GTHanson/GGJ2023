using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float Speed;
    [HideInInspector]
    public readonly int VeggieLayer = 12;

    [SerializeField]
    private GameObject hitParticlesPrefab;
    private Vector3 moveDirection = Vector3.forward;
    private CharacterController cc;
    private Rigidbody rb;
    private Player player;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        player = FindFirstObjectByType<Player>();
    }

    private void Update()
    {
        if (cc.enabled)
            cc.Move(moveDirection.normalized * (Speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == VeggieLayer)
        {
            cc.enabled = false;
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
