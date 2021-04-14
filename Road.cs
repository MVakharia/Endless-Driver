using UnityEngine;

public class Road : MonoBehaviour
{
    #region Fields
    [SerializeField]
    bool carCrossedRoadOnce;
    [SerializeField]
    bool carhasLeftThisTrigger;
    [SerializeField]
    float distanceFromCar;
    [SerializeField]
    float maxDistanceFromCar;
    [SerializeField]
    int numberOfLanes;
    [SerializeField]
    int tileNumber;
    [SerializeField]
    PlayerVehicleDrive player;
    [SerializeField]
    Renderer thisRenderer;
    [SerializeField]
    GameObject[] roadObjectSpawnPoints;
    [SerializeField]
    GameObject[] objectsOnRoad;
    [SerializeField]
    GameObject[] streetLightSpawnPoints;
    [SerializeField]
    GameObject[] streetLightSlots;
    [SerializeField]
    GameObject finishLineSlot;
    #endregion

    #region Properties
    public PlayerVehicleDrive Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player Car").GetComponent<PlayerVehicleDrive>();
            }
            return player;
        }
    }
    bool IsFinishLineLocation => tileNumber == RoadManager.Singleton.RoadTileWithFinishLine && GameManager.Singleton.Phase == GamePhase.Level;
    bool PlayerHasCrossedThisFinishLine => carhasLeftThisTrigger && IsFinishLineLocation;
    bool NoMoreRoomInRearPool => RoadManager.Singleton.NoMoreRoomBehindCar;
    bool FinishLineNotYetPlaced => IsFinishLineLocation && !RoadManager.Singleton.FinishLineHasSpawned;
    #endregion

    #region Methods
    public void AddObjects()
    {
        for (int i = 0; i < numberOfLanes; i++)
        {
            if(roadObjectSpawnPoints[i] == null)
            {
                roadObjectSpawnPoints[i] = transform.Find("Object Spawn Point " + i.ToString()).gameObject;
            }
        }
    }

    public void CheckCarAsCrossedRoad() => carCrossedRoadOnce = true;
    public void UncheckCarAsCrossedRoad() => carCrossedRoadOnce = false;
    public void CheckCarAsLeftTrigger() => carhasLeftThisTrigger = true;
    public void UncheckCarAsLeftTrigger() => carhasLeftThisTrigger = false;
    public void Move() => transform.Translate(Vector3.back * Time.deltaTime * Player.CurrentSpeedMPS);

    public void SpawnObject(GameObject[] spawnPoints, GameObject[] spawnSlots, GameObject prefab, int spawnSlotNumber, Quaternion rotation)
    {
        if (spawnSlots[spawnSlotNumber] == null)
        {
            spawnSlots[spawnSlotNumber] = Instantiate(prefab, spawnPoints[spawnSlotNumber].transform.position, spawnPoints[spawnSlotNumber].transform.rotation * rotation);
            spawnSlots[spawnSlotNumber].transform.parent = transform;
        }
    }

    public void SpawnObjects(GameObject[] spawnPoints, GameObject[] spawnSlots, GameObject[] prefabs, int spawnSlotNumber, Quaternion rotation)
    {
        int rand = Random.Range(0, prefabs.Length);

        if(spawnSlots[spawnSlotNumber] == null)
        {
            spawnSlots[spawnSlotNumber] = Instantiate(prefabs[rand], spawnPoints[spawnSlotNumber].transform.position, spawnPoints[spawnSlotNumber].transform.rotation * rotation);
            spawnSlots[spawnSlotNumber].transform.parent = transform;
        }
    }

    public void SpawnStreetLight ()
    {
        int spawnSlotNumber = -1;

        if(tileNumber % 4 == 0)
        {
            spawnSlotNumber = 0;
        }
        if (tileNumber % 4 == 2)
        {
            spawnSlotNumber = 1;
        }
        if(spawnSlotNumber >= 0)
        {
            SpawnObject(streetLightSpawnPoints, streetLightSlots, TrafficManager.Singleton.StreetLight, spawnSlotNumber, Quaternion.identity);
        }        
    }

    public void SpawnVehiclesInSlots()
    { 
        for (int i = 0; i < roadObjectSpawnPoints.Length; i++) 
        { 
            if (Random.Range(0, 4) != 0) 
            {
                SpawnObjects(roadObjectSpawnPoints, objectsOnRoad, TrafficManager.Singleton.TrafficPrefabs, i, Quaternion.Euler(0, 180, 0));
            } 
        }
    }

    public void SpawnRepairItemsInSlots()
    {
        for (int i = 0; i < roadObjectSpawnPoints.Length; i++)
        {
            if (Random.Range(0, 4) != 0)
            {
                SpawnObjects(roadObjectSpawnPoints, objectsOnRoad, TrafficManager.Singleton.RepairItemPrefabs, i, Quaternion.identity);
            }
        }
    }

    public void DestroyObjectsOnRoad ()
    {
        for (int i = 0; i < objectsOnRoad.Length; i++)
        {
            if(objectsOnRoad[i] != null)
            {
                Destroy(objectsOnRoad[i]);
            }
        }
    }

    public void DestroyStreetLightOnRoad ()
    {
        for (int i = 0; i < streetLightSlots.Length; i++)
        {
            if (streetLightSlots[i] != null)
            {
                Destroy(streetLightSlots[i]);
                streetLightSlots[i] = null;
            }
        }
    }

    public void SetTileNumber(int num) => tileNumber = num;
    public void SpawnFinishLine() => finishLineSlot = Instantiate(RoadManager.Singleton.FinishLinePrefab, transform.position + new Vector3(0, 2, 0), transform.rotation, transform);
    public void DestroyFinishLine() => Destroy(finishLineSlot);

    private void MoveFromFrontPoolToRearPool ()
    {
        RoadManager.Singleton.RemoveFromTilesInFrontOfCar(gameObject);
        RoadManager.Singleton.AddToTilesBehindCar(gameObject);
    }

    private void SetThisTileAsFurthestForward () => RoadManager.Singleton.SetRoadFurthestForward(this);

    private void MoveFromRearPoolToTilePool ()
    {
        RoadManager.Singleton.RemoveFromTilesBehindCar(gameObject);
        RoadManager.Singleton.AddToTilePool(gameObject);
    }
    private void CheckFinishLineAsSpawned () => RoadManager.Singleton.CheckFinishLineAsSpawned();    
    #endregion

    void Start()
    {
        AddObjects();

        objectsOnRoad = new GameObject[roadObjectSpawnPoints.Length];

        SpawnStreetLight();

        SetThisTileAsFurthestForward();
    }

    void Update()
    {
        if (carCrossedRoadOnce)
        {
            UncheckCarAsCrossedRoad();
            CheckCarAsLeftTrigger();
            MoveFromFrontPoolToRearPool();
        }

        if (PlayerHasCrossedThisFinishLine)
        {
            DestroyFinishLine();
            CameraTarget.Singleton.SetLastTile(gameObject);
            StartCoroutine(PostGameUIManager.Singleton.LevelComplete());
        }

        if (NoMoreRoomInRearPool)
        {
            MoveFromRearPoolToTilePool();
            UncheckCarAsLeftTrigger();
        }

        if(FinishLineNotYetPlaced)
        {
            SpawnFinishLine();
            CheckFinishLineAsSpawned();
        }
    }

    void FixedUpdate()
    {
        Move();
    }
}