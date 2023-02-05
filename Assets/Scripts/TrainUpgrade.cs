using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrainUpgrade : MonoBehaviour
{
    private TrainManager trainManager;
    public enum CannonUpgradeState
    {
        locked,
        ready
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
    [SerializeField]
    private TextMeshPro worldText;

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
        trainManager = FindFirstObjectByType<TrainManager>();
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

            var interact = GetComponentInChildren<Interactable>();
            if (interact)
            {
                worldText.text = "Slowing Down!";
                StartCoroutine(UpdateText());
            }

            trainManager.SlowTrain();

            if (audioSource)
                audioSource.Play();
        }
    }

    private IEnumerator UpdateText()
    {
        yield return new WaitForSeconds(0.5f);
        worldText.text = "Slow Down Train\r\nPurchase - $20 (E)";
    }
    #endregion

    #region Helper


    #endregion
}
