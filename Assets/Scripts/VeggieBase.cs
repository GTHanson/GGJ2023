using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieBase : MonoBehaviour
{
    public enum VeggieType
    {
        Carrot,
        Onion,
        Turnip
    }

    public VeggieType Type;
    public GameObject Root;

    [HideInInspector]
    public int Value;

    private void OnValidate()
    {
        switch(Type)
        {
            case VeggieType.Carrot:
                Value = 4;
                break;
            case VeggieType.Onion:
                Value = 40;
                break;
            case VeggieType.Turnip:
                Value = 100;
                break;
        }
    }
}
