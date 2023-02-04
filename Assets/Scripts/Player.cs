using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private StarterAssetsInputs _input;

    public int Money
    {
        get => internalMoney;
        set
        {
            OnMoneySet(value);
            internalMoney = value;
        }
    }

    private int internalMoney = 0;

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

    private void OnMoneySet(int value)
    {

    }
}
