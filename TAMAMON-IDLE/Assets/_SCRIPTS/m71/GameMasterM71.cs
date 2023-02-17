using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMasterM71 : BaseController
{
    [Header("Song Start Time")]
    [SerializeField] private float songStartTime;
    [SerializeField] private float marginSeconds;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        if (Data.TigerSongExists())
        {
            Data.DestroyTigerSong();
        }
        if (!Data.SabreSongExists())
        {
            Data.CreateSabreSong();
        }

        // if out of sync, set time
        if (Mathf.Abs(Data.GetSabreSong().time - songStartTime) > marginSeconds)
        {
            Data.GetSabreSong().time = (songStartTime);
        }

        if (!Data.GetSabreSong().isPlaying)
        {
            Data.GetSabreSong().Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.BaseUpdate();
        var currentTime = Data.GetSabreSong().time;



    }
}
