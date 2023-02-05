using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAnimBrain : MonoBehaviour
{
    public enum UpgradeState
    {
        Locked,
        Ready
    }

    public UpgradeState CurrentState
    {
        get => internalPlotState;
        set
        {
            SetModel(value);
            internalPlotState = value;
        }
    }
    private UpgradeState internalPlotState = UpgradeState.Ready;

    [SerializeField]
    private GameObject LockedButton;

    [SerializeField] private GameObject ReadyButton;

    private void SetModel(UpgradeState state)
    {
        switch (state)
        {
            case UpgradeState.Locked:
                LockedButton.SetActive(true);
                ReadyButton.SetActive(false);
                break;
            case UpgradeState.Ready:
                LockedButton.SetActive(false);
                ReadyButton.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
