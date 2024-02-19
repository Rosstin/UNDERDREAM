using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpiderEventCutscene : BaseCutsceneController
{
    private float elapsed = 0f;

    [Header("Timing")]
    public float TotalDuration;

    public TamaDataSO data;

    void Update()
    {
        elapsed += Time.deltaTime;

        if(elapsed > TotalDuration)
        {
            data.AddCourageDelta(1);
            SceneManager.LoadScene("Boot", LoadSceneMode.Single);
        }
    }
}