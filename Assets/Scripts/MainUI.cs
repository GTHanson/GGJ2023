using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText;

    public void UpdateMoney(int newMoney)
    {
        moneyText.text = "$" + newMoney;
    }
}
