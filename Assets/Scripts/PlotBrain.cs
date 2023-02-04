using System;
using System.Collections;
using System.Collections.Generic;
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

    public int plotCost = 0;
    public float secondsPerVeggie = 4f;

    public PlotAnimBrain AnimBrain;
    public Transform VeggiePool;

    public GameObject VeggiePrefab;

    #endregion

    #region State

    private float growProgress = 0f;
    private Player playerQuickRef;
    
    public PlotBrainState state = PlotBrainState.locked;

    #endregion

    // ^ vars - funcs v //

    #region Setup

    void OnEnable()
    {
        playerQuickRef = FindFirstObjectByType<Player>();
        InvokeRepeating(nameof(SlowUpdate), timeBetweenUpdates, timeBetweenUpdates);
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
                state = (playerQuickRef.Money >= plotCost) ? PlotBrainState.ready : PlotBrainState.locked;
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

        if (state == PlotBrainState.growing && VeggiePool.childCount >= 20) state = PlotBrainState.stuck;
        if (state == PlotBrainState.stuck && VeggiePool.childCount < 20) state = PlotBrainState.growing;

        if (growProgress >= 1f)
        {
            SpawnVeggie();
            growProgress -= 1f;
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
            AnimBrain.CurrentState = growProgress switch
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
        if (playerQuickRef && playerQuickRef.Money >= plotCost )
        {
            playerQuickRef.Money -= plotCost;
            state = PlotBrainState.growing;

            var interact = GetComponentInChildren<Interactable>();
            if (interact)
            {
                interact.gameObject.SetActive(false);
            }
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


    #endregion
}
