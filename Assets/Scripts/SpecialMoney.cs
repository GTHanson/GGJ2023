using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMoney : MonoBehaviour
{
    private NotificationBox notifBox;
    private Player player;

    private void Start()
    {
        notifBox = GetComponent<NotificationBox>();
        player = FindFirstObjectByType<Player>();
    }
    void Update()
    {
        if(player.Money >= 50)
        {
            notifBox.StartNotif();
        }
    }
}
