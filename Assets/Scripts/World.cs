using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public float Speed;

    private void FixedUpdate()
    {
        transform.position += transform.right * Speed;
    }
}
