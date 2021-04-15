using UnityEngine;

public enum CarState { Destroyed, Critical, HeavyDamage, MediumDamage, LightDamage, NoDamage }

public abstract class VehicleInteraction : MonoBehaviour
{
    #region Fields
    [SerializeField]    protected Counter health;
    [SerializeField]    CarState thisCarState;
    [SerializeField]    private int damageTakenThisRound;
    #endregion

    #region Properties
    public Counter Health => health;

    public CarState ThisCarState
    {
        get
        {
            float healthAsFloat = health.Count;
            float maxHealthAsFloat = health.upperLimit;

            if(healthAsFloat == 0)
            {
                thisCarState = CarState.Destroyed;
            }
            else if (healthAsFloat / maxHealthAsFloat > 0 && healthAsFloat / maxHealthAsFloat < 0.25F)
            {
                thisCarState = CarState.Critical;
            }
            else if(healthAsFloat / maxHealthAsFloat >= 0.25F && healthAsFloat / maxHealthAsFloat < 0.5F)
            {
                thisCarState = CarState.HeavyDamage;
            }
            else if (healthAsFloat / maxHealthAsFloat >= 0.5F && healthAsFloat / maxHealthAsFloat < 0.75F)
            {
                thisCarState = CarState.MediumDamage;
            }
            else if (healthAsFloat / maxHealthAsFloat >= 0.75F && healthAsFloat / maxHealthAsFloat < 1F)
            {
                thisCarState = CarState.LightDamage;
            }
            else if (healthAsFloat / maxHealthAsFloat == 1)
            {
                thisCarState = CarState.NoDamage;
            }
            return thisCarState;
        }
    }

    public int DamageTakenThisRound => damageTakenThisRound;
    #endregion

    #region
    public void TakeDamage(int amount)
    {
        //play a sound effect
        //Play a particle effect

        health.Subtract(amount);

        damageTakenThisRound += amount;
    }

    public void RestoreHealth(int amount)
    {
        //play a sound effect
        //play a particle effect

        health.Add(amount);
    }
    #endregion
}