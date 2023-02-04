using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class CarBrain : MonoBehaviour
{
    #region Inspector Set

    public Transform CameraTransform;

    [SerializeField]
    private Transform EastEntryPoint;
    [SerializeField]
    private Transform WestEntryPoint;

    public GameObject EastCarPrefab;
    [SerializeField]
    private bool LoadEastCarOnStart;

    public GameObject WestCarPrefab;
    [SerializeField]
    private bool LoadWestCarOnStart;

    #endregion

    #region State

    private CarBrain WestCar = null;
    private CarBrain EastCar = null;

    private TrainBrain trainBrain;

    #endregion

    // ^ vars - funcs v //

    #region Setup

    private void OnEnable()
    {
        trainBrain = FindFirstObjectByType<TrainBrain>();
    }

    private void Setup(CarBrain ParentCar, bool ParentToEast)
    {
        if (ParentToEast) EastCar = ParentCar;
        else WestCar = ParentCar;

    }

    private void Start()
    {
        if (LoadEastCarOnStart)
        {
            LoadEastCar();
        }

        if (LoadWestCarOnStart)
        {
            LoadWestCar();
        }
    }

    #endregion

    #region Update

    

    #endregion

    #region Interface

    public Vector3 GetEastEntryPosition()
    {
        return EastEntryPoint.position;
    }

    public Vector3 GetWestEntryPosition()
    {
        return WestEntryPoint.position;
    }

    #endregion

    #region Events

    public void PlayerTriggeredEastDoor()
    {
        if (EastCar == null) return;

        //get player
        var playerController = FindFirstObjectByType<ThirdPersonController>();

        if (playerController)
        {
            //tell player to go to eastcar.westentrypoint
            playerController.JumpToPoint(EastCar.GetWestEntryPosition(), 5f, 1f);
            trainBrain.SendCameraToPoint(EastCar.CameraTransform.position);
        }
    }

    public void PlayerTriggeredWestDoor()
    {
        if (WestCar == null) return;

        //get player
        var playerController = FindFirstObjectByType<ThirdPersonController>();

        if (playerController)
        {
            //tell player to go to eastcar.westentrypoint
            playerController.JumpToPoint(WestCar.GetEastEntryPosition(), 5f, 1f);
            trainBrain.SendCameraToPoint(WestCar.CameraTransform.position);
        }
    }

    #endregion

    #region Helper

    private void LoadEastCar()
    {
        if (EastCar != null) return;

        var carList = FindObjectsByType<CarBrain>(FindObjectsSortMode.None);
        if (carList.Length >= 50) return;

        var eastCarGameObject = Instantiate(EastCarPrefab, transform.position - new Vector3(-20, 0, 0), new Quaternion());
        eastCarGameObject.transform.parent = transform.parent;
        EastCar = eastCarGameObject.GetComponent<CarBrain>();
        EastCar.Setup(this, false);
    }

    private void LoadWestCar()
    {
        if (WestCar != null) return;

        var carList = FindObjectsByType<CarBrain>(FindObjectsSortMode.None);
        if (carList.Length >= 50) return;

        var westCarGameObject = Instantiate(WestCarPrefab, transform.position - new Vector3(20, 0, 0), new Quaternion());
        westCarGameObject.transform.parent = transform.parent;
        WestCar = westCarGameObject.GetComponent<CarBrain>();
        WestCar.Setup(this, false);
    }

    #endregion
}
