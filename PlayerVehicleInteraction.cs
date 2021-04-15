using UnityEngine;
public class PlayerVehicleInteraction : VehicleInteraction
{
    private static PlayerVehicleInteraction singleton;
    public static PlayerVehicleInteraction Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Player Car").GetComponent<PlayerVehicleInteraction>();
            }
            return singleton;
        }
    }

    #region Fields
    [SerializeField] private Counter vehiclesDestroyedThisRound;
    [SerializeField] private Counter damageRepairedThisRound;
    [SerializeField] private Counter repairPacksCollectedThisRound;
    #endregion

    #region Properties
    public Counter VehiclesDestroyedThisRound => vehiclesDestroyedThisRound;
    public Counter DamageRepairedThisRound => damageRepairedThisRound;
    public Counter RepairPacksCollectedThisRound => repairPacksCollectedThisRound;
    #endregion

    #region Methods
    public void HitTrafficVehicle(ref TrafficVehicleInteraction trafficVehicle, int damageAmount, float speedReduction)
    {
        if (!PlayerVehicleDrive.Singleton.BoostApplied)
        {
            TakeDamage(damageAmount);
            PlayerVehicleDrive.Singleton.ReduceSpeedMPS(speedReduction);
            PlayerVehicleDrive.Singleton.AddBoostCharge(trafficVehicle.VehicleWeight);
            vehiclesDestroyedThisRound.Increment();

            if (Health.LowerLimitReached)
            {
                StartCoroutine(PostGameUIManager.Singleton.GameOver());
            }
        }
        //play a sound

        GameManager.Singleton.Score.Add(trafficVehicle.Health.upperLimit);
    }

    public void CollectRepairPack(ref RepairPackInteraction repairPack)
    {
        RestoreHealth(repairPack.HealthToRestore);
        damageRepairedThisRound.Add(repairPack.HealthToRestore);
        repairPacksCollectedThisRound.Increment();
        //play a sound
    }
    #endregion

    private void Start()
    {
        Health.ResetToUpperLimit();
    }

    private void Update()
    {
        Health.UseUpperLimit();
        Health.UseLowerLimit();

        print(ThisCarState);
    }
}