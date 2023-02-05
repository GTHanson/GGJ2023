using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NotificationBox : MonoBehaviour
{
    [SerializeField] private bool playOnStart;
    [SerializeField] private GameObject notificationMenu;
    [SerializeField] private TMP_Text textUI;
    [SerializeField] private string notificationMessage;
    [SerializeField] private GameObject continueText;
    private PlayerInput plut;
    private BoxCollider objectCollider;
    private bool openNotif = false;

    private void Awake()
    {
        plut = FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();
        objectCollider = GetComponent<BoxCollider>();
        if (playOnStart)
        {
            StartNotif();
        }
    }

    private void Update()
    {
        if (openNotif && (plut.actions["Start"].WasPressedThisFrame() || plut.actions["Interact"].WasPressedThisFrame()))
        {
            notificationMenu.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartNotif();
        }
    }

    public void StartNotif()
    {
        StartCoroutine(EnableNotification());
        StartCoroutine(ContinueTime());
    }

    private IEnumerator EnableNotification()
    {
        objectCollider.enabled = false;
        notificationMenu.SetActive(true);
        textUI.text = notificationMessage;
        yield return new WaitForSeconds(10f);
        continueText.SetActive(false);
        notificationMenu.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator ContinueTime()
    {
        string oldtext = textUI.text;
        yield return new WaitForSeconds(4f);
        if(textUI.text == oldtext)
        {
            openNotif = true;
            continueText.SetActive(true);
        }
    }
}
