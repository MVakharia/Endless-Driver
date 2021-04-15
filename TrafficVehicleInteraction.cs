using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TrafficVehicleInteraction : VehicleInteraction
{
    #region Fields
    [SerializeField] PlayerVehicleInteraction player;
    [SerializeField] Renderer thisRenderer;
    [SerializeField] Image thisPanel;
    [SerializeField] TMP_Text thisPanelText;
    [SerializeField] int minimumColorDegree;
    [SerializeField] int maximumColorDegree;
    [SerializeField] int meshColliderChildNumber;
    [SerializeField] int canvasChildNumber;
    [SerializeField] int meshRendererChildNumber;
    [SerializeField] int vehicleWeightKG;

    const int defaultMaximumColor = 2;

    readonly string[] vehicleNames = new string[] { "Bus", "Compact", "Hybrid", "PickUp", "Sedan", "SUV", "Truck", "Van" };
    #endregion

    #region Properties
    public PlayerVehicleInteraction Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player Car").GetComponent<PlayerVehicleInteraction>();
            }
            return player;
        }
    }
    public Renderer ThisRenderer
    {
        get
        {
            if (thisRenderer == null)
            {
                thisRenderer = transform.GetChild(meshRendererChildNumber).GetComponent<Renderer>();
                Debug.LogError("Please insert a permanent reference to the renderer for the object" + gameObject.name);
            }
            return thisRenderer;
        }
    }

    string VehicleName => gameObject.name.Replace("(Clone)", "");
    int CalculateMaximumHealth => Random.Range(5, Mathf.RoundToInt((float)Player.Health.upperLimit / 2));
    Color ColorRelativeToPlayerHealth => Color.HSVToRGB(Mathf.Lerp(178, 0, (float)health.Count / (player.Health.upperLimit / 2)) / 360F, 1, 1);
    public int VehicleWeightKG
    {
        get
        {
            if (VehicleName.Contains(vehicleNames[1]))
            {
                //I am a compact
                vehicleWeightKG = 1000;
            }
            else if (VehicleName.Contains(vehicleNames[4]))
            {
                //I am a sedan
                vehicleWeightKG = 1500;
            }
            else if (VehicleName.Contains(vehicleNames[2]))
            {
                //I am a hybrid
                vehicleWeightKG = 1500;
            }
            else if (VehicleName.Contains(vehicleNames[5]))
            {
                //I am an SUV
                vehicleWeightKG = 2000;
            }
            else if (VehicleName.Contains(vehicleNames[7]))
            {
                //I am a van
                vehicleWeightKG = 3000;
            }
            else if (VehicleName.Contains(vehicleNames[3]))
            {
                //I am a pickup
                vehicleWeightKG = 2500;
            }
            else if (VehicleName.Contains(vehicleNames[0]))
            {
                //I am a bus
                vehicleWeightKG = 12000;
            }
            else if (VehicleName.Contains(vehicleNames[6]))
            {
                //I am a truck
                vehicleWeightKG = 44000;
            }
            return vehicleWeightKG;
        }
    }
    public float VehicleWeight => VehicleWeightKG / 200;
    #endregion

    #region Methods
    private void SetVehicleColor()
    {
        if (maximumColorDegree == 0)
        {
            maximumColorDegree = defaultMaximumColor;
        }

        int randRed = Random.Range(minimumColorDegree, maximumColorDegree);
        int randGreen = Random.Range(minimumColorDegree, maximumColorDegree);
        int randBlue = Random.Range(minimumColorDegree, maximumColorDegree);

        thisRenderer.materials[1].color = new Color((float)randRed / (maximumColorDegree - 1), (float)randGreen / (maximumColorDegree - 1), (float)randBlue / (maximumColorDegree - 1));
    }
    private void SetPanelColor() => thisPanel.color = ColorRelativeToPlayerHealth;
    #endregion

    private void Start()
    {
        if (health.upperLimit == 0)
        {
            health.SetLimits(0, (CalculateMaximumHealth));
        }

        health.ResetToUpperLimit();

        SetVehicleColor();
    }

    private void Update()
    {
        SetPanelColor();

        thisPanelText.text = Health.Count.ToString();
    }
}