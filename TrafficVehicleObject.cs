using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficVehicleObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Death Zone")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
