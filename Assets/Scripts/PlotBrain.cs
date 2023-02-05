using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlotBrain : MonoBehaviour
{
    public enum PlotBrainState
    {
        locked,
        ready,
        growing,
        stuck,
    }

    #region Inspector Set


    public UpgradeDetail InitialPurchase;
    public List<UpgradeDetail> upgradeList;

    public PlotAnimBrain AnimBrain;
    public Transform VeggiePool;

    public GameObject VeggiePrefab;

    public TextMeshPro upgradeText;

    #endregion

    #region State

    private float SecondsPerVeggie = 1000000f;
    private int VeggieCap = 0;

    private float growProgress = 0f;
    private Player playerQuickRef;
    
    public PlotBrainState state = PlotBrainState.locked;

    private UpgradeDetail nextUpgradeDetail;

    #endregion

    private AudioSource audioSource;

    // ^ vars - funcs v //

    #region Setup

    void OnEnable()
    {
        nextUpgradeDetail = InitialPurchase;
        upgradeText.text = "-" + nextUpgradeDetail.cost;
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
            case PlotBrainState.locked:
            case PlotBrainState.ready:
                state = (playerQuickRef.Money >= nextUpgradeDetail.cost) ? PlotBrainState.ready : PlotBrainState.locked;
                    break;
            case PlotBrainState.growing:
                growProgress += .1f;
                break;
            case PlotBrainState.stuck:
                growProgress = 0f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (state == PlotBrainState.growing && VeggiePool.childCount >= VeggieCap) state = PlotBrainState.stuck;
        if (state == PlotBrainState.stuck && VeggiePool.childCount < VeggieCap) state = PlotBrainState.growing;

        if (growProgress >= SecondsPerVeggie)
        {
            SpawnVeggie();
            growProgress -= SecondsPerVeggie;
        }

        AnimBrain.CurrentState = state switch
        {
            PlotBrainState.locked => PlotAnimBrain.PlotState.Locked,
            PlotBrainState.ready => PlotAnimBrain.PlotState.Ready,
            PlotBrainState.growing => PlotAnimBrain.PlotState.Dirt,
            PlotBrainState.stuck => PlotAnimBrain.PlotState.Dirt,
            _ => PlotAnimBrain.PlotState.Dirt
        };

        if (state == PlotBrainState.growing)
        {
            AnimBrain.CurrentState = (growProgress/SecondsPerVeggie) switch
            {
                < .25f => PlotAnimBrain.PlotState.Dirt,
                < .5f => PlotAnimBrain.PlotState.Baby,
                < .75f => PlotAnimBrain.PlotState.Gettingthere,
                < 1f => PlotAnimBrain.PlotState.Soontm,
                _ => PlotAnimBrain.PlotState.Dirt
            };
        }


    }

    #endregion

    #region Events

    public void AttemptPurchase()
    {
        if (playerQuickRef && nextUpgradeDetail!=null && playerQuickRef.Money >= nextUpgradeDetail.cost )
        {
            playerQuickRef.Money -= nextUpgradeDetail.cost;
            state = PlotBrainState.growing;

            Upgrade();

            var interact = GetComponentInChildren<Interactable>();
            if (interact && nextUpgradeDetail == null)
            {
                interact.gameObject.SetActive(false);
            }

            audioSource.Play();
        }
    }

    public void SpawnVeggie()
    {
        var veggie = Instantiate(VeggiePrefab, VeggiePool);
        if (veggie.TryGetComponent<Rigidbody>(out var VgRb))
        {
            VgRb.AddForce(Random.insideUnitSphere * 100f);
        }
    }

    #endregion

    #region Helper

    public void Upgrade()
    {
        if (nextUpgradeDetail == null) return;

        SecondsPerVeggie = nextUpgradeDetail.secondsPerVeggie;
        VeggieCap = nextUpgradeDetail.veggieCap;

        if (upgradeList.Count <= 0)
        {
            nextUpgradeDetail = null;
            upgradeText.text = "";
        }
        else
        {
            nextUpgradeDetail = upgradeList[0];
            upgradeList.RemoveAt(0);
            upgradeText.text = "-" + nextUpgradeDetail.cost;
        }
    }

    #endregion
}
