using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPackInteraction : MonoBehaviour
{
    [SerializeField]
    private int healthToRestore;

    public int HealthToRestore { get => healthToRestore; private set => healthToRestore = value; }
}
