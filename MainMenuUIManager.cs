using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using AutoDriverLibrary;

public class MainMenuUIManager : MonoBehaviour
{
    #region Fields
    [SerializeField]
    GameObject menuUI;
    [SerializeField]
    GameObject inGameUI;
    [SerializeField]
    GameObject settingsUI;
    [SerializeField]
    GameObject challengeUI;
    [SerializeField]
    GameObject statsUI;
    [SerializeField]
    PlayerStatisticsBoard lifetimeStatBoard;
    #endregion

    #region Properties
    public void GetMoreCreditsButton() => StartCoroutine(GetMoreCredits());
    public void PlayEndlessModeButton() => StartCoroutine(PlayEndlessMode());
    public void PlayNextLevelButton() => StartCoroutine(PlayNextLevel());
    public void SettingsButton() => StartCoroutine(OpenSettingsMenu());
    public void ReturnToMainMenuButton() => StartCoroutine(ReturnToMainMenu());
    public void UpgradeYourCarButton() => StartCoroutine(UpgradeYourCar());
    public void OpenChallengeModeMenuButton() => StartCoroutine(OpenChallengeModeMenu());
    public void RemoveAdsButton() => StartCoroutine(RemoveAds());
    public void OpenStatisticsButton() => StartCoroutine(OpenStatisticsMenu());
    public GameManager Game_Manager => GameManager.Singleton;
    #endregion

    #region Methods
    public IEnumerator GetMoreCredits()
    {
        Debug.Log("Opening Credit Purchase Menu");

        yield return new WaitForSeconds(0);
    }

    public void PlayLevel(GamePhase phase)
    {
        StartCoroutine(Game_Manager.IncrementScore());

        menuUI.SetActive(false);

        Game_Manager.SetGamePhase(phase);

        inGameUI.SetActive(true);
    }

    public IEnumerator PlayEndlessMode()
    {
        Debug.Log("Starting Endless Mode");

        PlayLevel(GamePhase.Endless);

        yield return new WaitForSeconds(0);
    }

    public IEnumerator PlayNextLevel()
    {
        Debug.Log("Starting Next Level");

        PlayLevel(GamePhase.Level);

        yield return new WaitForSeconds(0);
    }

    public IEnumerator OpenSettingsMenu()
    {
        menuUI.SetActive(false);

        settingsUI.SetActive(true);

        yield return new WaitForSeconds(0);
    }

    public IEnumerator ReturnToMainMenu()
    {
        if (settingsUI.activeSelf)
        {
            settingsUI.SetActive(false);
        }

        if (challengeUI.activeSelf)
        {
            challengeUI.SetActive(false);
        }

        if (statsUI.activeSelf)
        {
            statsUI.SetActive(false);
        }

        menuUI.SetActive(true);

        yield return new WaitForSeconds(0);
    }

    public IEnumerator UpgradeYourCar()
    {
        Debug.Log("Loading Garage.");

        yield return new WaitForSeconds(0);
    }

    public IEnumerator OpenChallengeModeMenu()
    {
        menuUI.SetActive(false);

        challengeUI.SetActive(true);

        yield return new WaitForSeconds(0);
    }

    public IEnumerator RemoveAds()
    {
        Debug.Log("Opening in-app purchase to remove ads for 99p.");

        yield return new WaitForSeconds(0);
    }

    public IEnumerator OpenStatisticsMenu ()
    {
        menuUI.SetActive(false);

        statsUI.SetActive(true);

        LoadLifetimeStatistics();

        yield return new WaitForSeconds(0);
    }

    public void ResetStatisticsAndGame ()
    {
        AutoDriverSaveSystem.EraseScoreData();

        SceneManager.LoadSceneAsync(0);
    }

    public void LoadLifetimeStatistics ()
    {
        PlayerStatisticsData loadedData = AutoDriverSaveSystem.LoadedScoreData();

        if(loadedData != null)
        {
            lifetimeStatBoard.scoreTMPText.text = loadedData.score.ToString();

            lifetimeStatBoard.distanceTravelledTMPText.text = loadedData.distance.ToString("0.00") + " miles";

            lifetimeStatBoard.damageTakenTMPText.text = loadedData.damageTaken.ToString();

            lifetimeStatBoard.carsDestroyedTMPText.text = loadedData.carsDestroyed.ToString();

            lifetimeStatBoard.damageRepairedTMPText.text = loadedData.damageRepaired.ToString();

            lifetimeStatBoard.repairPickupsCollectedTMPText.text = loadedData.repairPacksCollected.ToString();
        }
    }
    #endregion
}