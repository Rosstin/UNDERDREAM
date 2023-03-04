using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveController : MonoBehaviour
{
    // STATS
    private const string COURAGE_KEY = "COURAGE_KEY";
    private const string VENOM_KEY = "VENOM_KEY";

    [Header("Background Scroller Outlet")]
    [SerializeField] private BackgroundScroller backgroundScroller;

    [Header("Tama Outlet")]
    [SerializeField] private GameObject tama;

    private void Awake()
    {
        Debug.LogWarning("current courage " + PlayerPrefs.GetInt(COURAGE_KEY));
        Debug.LogWarning("current venom " + PlayerPrefs.GetInt(VENOM_KEY));

        backgroundScroller.enabled = true;
        tama.SetActive(true);


    }

}
