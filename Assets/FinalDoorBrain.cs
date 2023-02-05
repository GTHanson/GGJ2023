using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoorBrain : MonoBehaviour
{
    private Player playerQuickRef;

    void OnEnable()
    {
        playerQuickRef = FindFirstObjectByType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ThirdPersonController>(out var player))
        {
            //get player
            var playerController = FindFirstObjectByType<ThirdPersonController>();

            if (playerController)
            {
                //tell player to go to eastcar.westentrypoint
                playerController.JumpToPoint(transform.position - new Vector3(5,-2,0), 5f, 1f);
                //trainBrain.SendCameraToPoint(WestCar.CameraTransform.position);
                Invoke(nameof(LoadHappyHome), 1f);
            }
        }
    }

    void LoadHappyHome()
    {
        SceneManager.LoadScene("HappyHome");
    }
}
