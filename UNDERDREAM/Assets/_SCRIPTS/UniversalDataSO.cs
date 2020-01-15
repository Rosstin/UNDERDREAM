using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UniversalDataSO", order = 1)]
public class UniversalDataSO : ScriptableObject
{
    public List<string> Scenes;
    public List<string> Passwords;
}
