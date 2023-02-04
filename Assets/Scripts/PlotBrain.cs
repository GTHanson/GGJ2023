using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotBrain : MonoBehaviour
{
    private enum PlotBrainState
    {
        locked,
        purcasable,
        growing,
        stuck,
    }

    #region Inspector Set

    public int plotCost = 0;

    public PlotAnimBrain AnimBrain;

    #endregion

    #region State

    private float growProgress = 0f;
    private Player playerQuickRef;
    private PlotBrainState state = PlotBrainState.locked;

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
            case PlotBrainState.purcasable:
                state = (playerQuickRef.Money >= plotCost) ? PlotBrainState.purcasable : PlotBrainState.locked;
                    break;
            case PlotBrainState.growing:
                break;
            case PlotBrainState.stuck:
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
        }
    }

    #endregion

    #region Helper


    #endregion
}
