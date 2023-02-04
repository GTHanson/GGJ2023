using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MaxwellDance : MonoBehaviour
{
    public Vector3 LeftVector3 = new Vector3(-.5f, 1, -1);
    public Vector3 RightVector3 = new Vector3(.5f, 1, -1);

    // Start is called before the first frame update
    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < 200; i++)
        {
            sequence.Append(transform.DORotate(LeftVector3, 0.4f));
            sequence.Append(transform.DORotate(RightVector3, 0.4f));
        }
    }
}
