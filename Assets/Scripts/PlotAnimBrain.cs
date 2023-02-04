using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotAnimBrain : MonoBehaviour
{
    public enum PlotState
    {
        Locked,
        Ready,
        Dirt,
        Baby,
        Gettingthere,
        Soontm,
    }

    public PlotState CurrentState
    {
        get => internalPlotState;
        set
        {
            SetModel(value);
            internalPlotState = value;
        }
    }
    private PlotState internalPlotState = PlotState.Ready;

    [SerializeField]
    private GameObject LockedButton;

    [SerializeField] private GameObject ReadyButton;
    [SerializeField]
    private GameObject Dirt;
    [SerializeField]
    private GameObject Baby;
    [SerializeField]
    private GameObject Gettingthere;
    [SerializeField]
    private GameObject Soontm;

    private void SetModel(PlotState state)
    {
        switch (state)
        {
            case PlotState.Locked:
                LockedButton.SetActive(true);
                ReadyButton.SetActive(false);
                Dirt.SetActive(false);
                Baby.SetActive(false);
                Gettingthere.SetActive(false);
                Soontm.SetActive(false);
            break;
            case PlotState.Ready:
                LockedButton.SetActive(false);
                ReadyButton.SetActive(true);
                Dirt.SetActive(false);
                Baby.SetActive(false);
                Gettingthere.SetActive(false);
                Soontm.SetActive(false);
                break;
            case PlotState.Dirt:
                LockedButton.SetActive(false);
                ReadyButton.SetActive(false);
                Dirt.SetActive(true);
                Baby.SetActive(false);
                Gettingthere.SetActive(false);
                Soontm.SetActive(false);
            break;
            case PlotState.Baby:
                LockedButton.SetActive(false);
                ReadyButton.SetActive(false);
                Dirt.SetActive(true);
                Baby.SetActive(true);
                Gettingthere.SetActive(false);
                Soontm.SetActive(false);
            break;
            case PlotState.Gettingthere:
                LockedButton.SetActive(false);
                ReadyButton.SetActive(false);
                Dirt.SetActive(true);
                Baby.SetActive(false);
                Gettingthere.SetActive(true);
                Soontm.SetActive(false);
            break;
            case PlotState.Soontm:
                LockedButton.SetActive(false);
                ReadyButton.SetActive(false);
                Dirt.SetActive(true);
                Baby.SetActive(false);
                Gettingthere.SetActive(false);
                Soontm.SetActive(true);
            break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}
