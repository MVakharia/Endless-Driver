using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficVehicleDrive : VehicleDrive
{
    protected override float TopSpeed => base.TopSpeed;

    private void FixedUpdate()
    {
        Accelerate();
        SetMilesPerHour();
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * currentSpeedMPS * Time.deltaTime, Space.Self);
    }
}
