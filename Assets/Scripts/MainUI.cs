using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText;
    [SerializeField]
    private TextMeshProUGUI carrotText;
    [SerializeField]
    private TextMeshProUGUI onionText;
    [SerializeField]
    private TextMeshProUGUI turnipText;


    public void UpdateMoney(int newMoney)
    {
        moneyText.text = "$" + newMoney;
    }

    public void UpdateResource(PickupTypes type, int newAmount)
    {
        switch (type)
        {
            case PickupTypes.Carrot:
                carrotText.text = "" + newAmount;
                break;
            case PickupTypes.Onion:
                onionText.text = "" + newAmount;
                break;
            case PickupTypes.Turnip:
                turnipText.text = "" + newAmount;
                break;
        }
    }
}
