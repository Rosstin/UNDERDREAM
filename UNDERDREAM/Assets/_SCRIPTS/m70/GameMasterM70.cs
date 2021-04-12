using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterM70 : MonoBehaviour
{
    public UniversalDataSO Data;

    [Header("Timing")]
    [SerializeField] protected List<float> mineTimes;
    [SerializeField] protected List<float> cannonTimes;

    [Header("Outlets")]
    [SerializeField]
    private SternShipM70 ship;

    private int mineTimeIndex = 0;
    private int cannonTimeIndex = 0;

    private float elapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Data.DestroyTigerSong();
        Data.CreateSabreSong();

        if (!Data.GetSabreSong().isPlaying)
        {
            Data.GetSabreSong().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var currentTime = Data.GetSabreSong().time;

        if(mineTimeIndex < mineTimes.Count)
        {
            if(currentTime > mineTimes[mineTimeIndex])
            {
                ship.FireBackCannon();
                mineTimeIndex++;
            }
        }

        if(cannonTimeIndex < cannonTimes.Count)
        {
            if(currentTime > cannonTimes[cannonTimeIndex])
            {
                ship.FireFrontCannon();
                cannonTimeIndex++;
            }
        }
    }
}
