using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CannonUpgradeBrain : MonoBehaviour
{
    public enum CannonUpgradeState
    {
        locked,
        ready,
        purchased
    }

    #region Inspector Set

    public int plotCost = 0;
    public CannonAnimBrain AnimBrain;

    #endregion

    #region State

    private Player playerQuickRef;

    public CannonUpgradeState state = CannonUpgradeState.locked;

    #endregion

    private AudioSource audioSource;

    // ^ vars - funcs v //

    #region Setup

    void OnEnable()
    {
        playerQuickRef = FindFirstObjectByType<Player>();
        InvokeRepeating(nameof(SlowUpdate), timeBetweenUpdates, timeBetweenUpdates);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    #endregion

    #region Update

    private float timeBetweenUpdates = .1f;
    void SlowUpdate()
    {
        switch (state)
        {
            case CannonUpgradeState.locked:
            case CannonUpgradeState.ready:
                state = (playerQuickRef.Money >= plotCost) ? CannonUpgradeState.ready : CannonUpgradeState.locked;
                break;
            case CannonUpgradeState.purchased:
                if (audioSource == null) Destroy(gameObject);
                else if (audioSource.isPlaying == false) Destroy(gameObject);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        AnimBrain.CurrentState = state switch
        {
            CannonUpgradeState.locked => CannonAnimBrain.UpgradeState.Locked,
            CannonUpgradeState.ready => CannonAnimBrain.UpgradeState.Ready,
            _ => CannonAnimBrain.UpgradeState.Ready,
        };
    }

    #endregion

    #region Events

    public void AttemptPurchase()
    {
        if (playerQuickRef && playerQuickRef.Money >= plotCost)
        {
            playerQuickRef.Money -= plotCost;

            state = CannonUpgradeState.purchased;
            // move dont destroy
            transform.position = new Vector3(0, -1000, 0);

            var interact = GetComponentInChildren<Interactable>();
            if (interact)
            {
                interact.gameObject.SetActive(false);
            }

            Cannon c = FindObjectOfType<Cannon>();
            c.CooldownTime -= 0.5f;
            c.CooldownTime = Mathf.Clamp(c.CooldownTime, 0.01f, c.CooldownTime);
            c.ShootForce += 30;
            c.ShootForce = Mathf.Clamp(c.ShootForce, c.ShootForce, 200);

            if (audioSource)
                audioSource.Play();
        }
    }
    #endregion

    #region Helper


    #endregion
}
