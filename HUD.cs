using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class StatusBar
{
    [SerializeField]
    [Range(0, 1)]
    private float moveSpeed;
    public float TargetAttributeValue { get; private set; }
    public Slider bar;
    public Image fill;
    public TMP_Text text;
    [SerializeField]
    private float displayedAttribute;
    public float DisplayedAttribute
    {
        get
        {
            displayedAttribute = Mathf.MoveTowards(displayedAttribute, TargetAttributeValue, moveSpeed * Time.deltaTime);
            return displayedAttribute;
        }
    }
    public void SetTargetAttributeValue(float amount) => TargetAttributeValue = amount;

    public void SetBarValueToDisplayedAttribute() => bar.value = DisplayedAttribute;
}

public class HUD : MonoBehaviour
{
    #region Singleton
    static HUD singleton;

    public static HUD Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<HUD>();
            }
            return singleton;
        }
    }
    #endregion

    #region Fields

    [SerializeField]
    StatusBar health;
    [SerializeField]
    StatusBar boost;   
    [SerializeField]
    TMP_Text[] speedometerDigits;
    [SerializeField]
    TMP_Text[] scoreDigits;
    [SerializeField]
    TMP_Text[] odometerDigits;
    [SerializeField]
    PlayerVehicleDrive drive;
    float cyclingNumber;
    #endregion

    #region Properties
    
    public PlayerVehicleDrive Drive
    {
        get
        {
            if (drive == null)
            {
                drive = GameObject.FindGameObjectWithTag("Player Car").GetComponent<PlayerVehicleDrive>();
            }
            return drive;
        }
    }

     Color ColorChangingElementHSVToRGB (float startingHueNumber, float endingHueNumber, float sliderValue) => 
        Color.HSVToRGB(Mathf.Lerp(startingHueNumber, endingHueNumber, sliderValue) / 360f, 1, 1);
     Color DefaultBoostBarColor => ColorChangingElementHSVToRGB(50, 225, boost.bar.value);
     Color CyclingRainbowColor => Color.HSVToRGB(cyclingNumber, 1, 1);
     Color BoostBarColor => drive.BoostApplied ? CyclingRainbowColor : DefaultBoostBarColor;
     static float RedHueNumber => 15;
     static float CyanHueNumber => 89;
     float PlayerHealth => PlayerVehicleInteraction.Singleton.Health.Count;
     float PlayerMaximumHealth => PlayerVehicleInteraction.Singleton.Health.upperLimit;
     float HealthBarValue => PlayerHealth / PlayerMaximumHealth;
     float BoostAmount => PlayerVehicleDrive.Singleton.BoostAmount;
     float BoostCapacity => PlayerVehicleDrive.Singleton.BoostCapacity;
     float BoostBarValue => BoostAmount / BoostCapacity;
     string BoostAmountToString => BoostAmount.ToString("0");
     string HealthAmountToString => PlayerHealth.ToString("0") + "%";
    #endregion

    #region Methods
    void AddObjects()
    {
        if (speedometerDigits[0] == null)
        {
            speedometerDigits[0] = GameObject.Find("Current Speed First Digit").GetComponent<TMP_Text>();
        }
        if (speedometerDigits[1] == null)
        {
            speedometerDigits[1] = GameObject.Find("Current Speed Second Digit").GetComponent<TMP_Text>();
        }
        if (speedometerDigits[2] == null)
        {
            speedometerDigits[2] = GameObject.Find("Current Speed Third Digit").GetComponent<TMP_Text>();
        }
    }
    void SetBoostBarColor() => boost.fill.color = BoostBarColor;
    void CycleNumber() { if (cyclingNumber > 1) { cyclingNumber = 0; } cyclingNumber += (Time.deltaTime * 2); }
    void SetHealthBarColor() => health.fill.color = ColorChangingElementHSVToRGB(RedHueNumber, CyanHueNumber, health.bar.value);
    void SetHealthAmountText() => health.text.text = HealthAmountToString;
    void ConvertNumberDigitsToTMP_Text(float numberToConvert, string ToStringParam, TMP_Text[] TMP_TextArray)
    {
        double truncatedNumber = Math.Truncate(numberToConvert * 10) / 10;

        string truncatedNumAsString = truncatedNumber.ToString(ToStringParam);

        char[] digits = truncatedNumAsString.ToCharArray();

        for (int i = 0; i < digits.Length; i++)
        {
            TMP_TextArray[i].text = digits[i].ToString();
        }
    }
    #endregion

    void Start()
    {
        AddObjects();
    }

    void Update()
    {
        health.SetTargetAttributeValue(HealthBarValue);

        boost.SetTargetAttributeValue(BoostBarValue);

        health.SetBarValueToDisplayedAttribute();

        boost.SetBarValueToDisplayedAttribute();

        SetHealthBarColor();

        SetHealthAmountText();

        ConvertNumberDigitsToTMP_Text(Drive.CurrentSpeedMPH, "000", speedometerDigits);

        ConvertNumberDigitsToTMP_Text(GameManager.Singleton.Score.Count, "000000", scoreDigits);

        ConvertNumberDigitsToTMP_Text(Drive.Odometer, "0000.0", odometerDigits);

        SetBoostBarColor();

        CycleNumber();
    }
}