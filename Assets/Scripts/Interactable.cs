using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class InteractableManager
{
    public static Transform PlayerTransform;
    private static List<Interactable> interactables = new List<Interactable>();

    public static void AddInteractable(Interactable interactable)
    {
        interactables.Add(interactable);
    }

    // called in player
    public static void Update()
    {
        // find closest interactable
        Interactable closest = interactables[0];
        foreach (Interactable interactable in interactables)
        {
            //if(Vector3.Distance(closest.transform.position, ))
        }
    }
}

public class Interactable : MonoBehaviour
{
    public UnityEvent OnInteract;

    
}
