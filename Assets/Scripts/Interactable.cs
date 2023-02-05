using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class InteractableManager
{
    public static Transform PlayerTransform;
    public static float InteractRange = 2;
    private static List<Interactable> interactables = new List<Interactable>();
    private static Interactable closest = null;

    public static void AddInteractable(Interactable interactable)
    {
        interactables.Add(interactable);
    }

    // called in player
    public static void Update()
    {
        // find closest interactable
        Interactable lastClosest = closest;
        float closestDistance = float.MaxValue;
        foreach (Interactable interactable in interactables)
        {
            float dist = Vector3.Distance(interactable.transform.position, PlayerTransform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = interactable;
            }
        }

        if (closestDistance > InteractRange)
        {
            closest.DisableInteraction();
            closest = null;
        }
        else if (closest != lastClosest)
        {
            closest.EnableInteraction();

            if (lastClosest != null)
                lastClosest.DisableInteraction();
        }
    }

    public static void Interact()
    {
        if (closest == null) return;
        closest.OnInteract.Invoke();
    }

    public static void Clear()
    {
        interactables.Clear();
    }
}

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;
    [SerializeField]
    private GameObject worldUI;
    private bool interactEnabled = false;

    private void Awake()
    {
        InteractableManager.AddInteractable(this);
    }

    private void OnDisable()
    {
        worldUI.SetActive(false);
    }

    private void OnEnable()
    {
        if (interactEnabled)
            worldUI.SetActive(true);
    }

    public void EnableInteraction()
    {
        interactEnabled = true;
        worldUI.SetActive(true);
    }

    public void DisableInteraction()
    {
        interactEnabled = false;
        worldUI.SetActive(false);
    }
}
