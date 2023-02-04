using StarterAssets;
using System.Collections;
using System.Collections.Generic;
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

    private ThirdPersonController playerController;
    private StarterAssetsInputs inputs;
    private PlayerInput playerInput;
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

    private bool canFire = true;

    private void Start()
    {
        mainCam = Camera.main;
        playerController = FindFirstObjectByType<ThirdPersonController>();
        inputs = FindFirstObjectByType<StarterAssetsInputs>();
        playerInput = FindFirstObjectByType<PlayerInput>();
        animator = GetComponent<Animator>();
        fireClip = GetComponent<AudioSource>();

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
    }

    public void StopInteraction()
    {
        interaction.gameObject.SetActive(true);
        mainCam.enabled = true;
        canonCam.enabled = false;
        playerController.CanMove = true;
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
        if (canFire == false) return;
        canFire = false;
        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        // fire
        fireClip.Play();

        // particles
        muzzleParticle.Play();

        // animation
        animator.SetTrigger("Fire");

        // wait a sec
        yield return new WaitForSeconds(0.2f);

        GameObject shotObject = Instantiate(CarrotPrefab, shootPoint.position, rotatePivot.rotation);
        Rigidbody rigidbody = shotObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.AddForce(rotatePivot.forward * ShootForce, ForceMode.Impulse);

        // cooldown
        yield return new WaitForSeconds(CooldownTime);
        canFire = true;
    }
}
