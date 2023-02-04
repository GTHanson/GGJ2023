using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TrainBrain : MonoBehaviour
{
    public Transform CameraTransform;

    private void OnEnable()
    {
        if (CameraTransform == null)
        {
            Debug.LogError("Camera missing on train brain");
        }
    }

    public void SendCameraToPoint(Vector3 position)
    {
        CameraTransform.DOMove(position, 1f);
    }


}
