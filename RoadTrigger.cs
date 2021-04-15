using UnityEngine;

public class RoadTrigger : MonoBehaviour
{
    private Road thisRoad;

    public Road ThisRoad
    {
        get
        {
            thisRoad = transform.parent.GetComponent<Road>();
            return thisRoad;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player Car Model"))
        {
            ThisRoad.CheckCarAsCrossedRoad();

            if(GameManager.Singleton.IsInGame)
            {
                TrafficManager.Singleton.SpawnVehicleOnCount.Increment();
                TrafficManager.Singleton.SpawnSpannerOnCount.Increment();
            }

        }
    }
}