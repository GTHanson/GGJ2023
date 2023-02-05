using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrain : MonoBehaviour
{
    [SerializeField]
    private Transform StartPosition;
    [SerializeField]
    private Transform LoopPosition;
    [SerializeField]
    private float Speed;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, LoopPosition.position, Speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, LoopPosition.position) < 0.001f)
        {
            transform.position = StartPosition.transform.position;
        }
    }
}
