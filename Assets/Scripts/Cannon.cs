using CustomOutline;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private Interactable interaction;
    [SerializeField]
    private Camera canonCam;
    private Camera mainCam;

    [SerializeField]
    private Transform rotatePivot;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private ParticleSystem muzzleParticle;
    private Animator animator;
    private AudioSource fireClip;
    private GameMusicFade gameMusic;

    private ThirdPersonController playerController;
    private StarterAssetsInputs inputs;
    private PlayerInput playerInput;
    private Player player;
    bool interacting = false;

    [Header("Settings")]
    public float ShootForce;
    public float CooldownTime;

    [Header("Rotation Stuff")]
    public float MouseSensitivity;
    public float VerticalClampAngle;
    public float HoriztonalClampAngle;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    [Header("Veggie Prefabs")]
    public GameObject CarrotPrefab;
    public GameObject OnionPrefab;
    public GameObject TurnipPrefab;
    public GameObject VeggieTrailPrefab;

    private bool canFire = true;

    private void Start()
    {
        mainCam = Camera.main;
        playerController = FindFirstObjectByType<ThirdPersonController>();
        inputs = FindFirstObjectByType<StarterAssetsInputs>();
        playerInput = FindFirstObjectByType<PlayerInput>();
        animator = GetComponent<Animator>();
        fireClip = GetComponent<AudioSource>();
        player = FindFirstObjectByType<Player>();
        gameMusic = FindObjectOfType<GameMusicFade>();
        try
        {
            MouseSensitivity = PlayerPrefs.GetFloat("sensitivity");
        }
        catch
        {
            PlayerPrefs.SetFloat("sensitivity", 1f);
            MouseSensitivity = 1;
        }
            

        Vector3 rot = rotatePivot.transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    // called by event
    public void ToggleInteraction()
    {
        interacting = !interacting;

        if (interacting)
            StartInteraction();
        else
            StopInteraction();
    }

    public void StartInteraction()
    {
        interaction.gameObject.SetActive(false);
        mainCam.enabled = false;
        canonCam.enabled = true;
        playerController.CanMove = false;
        MouseSensitivity = PlayerPrefs.GetFloat("sensitivity");
        gameMusic.ToggleMusic(false);

        Outline[] outlines = FindObjectsOfType<Outline>();
        foreach (var o in outlines)
        {
            o.enabled = true;
        }
    }

    public void StopInteraction()
    {
        interaction.gameObject.SetActive(true);
        mainCam.enabled = true;
        canonCam.enabled = false;
        playerController.CanMove = true;
        gameMusic.ToggleMusic(true);

        Outline[] outlines = FindObjectsOfType<Outline>();
        foreach (var o in outlines)
        {
            o.enabled = false;
        }
    }

    private void Update()
    {
        if (interacting == false) return;

        if (playerInput.actions["Fire"].IsPressed())
        {
            Fire();
        }
    }

    private void LateUpdate()
    {
        if (interacting == false) return;

        // player is controlling cannon
        float mouseX = inputs.look.x;
        float mouseY = inputs.look.y;

        rotY += mouseX * MouseSensitivity * Time.deltaTime;
        rotX += mouseY * MouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -VerticalClampAngle, VerticalClampAngle);
        rotY = Mathf.Clamp(rotY, -HoriztonalClampAngle, HoriztonalClampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        rotatePivot.transform.localRotation = localRotation;

    }

    public void Fire()
    {
        if (canFire == false || Time.timeScale == 0) return;

        GameObject objectToShoot = null;

        void DeleteAndDestroy(PickupTypes type)
        {
            Destroy(player.PickedUpObjs[type].Last());
            player.PickedUpObjs[type].RemoveAt(player.PickedUpObjs[type].Count - 1);
            player.mainUI.UpdateResource(type, player.PickedUpObjs[type].Count);
        }

        // prioritize the higher value items
        if (player.PickedUpObjs[PickupTypes.Turnip].Count > 0)
        {
            objectToShoot = TurnipPrefab;
            DeleteAndDestroy(PickupTypes.Turnip);
        }
        else if (player.PickedUpObjs[PickupTypes.Onion].Count > 0)
        {
            objectToShoot = OnionPrefab;
            DeleteAndDestroy(PickupTypes.Onion);
        }
        else if (player.PickedUpObjs[PickupTypes.Carrot].Count > 0)
        {
            objectToShoot = CarrotPrefab;
            DeleteAndDestroy(PickupTypes.Carrot);
        }

        if (objectToShoot == null) return;

        canFire = false;
        StartCoroutine(FireRoutine(objectToShoot));
    }

    private IEnumerator FireRoutine(GameObject objPrefab)
    {
        // fire
        fireClip.Play();

        // particles
        muzzleParticle.Play();

        // animation
        animator.SetTrigger("Fire");

        GameObject shotObject = Instantiate(objPrefab, shootPoint.position, rotatePivot.rotation);
        Instantiate(VeggieTrailPrefab, shotObject.transform);

        Rigidbody rigidbody = shotObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.AddForce(rotatePivot.forward * ShootForce, ForceMode.Impulse);

        StartCoroutine(DestroyAfterTime(shotObject, 20));

        // cooldown
        yield return new WaitForSeconds(CooldownTime);
        canFire = true;
    }

    private IEnumerator DestroyAfterTime(GameObject obj, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(obj);
    }
}
