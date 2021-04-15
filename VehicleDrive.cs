using UnityEngine;

public class VehicleDrive : MonoBehaviour
{
    #region Fields
    [SerializeField] protected float normalAcceleration;
    [SerializeField] protected float boostedAcceleration;
    [SerializeField] protected float currentSpeedMPS;
    [SerializeField] protected float currentSpeedMPH;
    [SerializeField] protected float normalTopSpeedMPH;
    [SerializeField] protected float boostedTopSpeedMPH;
    [SerializeField] protected float noughtTo62;
    [SerializeField] protected bool hasReached62MPH;
    [SerializeField] protected float noughtTo100;
    [SerializeField] protected bool hasReached100MPH;
    [SerializeField] protected bool boostApplied;
    [SerializeField] protected float boostAmount;
    [SerializeField] protected float boostCapacity;
    [SerializeField] protected float boostConsumptionRate;
    [SerializeField] protected float odometer;
    #endregion

    #region Properties
    public float CurrentSpeedMPS
    {
        get
        {
            if (currentSpeedMPS < 0)
            {
                currentSpeedMPS = 0;
            }
            return currentSpeedMPS;
        }
        private set
        {
            currentSpeedMPS = value;
        }
    }

    public float CurrentSpeedMPH => currentSpeedMPH;

    public float BoostAmount
    {
        get
        {
            if (boostAmount > boostCapacity)
            {
                boostAmount = boostCapacity;
            }

            if (boostAmount < 0)
            {
                boostAmount = 0;
            }
            return boostAmount;
        }
        private set
        {
            boostAmount = value;
        }
    }
    public float BoostCapacity => boostCapacity;
    public float Odometer => odometer;
    protected float CalculateMilesPerHour => CurrentSpeedMPS * 2.237F;
    public bool BoostApplied => boostApplied;
    protected string MilesPerHourString => currentSpeedMPH.ToString("0");
    protected virtual float Acceleration => normalAcceleration;
    protected virtual float TopSpeed => normalTopSpeedMPH;
    public bool BoostMeterIsEmpty => boostAmount <= 0;
    public bool IsAtBoostCapacity => boostAmount >= boostCapacity;
    #endregion

    #region Methods
    protected void Accelerate()
    {
        if (currentSpeedMPH < TopSpeed)
        {
            CurrentSpeedMPS += Acceleration * Time.deltaTime;
        }
        else if (currentSpeedMPH > TopSpeed)
        {
            CurrentSpeedMPS -= Acceleration * Time.deltaTime;
        }
    }
    protected void SetMilesPerHour() => currentSpeedMPH = CalculateMilesPerHour;
    protected void Check62() => hasReached62MPH = true;
    protected void Set0To62Time() { if (currentSpeedMPH > 62 && !hasReached62MPH) { Check62(); noughtTo62 = Time.time; } }
    protected void Check100() => hasReached100MPH = true;
    protected void Set0To100Time() { if (currentSpeedMPH > 100 && !hasReached100MPH) { Check100(); noughtTo100 = Time.time; } }
    public void SetCurrentSpeedMPS(float amount) => CurrentSpeedMPS = amount;
    public void ReduceSpeedMPS(float amount) => CurrentSpeedMPS -= amount;
    public void ActivateBoostMode() => boostApplied = true;
    public void DeactivateBoostMode() => boostApplied = false;
    public void BurnBoostMeter() => BoostAmount -= boostConsumptionRate * Time.deltaTime;
    public void AddBoostCharge(float amount) => BoostAmount += amount;
    public void UpdateOdometer() => odometer += (currentSpeedMPH / 3600) * Time.deltaTime;
    #endregion
}