using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    private static TrafficManager singleton;
    public static TrafficManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Traffic Manager").GetComponent<TrafficManager>();
            }
            return singleton;
        }
    }


    #region Fields
    [SerializeField]    private ExtendingCounter spawnVehicleOnCount;
    [SerializeField]    private Counter spawnSpannerOnCount;
    [SerializeField]    private Counter vehicleDieRolls;
    [SerializeField]    private int vehicleDieRollSides;
    [SerializeField]    private int vehicleDieRollThreshold;
    [SerializeField]    private GameObject[] trafficPrefabs;
    [SerializeField]    private GameObject[] repairItemPrefabs;
    [SerializeField]    private GameObject streetLightPrefab;
    [SerializeField]    GameObject finishLinePrefab;
    [SerializeField]    private Counter totalRepairPacksSpawned;
    #endregion

    #region Properties
    private RoadManager Road_Manager => RoadManager.Singleton;
    public GameObject[] TrafficPrefabs => trafficPrefabs;
    public GameObject[] RepairItemPrefabs => repairItemPrefabs;
    public GameObject StreetLight => streetLightPrefab;    
    public ExtendingCounter SpawnVehicleOnCount => spawnVehicleOnCount;
    public Counter SpawnSpannerOnCount => spawnSpannerOnCount;
    public bool CanSpawnObjects
    {
        get
        {
            switch (GameManager.Singleton.Phase)
            {
                case GamePhase.Level: return Road_Manager.RoadTileWithFinishLine > Road_Manager.TilesSpawnedThisRound.Count + (spawnVehicleOnCount.upperLimit);
                case GamePhase.Endless: return true;
                default: return false;
            }
        }
    }

    private bool DieRolledAboveThreshold
    {
        get
        {
            vehicleDieRolls.Increment();
            return Random.Range(0, vehicleDieRollSides) > vehicleDieRollThreshold;
        }
    }

    private bool ReadyToSpawnRepairPacks => CanSpawnObjects && spawnSpannerOnCount.UpperLimitReached;
    private bool VehicleExtendedCountLimitReached => spawnVehicleOnCount.isUsingExtendedLimit && spawnVehicleOnCount.ExtendedLimitReached;
    private bool VehicleUpperCountLimitReached => !spawnVehicleOnCount.isUsingExtendedLimit && spawnVehicleOnCount.UpperLimitReached;
    private bool ReadyToSpawnVehiclesOnDieRoll => CanSpawnObjects && VehicleUpperCountLimitReached;
    private bool ReadyToSpawnVehiclesOnAdditionalDieRoll => CanSpawnObjects && VehicleExtendedCountLimitReached;
    private bool FirstVehicleDieRollSuccessful => ReadyToSpawnVehiclesOnDieRoll && DieRolledAboveThreshold;
    private bool FirstVehicleDieRollFailed => ReadyToSpawnVehiclesOnDieRoll && !DieRolledAboveThreshold;
    private bool AdditionalVehicleDieRollSuccessful => ReadyToSpawnVehiclesOnAdditionalDieRoll && DieRolledAboveThreshold;
    private bool AdditionalVehicleDieRollFailed => ReadyToSpawnVehiclesOnAdditionalDieRoll && !DieRolledAboveThreshold;
    private bool AnyVehicleDieRollFailed => FirstVehicleDieRollFailed || AdditionalVehicleDieRollFailed;
    private bool AnyVehicleDieRollSuccessful => FirstVehicleDieRollSuccessful || AdditionalVehicleDieRollSuccessful;
    #endregion

    #region Methods
    private void SpawnRepairPacks()
    {
        spawnSpannerOnCount.ResetToLowerLimit();
        Road_Manager.RoadFurthestForward.SpawnRepairItemsInSlots();
        totalRepairPacksSpawned.Increment();
    }

    private void ResetVehicleCountersAndSpawnVehicles()
    {
        spawnVehicleOnCount.ResetToLowerLimit();
        vehicleDieRolls.ResetToLowerLimit();
        spawnVehicleOnCount.StopUsingExtendedLimit();
        spawnVehicleOnCount.ResetExtendedLimit();

        Road_Manager.RoadFurthestForward.SpawnVehiclesInSlots();
    }

    private void ExtendVehicleCounterLimitOnDieRollFail() => spawnVehicleOnCount.ExtendLimit(spawnVehicleOnCount.extensionAmount);
    #endregion





    private void Update()
    {
        if (AnyVehicleDieRollFailed)
        {
            ExtendVehicleCounterLimitOnDieRollFail();
        }

        if (AnyVehicleDieRollSuccessful)
        {
            ResetVehicleCountersAndSpawnVehicles();
        }

        if (FirstVehicleDieRollFailed)
        {
            spawnVehicleOnCount.StartUsingExtendedLimit();
        }

        if (ReadyToSpawnRepairPacks)
        {
            SpawnRepairPacks();
        }
    }
}