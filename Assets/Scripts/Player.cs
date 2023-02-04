using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private StarterAssetsInputs _input;
    private MainUI mainUI;

    public int Money
    {
        get => internalMoney;
        set
        {
            internalMoney = value;
            OnMoneySet();
        }
    }

    private int internalMoney = 0;

    // Start is called before the first frame update
    void Start()
    {
        InteractableManager.PlayerTransform = transform;
        mainUI = FindFirstObjectByType<MainUI>();
    }


    private void Update()
    {
        InteractableManager.Update();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Money += 10;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Money += 100000000;
        }
    }

    public void Interact()
    {
        InteractableManager.Interact();
    }

    private void OnMoneySet()
    {
        mainUI.UpdateMoney(Money);
    }
}
