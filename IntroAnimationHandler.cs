using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroAnimationHandler : BaseAnimationHandler
{
    #region Fields
    [SerializeField] bool playIntroduction;
    [SerializeField] GameObject background;
    [SerializeField] Animator fadingImageAnimator;
    [SerializeField] Image fadingImageImageComponent;
    [SerializeField] GameObject fadingImageObject;
    [SerializeField] float delay;
    [SerializeField] GameObject splashLogo;
    [SerializeField] GameObject slider;
    [SerializeField] GameObject introUI;
    [SerializeField] GameObject menuUI;

    //animator parameter names
    private const string
        fade0To1 = "Fade 0 To 1",
        fade1To0 = "Fade 1 To 0";
    #endregion

    #region Methods
    public void EnableSliderObject() => slider.SetActive(true);
    public void DisableSliderObject() => slider.SetActive(false);
    public void EnableSplashLogo() => splashLogo.SetActive(true);
    public void DisableSplashLogo() => splashLogo.SetActive(false);
    public void EnableBackground() => background.SetActive(true);
    public void DisableBackground() => background.SetActive(false);

    private IEnumerator IntroAnimation(float delay)
    {
        Animate(fadingImageAnimator, fade0To1);

        yield return new WaitForSeconds(delay);

        DisableSliderObject();

        EnableSplashLogo();

        Animate(fadingImageAnimator, fade1To0);

        yield return new WaitForSeconds(delay * 2);

        Animate(fadingImageAnimator, fade0To1);

        yield return new WaitForSeconds(delay);

        DisableSplashLogo();

        DisableBackground();

        Animate(fadingImageAnimator, fade1To0);

        StartCoroutine(FadeFromIntroToMainMenu(delay));
    }

    private IEnumerator FadeFromIntroToMainMenu(float delay)
    {
        GameManager.Singleton.SetGamePhase(GamePhase.MainMenu);

        yield return new WaitForSeconds(delay);

        introUI.SetActive(false);
    }
    #endregion

    private void Start()
    {
        if (!menuUI.activeSelf)
        {
            menuUI.SetActive(true);
        }

        introUI.SetActive(playIntroduction);

        if (playIntroduction)
        {
            StartCoroutine(IntroAnimation(delay));
        }
        else
        {
            StartCoroutine(FadeFromIntroToMainMenu(delay));
        }
    }
}