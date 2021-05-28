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
    [SerializeField] protected List<float> crankTimes;
    [SerializeField] public float timeItTakesToCrank;

    [Header("Skip Time")]
    [SerializeField]private float skipTimeAmount;

    [Header("Outlets")]
    [SerializeField]
    private SternShipM70 ship;

    private int mineTimeIndex = 0;
    private int frontCannonTimeIndex = 0;
    private int backCannonTimeIndex = 0;
    private int crankTimeIndex=0;

    private bool skipNextUpdate;
    private float skipNextUpdateElapsed = 0f;

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

    /// <summary>
    /// skip next update when we skip the song ahead
    /// </summary>
    public void SkipNextUpdate()
    {
        skipNextUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (skipNextUpdate == true)
        {
            skipNextUpdateElapsed += Time.deltaTime;
            if(skipNextUpdateElapsed > skipTimeAmount)
            {
                skipNextUpdate = false;
            }
        }

        var currentTime = Data.GetSabreSong().time;

        if(mineTimeIndex < mineTimes.Count)
        {
            if(currentTime > mineTimes[mineTimeIndex])
            {
                if(!skipNextUpdate)
                    ship.Jump();
                mineTimeIndex++;
            }
        }

        if(frontCannonTimeIndex < frontCannonTimes.Count)
        {
            if(currentTime > frontCannonTimes[frontCannonTimeIndex]-timeItTakesCannonsToFire)
            {
                if (!skipNextUpdate)
                    ship.FireFrontCannon();
                frontCannonTimeIndex++;
            }
        }

        if (backCannonTimeIndex < backCannonTimes.Count)
        {
            if (currentTime > backCannonTimes[backCannonTimeIndex]-timeItTakesCannonsToFire)
            {
                if (!skipNextUpdate)
                    ship.FireBackCannon();
                backCannonTimeIndex++;
            }
        }

        if (crankTimeIndex < crankTimes.Count)
        {
            if (currentTime > crankTimes[crankTimeIndex] - timeItTakesToCrank)
            {
                if (!skipNextUpdate)
                    ship.CrankTop();
                crankTimeIndex++;
            }
        }


    }
}
