using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class DoorBrain : MonoBehaviour
{
    public CarBrain ParentCarBrain;
    public bool EastDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ThirdPersonController>(out var player))
        {
            if (EastDoor)
            {
                ParentCarBrain.PlayerTriggeredEastDoor();
            }
            else
            {
                ParentCarBrain.PlayerTriggeredWestDoor();
            }
        }
    }


}
