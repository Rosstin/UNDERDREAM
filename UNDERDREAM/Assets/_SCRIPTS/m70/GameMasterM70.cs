using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterM70 : MonoBehaviour
{
    public UniversalDataSO Data;

    [Header("Timing")]
    [SerializeField] protected List<float> mineTimes;
    [SerializeField] protected List<float> cannonTimes;

    // Start is called before the first frame update
    void Start()
    {
        Data.DestroyTigerSong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
