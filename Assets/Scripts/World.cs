using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public float Speed;

    [SerializeField]
    private Transform StartPosition;
    [SerializeField]
    private Transform LoopPosition;

    [SerializeField]
    private List<Transform> sections;


    private void Update()
    {
        foreach (Transform section in sections)
        {
            section.transform.position = Vector3.MoveTowards(section.transform.position, LoopPosition.position, Speed * Time.deltaTime);
            if (Vector3.Distance(section.transform.position, LoopPosition.position) < 0.001f)
            {
                section.transform.position = StartPosition.transform.position;
            }
        }
 
    }
}
