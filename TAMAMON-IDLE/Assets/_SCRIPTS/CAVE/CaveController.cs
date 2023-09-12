using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveController : MonoBehaviour
{
    // STATS
    private const string COURAGE_KEY = "COURAGE_KEY";
    private const string VENOM_KEY = "VENOM_KEY";

    [Header("Timing")]
    [SerializeField] private float EventPeriod;

    [Header("Background Scroller Outlet")]
    [SerializeField] private BackgroundScroller backgroundScroller;

    [Header("Tama Outlet")]
    [SerializeField] private Tama01 tama;

    [Header("Event Outlets")]
    public List<TamaEvent> events;

    private float elapsed = 0f;

    private void Awake()
    {
        //Debug.LogWarning("current courage " + PlayerPrefs.GetInt(COURAGE_KEY));
        //Debug.LogWarning("current venom " + PlayerPrefs.GetInt(VENOM_KEY));

        backgroundScroller.EnableScrolling(true);
        tama.gameObject.SetActive(true);
        tama.StartAnimation(Tama01.Tama01AnimationState.WALK);

        elapsed = 0f;


    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > EventPeriod)
        {
            elapsed = 0f;
            PlayRandomEvent();
        }
    }

    private void PlayRandomEvent()
    {
        int randomEventIndex = Random.Range(0, events.Count);
        if (events[randomEventIndex].EventCanFire())
        {
            events[randomEventIndex].Play();
        }
    }
}
