using GlobalOutline;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum PickupTypes
{
    Other = 0,
    Carrot = 1,
    Onion = 2,
    Turnip = 3,
    Fuel = 4
}

public class Player : MonoBehaviour
{
    public float PickupFloatingRadius;
    public int MaxPickupPerType;
    [SerializeField]
    private StarterAssetsInputs _input;
    [SerializeField]
    private Transform pickupPoint;
    public Dictionary<PickupTypes, List<GameObject>> PickedUpObjs = new Dictionary<PickupTypes, List<GameObject>>();
    public MainUI mainUI;
    private OutlineManager outlineManager;
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
    private float currentTimeScale = 1f;
    private PlayerInput plut;

    // Start is called before the first frame update
    void Start()
    {
        InteractableManager.PlayerTransform = transform;
        mainUI = FindFirstObjectByType<MainUI>();
        outlineManager = FindFirstObjectByType<OutlineManager>();

        for (PickupTypes i = 0; i <= PickupTypes.Fuel; i++)
        {
            PickedUpObjs.Add(i, new List<GameObject>());
        }
        plut = GetComponent<PlayerInput>();
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
        if (plut.actions["Start"].WasPressedThisFrame())
        {
            currentTimeScale = Time.timeScale;
            if (currentTimeScale == 1f)
            {
                currentTimeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("TySettings", LoadSceneMode.Additive);
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                currentTimeScale = 1f;
                SceneManager.UnloadSceneAsync("TySettings");
                Cannon cann = FindObjectOfType<Cannon>();
                cann.MouseSensitivity = PlayerPrefs.GetFloat("sensitivity");
            }
            Time.timeScale = currentTimeScale;
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

    public bool CanPickup(PickupTypes type)
    {
        if (PickedUpObjs.ContainsKey(type) == false)
            return true;

        if (PickedUpObjs[type].Count >= MaxPickupPerType)
            return false;

        return true;
    }

    public void Pickup(GameObject pickup, PickupTypes type)
    {
        pickup.transform.SetParent(pickupPoint);

        Vector3 newPos = pickupPoint.transform.position;
        newPos += Random.onUnitSphere * PickupFloatingRadius;
        pickup.transform.position = newPos;

        PickedUpObjs[type].Add(pickup);

        VeggieBase veg = pickup.GetComponentInChildren<VeggieBase>();
        if (veg)
        {
            foreach (GameObject obj in veg.ModelObjs)
            {
                outlineManager.AddGameObject(obj);
            }

            mainUI.UpdateResource(type, PickedUpObjs[type].Count);
        }
    }
}
