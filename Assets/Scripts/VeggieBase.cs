using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieBase : MonoBehaviour
{
    private const int PlayerLayer = 8;

    public PickupTypes Type;
    public GameObject Root;
    public List<GameObject> ModelObjs;

    [HideInInspector]
    public int Value;
    [SerializeField]
    private Rigidbody rb;
    private ParticleSystem particles;
    public bool pickedUp = false;

    private void OnValidate()
    {
        switch (Type)
        {
            case PickupTypes.Carrot:
                Value = 4;
                break;
            case PickupTypes.Onion:
                Value = 40;
                break;
            case PickupTypes.Turnip:
                Value = 100;
                break;
        }
    }

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == PlayerLayer && pickedUp == false)
        {
            Player player = other.GetComponentInParent<Player>();
            if (player.CanPickup(Type) == false) return;

            pickedUp = true;
            rb.isKinematic = true;
            player.Pickup(transform.parent.gameObject, Type);

            if (particles && Random.Range(0, 4) == 1)
            {
                particles.Play();
            }
        }
    }

    public void DisableRender()
    {
        foreach (GameObject obj in ModelObjs)
        {
            obj.GetComponent<Renderer>().enabled = false;
        }
    }

    public void EnableRender()
    {
        foreach (GameObject obj in ModelObjs)
        {
            obj.GetComponent<Renderer>().enabled = false;
        }
    }
}
