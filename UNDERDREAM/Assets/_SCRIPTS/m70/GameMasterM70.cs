using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterM70 : MonoBehaviour
{
    public UniversalDataSO Data;

    [Header("Timing")]
    [SerializeField] protected List<float> mineTimes;
    [SerializeField] protected List<float> frontCannonTimes;
    [SerializeField] protected List<float> backCannonTimes;
    [SerializeField] public float timeItTakesCannonsToFire;

    [Header("Outlets")]
    [SerializeField]
    private SternShipM70 ship;

    private int mineTimeIndex = 0;
    private int frontCannonTimeIndex = 0;
    private int backCannonTimeIndex = 0;

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
                ship.Jump();
                mineTimeIndex++;
            }
        }

        if(frontCannonTimeIndex < frontCannonTimes.Count)
        {
            if(currentTime > frontCannonTimes[frontCannonTimeIndex]-timeItTakesCannonsToFire)
            {
                ship.FireFrontCannon();
                frontCannonTimeIndex++;
            }
        }

        if (backCannonTimeIndex < backCannonTimes.Count)
        {
            if (currentTime > backCannonTimes[backCannonTimeIndex]-timeItTakesCannonsToFire)
            {
                ship.FireBackCannon();
                backCannonTimeIndex++;
            }
        }

    }
}
