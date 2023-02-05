using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    public float StartingSpeed;
    public float Acceleration;
    [SerializeField]
    private World world;

    private void Start()
    {
        StartTrain();
    }

    public void StartTrain()
    {
        StartCoroutine(StartTrainRoutine());
    }

    private IEnumerator StartTrainRoutine()
    {
        while(world.Speed < 2)
        {
            world.Speed += 1 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while(world.Speed < StartingSpeed)
        {
            world.Speed *= 1 + (Acceleration * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopTrain()
    {
        StartCoroutine(StopTrainRoutine());
    }

    private IEnumerator StopTrainRoutine()
    {
        while (world.Speed > 0)
        {
            world.Speed -= (Acceleration / 2.0f) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        world.Speed = 0;
    }
}
