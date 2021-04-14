using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_Text progressText;

    // 'SceneManager.LoadScene' stops all game processing and diverts all processing power towards loading a new scene.
    // 'SceneManager.LoadSceneAsync' allows game processing to continue while loading a new scene. 
    // This continuation of processing is what allows us to get information back from the new scene, such as its loading progress,
    //so that we can create a loading bar.

    private void Start()
    {
        LoadLevel(1);
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    private IEnumerator LoadAsynchronously (int sceneIndex)
    {
        loadingScreen.SetActive(true);

        AsyncOperation aOp = SceneManager.LoadSceneAsync(sceneIndex);

        while(!aOp.isDone)
        {
            float progress = Mathf.Clamp01(aOp.progress / 0.9F);

            slider.value = progress;
            progressText.text = progress * 100F + "%";

            yield return null;
        }
    }
}