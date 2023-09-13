using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TamaDataSO", order = 1)]
public class TamaDataSO : ScriptableObject
{
    public Era01Stats era01Stats;

    public void AddCourageDelta(int delta)
    {
        era01Stats.Courage += delta;
        Debug.LogWarning("New courage amount: " + era01Stats.Courage);
    }

    public void AddVenomDelta(int delta)
    {
        era01Stats.Venom += delta;
        Debug.LogWarning("New Venom amount: " + era01Stats.Venom);
    }

}


[Serializable]
public struct Era01Stats
{
    public int Courage;
    public int Venom;
}
