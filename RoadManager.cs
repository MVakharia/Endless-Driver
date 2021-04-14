using UnityEngine;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour
{
    [System.Serializable]
    public class Tile { public GameObject mainTile; public GameObject tileClip; }

    #region Fields
    private static RoadManager singleton;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    GameObject[] roadPrefabs;
    [SerializeField]
    [Header("If there are already tiles in the scene, drag in the tile furthest in front of the player.")]
    Tile newestSpawnedTile;
    [SerializeField]
    Tile previouslySpawnedTile;
    [SerializeField]
    Vector3 firstTilePosition;
    [SerializeField]
    Counter tilesSpawnedThisRound;
    [SerializeField]
    List<GameObject> tilesInFrontOfCar;
    [SerializeField]
    List<GameObject> tilesBehindCar;
    [SerializeField]
    List<GameObject> tilesInPool;
    [SerializeField]
    int maximumNumberOfTilesAhead;
    [SerializeField]
    int tilePoolCapacity;
    [SerializeField]
    int maxTilesBehindCar;
    [SerializeField]
    [Range(2, 3)]
    int numberOfLanes;
    [SerializeField]
    GameObject[] lanePoints;
    [SerializeField]
    float RoadLengthInMiles;
    [SerializeField]
    int roadTileWithFinishLine;
    [SerializeField]
    bool finishLineHasSpawned;
    [SerializeField]
    GameObject finishLinePrefab;
    [SerializeField]
    Road roadFurthestForward;
    #endregion

    #region Properties
    public static RoadManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Road Manager").GetComponent<RoadManager>();
            }
            return singleton;
        }
    }
    private Vector3 FirstTilePosition
    {
        get
        {
            firstTilePosition = new Vector3(FirstTileXPosition, firstTilePosition.y, firstTilePosition.z);
            return firstTilePosition;
        }
    }

    private Vector3 PreviousTileClipPosition => previouslySpawnedTile.tileClip.transform.position;
    private Quaternion PreviousTileRotation => previouslySpawnedTile.mainTile.transform.rotation;
    public bool TileFoundInScene => GameObject.FindGameObjectWithTag("Tile");
    public bool SpaceForTileAhead => tilesInFrontOfCar.Count + tilesBehindCar.Count < maximumNumberOfTilesAhead;
    public bool FinishLineHasSpawned => finishLineHasSpawned;
    public bool NoMoreRoomBehindCar => tilesBehindCar.Count > 0;
    public bool NoMoreRoomInTilePool => tilesInPool.Count > tilePoolCapacity;
    public int FirstTileXPosition => (numberOfLanes - 2) * 2;    
    public int NumberOfLanes => numberOfLanes;
    private float RoadLengthInKilometres => RoadLengthInMiles * 1.60934F;
    private float RoadLengthInMetres => RoadLengthInKilometres * 1000;

    public float RoadTileWithFinishLine
    {
        get
        {
            roadTileWithFinishLine = Mathf.RoundToInt(RoadLengthInMetres / TileLengthInMetres);

            return roadTileWithFinishLine;
        }
    }
    public float TileLengthInMetres => roadPrefabs[0].transform.Find("Track").localScale.z;
    public Counter TilesSpawnedThisRound => tilesSpawnedThisRound;
    public Road RoadFurthestForward => roadFurthestForward;
    public GameObject FinishLinePrefab => finishLinePrefab;
    public GameObject[] LanePoints => lanePoints;
    private GameObject NewestTileClip => newestSpawnedTile.mainTile.transform.Find("Clip").gameObject;
    private GameObject NewestTile => newestSpawnedTile.mainTile;
    private GameObject RoadPrefabToSpawn => roadPrefabs[numberOfLanes - 2];
    private bool CanInstantiateATileAhead => SpaceForTileAhead && !NoMoreRoomInTilePool;
    private bool CanPlaceTileAheadFromPool => SpaceForTileAhead && NoMoreRoomInTilePool;
    #endregion

    #region Methods
    public Camera MainCamera
    {
        get
        {
            mainCamera = Camera.main;
            return mainCamera;
        }
    }
    public void MakeNewTileOld() { previouslySpawnedTile = newestSpawnedTile; }
    public void AddToTilesInFrontOfCar(GameObject tile) { tilesInFrontOfCar.Add(tile); }
    public void RemoveFromTilesInFrontOfCar(GameObject tile) { tilesInFrontOfCar.Remove(tile); }
    public void AddToTilesBehindCar(GameObject tile) { tilesBehindCar.Add(tile); }
    public void RemoveFromTilesBehindCar(GameObject tile) { tilesBehindCar.Remove(tile); }
    public void AddToTilePool(GameObject tile) { tilesInPool.Add(tile); }
    public void RemoveFromTilePoolAtIndex(int index) { tilesInPool.RemoveAt(index); }
    public void CheckFinishLineAsSpawned() { finishLineHasSpawned = true; }
    public void UncheckFinishLineAsSpawned() { finishLineHasSpawned = false; }
    public void SetRoadFurthestForward(Road road) { roadFurthestForward = road; }

    private void SetUpMainTile(GameObject tile)
    {
        newestSpawnedTile.mainTile = tile;
        newestSpawnedTile.tileClip = NewestTileClip;
    }

    /*private void SetUpMainTile(GameObject tile, Vector3 tilePosition, Quaternion tileRotation)
    {
        newestSpawnedTile.mainTile = tile;
        newestSpawnedTile.mainTile.transform.position = tilePosition;
        newestSpawnedTile.mainTile.transform.rotation = tileRotation;
        newestSpawnedTile.tileClip = NewestTileClip;
    }*/
    #endregion

    private void Update()
    {
        if (SpaceForTileAhead && TileFoundInScene)
        {
            tilesSpawnedThisRound.Increment();
        }

        if (!TileFoundInScene)
        {
            GameObject tile = Instantiate(RoadPrefabToSpawn, FirstTilePosition, RoadPrefabToSpawn.transform.rotation);

            Road road = tile.GetComponent<Road>();

            road.SetTileNumber(tilesSpawnedThisRound.Count);

            SetUpMainTile(tile);
        }

        if (SpaceForTileAhead)
        {
            MakeNewTileOld();
        }

        if (CanInstantiateATileAhead)
        {
            GameObject tile = Instantiate(RoadPrefabToSpawn, PreviousTileClipPosition, PreviousTileRotation);

            Road road = tile.GetComponent<Road>();

            road.SetTileNumber(tilesSpawnedThisRound.Count);

            SetUpMainTile(tile);
        }

        if (CanPlaceTileAheadFromPool)
        {
            int rand = Random.Range(0, tilesInPool.Count - maxTilesBehindCar);

            GameObject tile = tilesInPool[rand];

            RemoveFromTilePoolAtIndex(rand);

            Road road = tile.GetComponent<Road>();

            road.DestroyObjectsOnRoad();

            road.SetTileNumber(tilesSpawnedThisRound.Count);

            SetRoadFurthestForward(road);

            newestSpawnedTile.mainTile = tile;

            newestSpawnedTile.mainTile.transform.position = PreviousTileClipPosition;

            newestSpawnedTile.mainTile.transform.rotation = PreviousTileRotation;

            newestSpawnedTile.tileClip = NewestTileClip;

            road.DestroyStreetLightOnRoad();

            road.SpawnStreetLight();
        }

        if (SpaceForTileAhead || !TileFoundInScene)
        {
            AddToTilesInFrontOfCar(NewestTile);
        }
    }
}