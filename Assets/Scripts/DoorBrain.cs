using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBrain : MonoBehaviour
{
    public int DoorCost = 10;

    public CarBrain ParentCarBrain;
    public bool EastDoor;

    public MeshRenderer ButtonGameObject;
    public Material RedMaterial;
    public Material GreenMaterial;
    public Collider TriggerCollider;
    private Player playerQuickRef;
    private AudioSource audioSource;

    void OnEnable()
    {
        playerQuickRef = FindFirstObjectByType<Player>();
        InvokeRepeating(nameof(SlowUpdate), timeBetweenUpdates, timeBetweenUpdates);
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private float timeBetweenUpdates = .1f;
    void SlowUpdate()
    {
        if (ButtonGameObject.gameObject.activeSelf)
        {
            ButtonGameObject.material = (playerQuickRef.Money - DoorCost) switch
            {
                < 0 => RedMaterial,
                >= 0 => GreenMaterial,
            };
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ThirdPersonController>(out var player))
        {
            if (EastDoor)
            {
                ParentCarBrain.PlayerTriggeredEastDoor();
            }
            else
            {
                ParentCarBrain.PlayerTriggeredWestDoor();
            }
        }
    }

    public void AttemptPurchase()
    {
        if (playerQuickRef && playerQuickRef.Money >= DoorCost)
        {
            playerQuickRef.Money -= DoorCost;

            Open();
        }
    }

    public void Open()
    {
        if (ParentCarBrain.EastCar == null && EastDoor) ParentCarBrain.LoadEastCar();
        if (ParentCarBrain.WestCar == null && !EastDoor) ParentCarBrain.LoadWestCar();

        TriggerCollider.enabled = true;

        ButtonGameObject.gameObject.SetActive(false);

        var interact = GetComponentInChildren<Interactable>();
        if (interact)
        {
            interact.gameObject.SetActive(false);
        }

        audioSource.Play();
    }
}
