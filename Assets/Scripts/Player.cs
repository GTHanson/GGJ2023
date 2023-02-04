using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private StarterAssetsInputs _input;
    // Start is called before the first frame update
    void Start()
    {
        InteractableManager.PlayerTransform = transform;
    }


    private void Update()
    {
        InteractableManager.Update();
    }

    public void Interact()
    {
        InteractableManager.Interact();
    }
}
