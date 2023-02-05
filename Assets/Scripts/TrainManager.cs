using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    public float MaxSpeed;
    public float Acceleration;
    public float Decelleration;
    [SerializeField]
    private World world;
    [SerializeField]
    private Player player;
    private float realMaxSpeed;
    private float goToSpeed = 0;
    private float timeSinceLastUpgrade = -30;

    private void Start()
    {
        StartTrain();
        realMaxSpeed = MaxSpeed;
        MaxSpeed *= 0.25f;
        goToSpeed = MaxSpeed;
    }

    public void StartTrain()
    {
        StartCoroutine(StartTrainRoutine());
    }

    private IEnumerator StartTrainRoutine()
    {
        while (gameObject)
        {
            if (player.Money > 50)
                MaxSpeed = realMaxSpeed;
            while (Time.time - timeSinceLastUpgrade > 30 && world.Speed < MaxSpeed)
            {
                world.Speed += Acceleration * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private Coroutine trainRoutine = null;
    public void SlowTrain()
    {
        if (trainRoutine != null) return;
        goToSpeed = 3;
        if (player.Money > 50)
            goToSpeed = 6;
        trainRoutine = StartCoroutine(SlowTrainRoutine());
    }

    private IEnumerator SlowTrainRoutine()
    {
        while (world.Speed > goToSpeed)
        {
            world.Speed -= Decelleration * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timeSinceLastUpgrade = Time.time;
        world.Speed = goToSpeed;
        trainRoutine = null;
    }
}
