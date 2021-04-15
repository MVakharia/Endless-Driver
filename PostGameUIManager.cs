using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using AutoDriverLibrary;

public class PostGameUIManager : MonoBehaviour
{
    private static PostGameUIManager singleton;
    public static PostGameUIManager Singleton
    {
        get
        {
            if (singleton == null) { singleton = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<PostGameUIManager>(); }
            return singleton;
        }
    }

    #region Fields    
    [SerializeField] GameObject continueUI;
    [SerializeField] GameObject levelCompleteUI;
    [SerializeField] PlayerStatisticsBoard roundEndStatBoard;
    #endregion

    #region Properties
    public GameManager Game_Manager => GameManager.Singleton;
    public int ScoreThisRound => Game_Manager.Score.Count;
    public float DistanceTravelledThisRoundInMiles => PlayerVehicleDrive.Singleton.Odometer;
    public int DamageTakenThisRound => PlayerVehicleInteraction.Singleton.DamageTakenThisRound;
    public int CarsDestroyedThisRound => PlayerVehicleInteraction.Singleton.VehiclesDestroyedThisRound.Count;
    public int DamageRepairedThisRound => PlayerVehicleInteraction.Singleton.DamageRepairedThisRound.Count;
    public int RepairPickupsCollectedThisRound => PlayerVehicleInteraction.Singleton.RepairPacksCollectedThisRound.Count;
    #endregion

    #region Methods
    private void SetScoreThisRoundText() => roundEndStatBoard.scoreTMPText.text = ScoreThisRound.ToString();
    private void SetDistanceTravelledThisRoundText() => roundEndStatBoard.distanceTravelledTMPText.text = DistanceTravelledThisRoundInMiles.ToString("0.00") + " miles";
    private void SetDamageTakenThisRoundText() => roundEndStatBoard.damageTakenTMPText.text = DamageTakenThisRound.ToString();
    private void SetCarsDestroyedThisRoundText() => roundEndStatBoard.carsDestroyedTMPText.text = CarsDestroyedThisRound.ToString();
    private void SetDamageRepairedThisRoundText() => roundEndStatBoard.damageRepairedTMPText.text = DamageRepairedThisRound.ToString();
    private void SetRepairPickupsCollectedThisRoundText() => roundEndStatBoard.repairPickupsCollectedTMPText.text = RepairPickupsCollectedThisRound.ToString();
    public void ContinueToMainMenuButton() => StartCoroutine(ContinueToMainMenu());

    public IEnumerator GameOver()
    {
        Game_Manager.SetGamePhase(GamePhase.GameOver);
        //set the car's speed to zero.
        PlayerVehicleDrive.Singleton.SetCurrentSpeedMPS(0);

        //disable the steering component.
        PlayerSteering.Singleton.enabled = false;

        //play a car explosion animation.

        PlayerVehicleDrive.Singleton.gameObject.GetComponentInChildren<MeshCollider>().enabled = false;

        yield return new WaitForSeconds(0.5F);

        continueUI.SetActive(true);

        SetScoreThisRoundText();

        SetDistanceTravelledThisRoundText();

        SetDamageTakenThisRoundText();

        SetCarsDestroyedThisRoundText();

        SetDamageRepairedThisRoundText();

        SetRepairPickupsCollectedThisRoundText();

        //half a second later, start an animation to fade in a UI canvas with the following options:

        //watch an ad to continue | restart | quit to menu

        //SceneManager.LoadSceneAsync(0);
    }

    public IEnumerator LevelComplete()
    {
        Game_Manager.SetGamePhase(GamePhase.WonGame);

        //increase the maximum number of tiles behind the car to 30.

        //The camera parents itself to the road with the finish line.

        //The car drives away from the player for 2 seconds, then the screen fades to another with the UI showing completion of the level.

        yield return new WaitForSeconds(2);

        SceneManager.LoadSceneAsync(0);
    }

    public IEnumerator ContinueToMainMenu()
    {
        //save data to google play services

        SavePostGameScore();

        yield return new WaitForSeconds(0);

        SceneManager.LoadSceneAsync(0);
    }

    public void SavePostGameScore()
    {
        if (GameManager.Singleton.SaveDataOnEndlessLoss)
        {
            AddScoreToLifetimeStats();
        }
    }

    private void AddScoreToLifetimeStats()
    {
        PlayerStatisticsData existingData = AutoDriverSaveSystem.LoadedScoreData();
        PlayerStatisticsData appendedData = existingData;

        appendedData.AddStats(this);

        AutoDriverSaveSystem.SaveAppendedScoreData(appendedData);
    }
    #endregion
}