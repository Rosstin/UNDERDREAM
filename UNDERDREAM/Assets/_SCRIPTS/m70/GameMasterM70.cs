using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasterM70 : MonoBehaviour
{
    public UniversalDataSO Data;

    [Header("Debug")]
    [SerializeField] private float songStartTime;

    [Header("Timing")]
    [SerializeField] protected List<float> mineTimes;
    [SerializeField] protected List<float> frontCannonTimes;
    [SerializeField] protected List<float> backCannonTimes;
    [SerializeField] public float timeItTakesCannonsToFire;
    [SerializeField] protected List<float> crankTimes;
    [SerializeField] public float timeItTakesToCrank;
    [SerializeField] private float particleStartTime;
    [SerializeField] private float flamesStartTime;
    [SerializeField] private float gunFireTime;
    [SerializeField] protected float wipeoutBuffer;

    [Header("SFX")]
    [SerializeField] protected AudioSource wipeout;
    [SerializeField] private AudioSource laserSaw;
    [SerializeField] private AudioSource laserCharge;
    [SerializeField] private AudioSource laserFire;

    [Header("Skip Time")]
    [SerializeField]private float skipTimeAmount;

    [Header("Outlets")]
    [SerializeField] private SternShipM70 ship;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject flames;
    [SerializeField] protected Cloudloop blackout;
    [SerializeField] private MissileLauncher missileLauncher;

    private int mineTimeIndex = 0;
    private int frontCannonTimeIndex = 0;
    private int backCannonTimeIndex = 0;
    private int crankTimeIndex=0;

    private bool skipNextUpdate;
    private float skipNextUpdateElapsed = 0f;

    private float elapsed = 0f;

    private float lastTime = 0f;

    /// <summary>
    /// fire the big gun and restart the scene
    /// </summary>
    private void GoGoItanoCircus()
    {
        // todo play itano circus anim
        Debug.LogWarning("GoGoItanoCircus");


        StartCoroutine(Failure());

    }

    protected IEnumerator Failure()
    {
        //this.enabled = (false); // stop the update loop
        laserFire.Play();
        blackout.gameObject.SetActive(true);
        blackout.enabled = true;
        wipeout.Play();
        yield return new WaitForSeconds(wipeout.clip.length + wipeoutBuffer);
        Restart();
    }

    protected void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // Start is called before the first frame update
    void Start()
    {
        particles.gameObject.SetActive(false);
        flames.gameObject.SetActive(false);
        Data.DestroyTigerSong();
        Data.DestroySabreSong();
        Data.CreateSabreSong();

        // if you're out of sync (maybe because this is a redo, or big slowdown), reset time to correct spot
        if (Data.GetSabreSong().time > songStartTime + 3 ||
            Data.GetSabreSong().time < songStartTime - 3
            )
        {
            Data.GetSabreSong().time = (songStartTime);
        }

        if (!Data.GetSabreSong().isPlaying)
        {
            Data.GetSabreSong().Play();
        }

        blackout.gameObject.SetActive(false);
        blackout.enabled = false;
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

        if (currentTime > particleStartTime && lastTime < particleStartTime)
        {
            laserSaw.Play();
            particles.gameObject.SetActive(true);
        }

        if (currentTime > flamesStartTime && lastTime < flamesStartTime){
            laserCharge.Play();
            missileLauncher.ExtendCannon();
            flames.gameObject.SetActive(true);
        }

        if (currentTime > gunFireTime && lastTime < gunFireTime)
        {
            GoGoItanoCircus();
        }

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

        lastTime = currentTime;

    }
}
