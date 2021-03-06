using UnityEngine;

public class PlayerVehicleObject : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject[] rearWheels;
    [SerializeField] private GameObject[] frontWheels;
    [SerializeField] private GameObject[] wheels;
    [SerializeField] private PlayerVehicleInteraction thisVehicleInteraction;
    [SerializeField] private GameObject thisVehicle;
    #endregion

    #region Properties
    public GameObject ThisVehicle => ThisVehicleInteraction.gameObject;    

    public PlayerVehicleInteraction ThisVehicleInteraction
    {
        get
        {
            if(thisVehicleInteraction == null)
            {
                thisVehicleInteraction = GetComponentInParent<PlayerVehicleInteraction>();
            }
            return thisVehicleInteraction;
        }
    }
    #endregion

    private void Update()
    {
        for(int i = 0; i < wheels.Length; i++)
        {
            wheels[i].transform.Rotate(new Vector3(PlayerVehicleDrive.Singleton.CurrentSpeedMPS, 0, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Traffic")
        {
            TrafficVehicleInteraction otherVehicle = other.GetComponentInParent<TrafficVehicleInteraction>();

            ThisVehicleInteraction.HitTrafficVehicle(ref otherVehicle, otherVehicle.Health.Count, otherVehicle.VehicleWeight);
            
            Destroy(other.transform.parent.gameObject);
        }

        if(other.gameObject.tag == "Repair Pack")
        {
            RepairPackInteraction repairPack = other.GetComponentInParent<RepairPackInteraction>();

            ThisVehicleInteraction.CollectRepairPack(ref repairPack);

            Destroy(other.transform.parent.gameObject);
        }
    }
}