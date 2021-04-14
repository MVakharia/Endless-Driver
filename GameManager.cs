using UnityEngine;
using System.Collections;
using AutoDriverLibrary;

public enum GamePhase { Intro, MainMenu, Level, Endless, GameOver, WonGame, Challenge }
public enum TestingMode { Normal, Debug }

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager singleton;
    public static GameManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
            }
            return singleton;
        }
    }
    #endregion

    #region Fields
    [SerializeField]
    GamePhase phase;
    [SerializeField]
    Counter score;
    [SerializeField]
    bool saveDataOnEndlessLoss;
    [SerializeField]
    bool saveDataOnLevelCompletion;
    [SerializeField]
    bool saveDataOnLevelLoss;
    [SerializeField]
    bool loadDataOnLaunch;
    [SerializeField]
    PlayerStatisticsData scoreData;
    #endregion

    #region Properties
    public GameObject Player => PlayerVehicleInteraction.Singleton.gameObject;
    public GamePhase Phase => phase;
    public Counter Score => score;
    public bool IsInGame => Phase == GamePhase.Level || Phase == GamePhase.Endless || Phase == GamePhase.Challenge || Phase == GamePhase.GameOver;
    public bool IsInGameAndCanMoveCar => Phase == GamePhase.Level || Phase == GamePhase.Endless || Phase == GamePhase.Challenge;
    public bool SaveDataOnEndlessLoss => saveDataOnEndlessLoss;
    public bool SaveDataOnLevelCompletion => saveDataOnLevelCompletion;
    public bool SaveDataOnLevelLoss => saveDataOnLevelLoss;
    public bool LoadDataOnLaunch => loadDataOnLaunch;
    #endregion

    #region Methods
    public void SetGamePhase(GamePhase _phase) => phase = _phase;
    public IEnumerator IncrementScore()
    {
        while (IsInGameAndCanMoveCar)
        {
            Score.Increment();

            yield return new WaitForSeconds(0.1F);
        }
    }

    private void LoadScoreData()
    {
        if (loadDataOnLaunch)
        {
            scoreData = AutoDriverSaveSystem.LoadedScoreData();
        }
    }
    #endregion

    private void Start()
    {
        LoadScoreData();
    }

    private void Update()
    {
        TrafficManager.Singleton.gameObject.SetActive(IsInGame);

        PlayerSteering.Singleton.enabled = IsInGameAndCanMoveCar;
    }
}