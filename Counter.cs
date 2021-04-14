using System;
using UnityEngine;

[Serializable]
public class Counter
{
    [SerializeField]
    protected int count;
    public int lowerLimit;
    public int upperLimit;
    public int Count => count;
    public void Increment() => count++;
    public void Decrement() => count--;
    public void Add(int amount) => count += amount;
    public void Subtract(int amount) => count -= amount;
    public void ResetToLowerLimit() => count = lowerLimit;
    public void ResetToUpperLimit() => count = upperLimit;
    public void Set(int amount) => count = amount;
    public bool UpperLimitReached => count >= upperLimit;
    public bool LowerLimitReached => count <= lowerLimit;
    public void UseUpperLimit() { if (UpperLimitReached) { ResetToUpperLimit(); } }
    public void UseLowerLimit() { if (LowerLimitReached) { ResetToLowerLimit(); } }
    public void SetLimits (int lower, int upper) { lowerLimit = lower; upperLimit = upper; }
}

/// <summary>
/// A counter with multiple limits.
/// </summary>
[Serializable]
public class ExtendingCounter : Counter
{
    public int extendedLimit;
    public int extensionAmount;
    public bool isUsingExtendedLimit;
    public bool ExtendedLimitReached => Count >= extendedLimit; 
    public void ResetExtendedLimit() => extendedLimit = upperLimit;
    public void ExtendLimit(int amount) => extendedLimit += amount;
    public void StartUsingExtendedLimit() => isUsingExtendedLimit = true;
    public void StopUsingExtendedLimit() => isUsingExtendedLimit = false;
}
