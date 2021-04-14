using UnityEngine;

public class PlayerVehicleDrive : VehicleDrive
{
    static PlayerVehicleDrive singleton;
    public static PlayerVehicleDrive Singleton
    {
        get
        {
            if (!singleton)
            {
                singleton = GameObject.FindGameObjectWithTag("Player Car").GetComponent<PlayerVehicleDrive>();
            }
            return singleton;
        }
    }

    #region Properties
    public void MoveToLocation(Vector3 location) => transform.position = location;

    protected override float Acceleration
    {
        get
        {
            if (boostApplied)
            {
                return boostedAcceleration;
            }

            if (GameManager.Singleton.Phase == GamePhase.GameOver)
            {
                return 0;
            }
            return base.Acceleration;
        }
    }

    protected override float TopSpeed
    {
        get
        {
            if (boostApplied)
            {
                return boostedTopSpeedMPH;
            }
            else
            {
                if (PlayerVehicleInteraction.Singleton.Health.Count > 50)
                {
                    return normalTopSpeedMPH;
                }
                else
                {
                    return normalTopSpeedMPH - (50 - PlayerVehicleInteraction.Singleton.Health.Count);
                }
            }
        }
    }
    #endregion

    private void Update()
    {
        Accelerate();
        SetMilesPerHour();
        Set0To62Time();
        Set0To100Time();

        if (IsAtBoostCapacity)
        {
            ActivateBoostMode();
        }

        if (boostApplied)
        {
            BurnBoostMeter();
        }

        if (BoostMeterIsEmpty)
        {
            DeactivateBoostMode();
        }

        if(GameManager.Singleton.IsInGame)
        {
            UpdateOdometer();
        }
    }
}