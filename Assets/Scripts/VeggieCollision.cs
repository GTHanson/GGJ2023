using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggieCollision : MonoBehaviour
{
    [SerializeField]
    private VeggieBase vegBase;
    private const int worldLayer = 16;

    private void OnCollisionEnter(Collision collision)
    {
        if (vegBase.pickedUp == false && collision.gameObject.layer == worldLayer)
        {
            GetComponent<Rigidbody>().useGravity = true;
            TrailRenderer tr = GetComponentInChildren<TrailRenderer>();
            if (tr)
            {
                tr.enabled = false;
                transform.SetParent(collision.transform);
            }
        }
    }
}
