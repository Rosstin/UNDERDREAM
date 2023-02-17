using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beatkeeper : BaseController
{
    [Header("Timing")]
    [SerializeField] protected List<float> beatTimes;
    [SerializeField] protected List<float> aTimes;
    [SerializeField] protected float beatRadiusVisualHint;
    [SerializeField] protected float beatInputLead;
    [SerializeField] protected float beatInputLag;
    [SerializeField] protected float wipeoutBuffer;
    [SerializeField] protected float songSegmentStartTime;
    [SerializeField] protected float songSegmentEndTime;

    [Header("SFX")]
    [SerializeField] protected AudioSource success;
    [SerializeField] protected AudioSource mistake;
    [SerializeField] protected AudioSource wipeout;

    [Header("Hurdle")]
    [SerializeField] protected Hurdle hurdle;
    [SerializeField] protected GameObject leftEdge;
    [SerializeField] protected GameObject rightEdge;

    [Header("A Object")]
    [SerializeField]
    protected Hurdle a;
    [SerializeField] protected Transform aStart;
    [SerializeField] protected Transform aEnd;
    [SerializeField] protected Transform aDisappearSpot;

    [Header("Mistakes")]
    [SerializeField]
    protected int allowedMistakes;

    [Header("Blackout")]
    [SerializeField] protected Cloudloop blackout;

    [Header("Output Text")]
    [SerializeField]
    protected TMPro.TextMeshPro outputText;
    [SerializeField] protected List<string> successShouts;
    [SerializeField] protected List<string> missShouts;
    [SerializeField] protected List<string> failureShouts;

    protected int lastSuccessShout;
    protected int lastMissShout;
    protected int lastFailureShout;

    protected float lastTime = 0f;

    protected int beatIndexPlayer = 0;
    protected int beatIndexInternal = 0;

    protected int aIndex = 0;

    protected bool itsOver = false;

    protected int numMistakes = 0;

    protected float currentTime = 0f;

    protected void Start()
    {
        base.Start();
        blackout.gameObject.SetActive(false);
        blackout.enabled = false;

        if (!Data.TigerSongExists())
        {
            Data.CreateTigerSong();
        }

        // if you're out of sync (maybe because this is a redo, or big slowdown), reset time to correct spot
        if(Data.GetTigerSong().time > songSegmentStartTime + 3 ||
            Data.GetTigerSong().time < songSegmentStartTime - 3
            )
        {
            Data.GetTigerSong().time = (songSegmentStartTime);
        }

        if (!Data.GetTigerSong().isPlaying)
        {
            Data.GetTigerSong().Play();
        }
    }

    protected IEnumerator Failure()
    {
        // shout a failure shout
        int failShoutIndex = Random.Range(0, failureShouts.Count);
        if (lastFailureShout == failShoutIndex)
        {
            failShoutIndex++;
            if (failShoutIndex >= failureShouts.Count)
            {
                failShoutIndex = 0;
            }
        }
        outputText.text = failureShouts[failShoutIndex];
        lastFailureShout = failShoutIndex;

        this.enabled = (false); // stop the update loop
        Data.GetTigerSong().Stop();
        blackout.gameObject.SetActive(true);
        blackout.enabled = true;
        wipeout.Play();
        yield return new WaitForSeconds(wipeout.clip.length + wipeoutBuffer);
        Restart();
    }

    /// <summary>
    /// kick off a visual flair on time to beat
    /// </summary>
    /// <param name="index"></param>
    /// <param name="beatObject"></param>
    /// <returns></returns>
    protected IEnumerator KickOffBeatObject(int index, BeatObject beatObject)
    {
        bool madeYellow = false;

        float visualStartTime = aTimes[index] - beatRadiusVisualHint;
        float visualEndTime = aTimes[index] + beatRadiusVisualHint;

        while (currentTime < visualEndTime)
        {
            float hurdleProgress = (currentTime - visualStartTime) / (visualEndTime - visualStartTime);
            beatObject.transform.position = Vector3.Lerp(aStart.transform.position, aEnd.transform.position, hurdleProgress);

            yield return 0;
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(beatObject.gameObject);
    }


    /// <summary>
    /// Kick off something the player needs to interact with
    /// </summary>
    /// <param name="myHurdleIndex"></param>
    /// <param name="myHurdle"></param>
    /// <returns></returns>
    protected IEnumerator KickOffHurdle(int myHurdleIndex, Hurdle myHurdle)
    {
        bool madeYellow = false;

        float visualStartTime = beatTimes[myHurdleIndex] - beatRadiusVisualHint;
        float visualEndTime = beatTimes[myHurdleIndex] + beatRadiusVisualHint;

        float inputStartTime = beatTimes[myHurdleIndex] - beatInputLead;
        float inputEndTime = beatTimes[myHurdleIndex] + beatInputLag;

        bool inputSuccess = false;
        bool missed = false;

        while (currentTime < visualEndTime)
        {
            if(currentTime > inputStartTime && !madeYellow)
            {
                madeYellow = true;
                myHurdle.MakeReady(); 
            }

            // success
            if(currentTime > inputStartTime && currentTime < inputEndTime &&  
                CommandsStartedThisFrame.ContainsKey(Command.Up)){
                success.Play();
                inputSuccess = true;

                // make the hurdle green
                myHurdle.MakeCorrect();

                // shout a success shout
                int successShoutIndex = Random.Range(0, successShouts.Count);
                if(lastSuccessShout == successShoutIndex)
                {
                    successShoutIndex++;
                    if(successShoutIndex >= successShouts.Count)
                    {
                        successShoutIndex = 0;
                    }
                }
                outputText.text = successShouts[successShoutIndex];
                lastSuccessShout = successShoutIndex;
            }

            // miss..
            else if(currentTime > inputEndTime && !inputSuccess && !missed)
            {
                missed = true;

                // todo toss the hurdle
                myHurdle.MakeWrong();

                // shout a miss shout
                int missShoutIndex = Random.Range(0, missShouts.Count);
                if (lastMissShout == missShoutIndex)
                {
                    missShoutIndex++;
                    if (missShoutIndex >= missShouts.Count)
                    {
                        missShoutIndex = 0;
                    }
                }
                outputText.text = missShouts[missShoutIndex];
                lastMissShout = missShoutIndex;

                numMistakes++;
                mistake.Play();
            }

            float hurdleProgress = (currentTime - visualStartTime) / (visualEndTime-visualStartTime);
            myHurdle.transform.position = Vector3.Lerp(rightEdge.transform.position, leftEdge.transform.position, hurdleProgress);

            yield return 0;
        }

        if(numMistakes > allowedMistakes)
        {
            StartCoroutine(Failure());
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(myHurdle.gameObject);
    }

    protected void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    protected void Update()
    {
        BaseUpdate();

        lastTime = currentTime;
        currentTime = Data.GetTigerSong().time;

        if ( currentTime > songSegmentEndTime)
        {
            LoadNextScene();
        }

        if (!itsOver)
        {

            if(beatIndexPlayer < beatTimes.Count)
            {
                // the player has to be able to see the hurdles ahead of time
                float startTime = beatTimes[beatIndexPlayer] - beatRadiusVisualHint;
                float endTime = beatTimes[beatIndexPlayer] + beatRadiusVisualHint;

                // kick off the hurdle        
                if (currentTime > startTime)
                {
                    Hurdle newHurdle = Instantiate(hurdle.gameObject).GetComponent<Hurdle>();
                    StartCoroutine(KickOffHurdle(beatIndexPlayer, newHurdle));
                    beatIndexPlayer++;
                }
            }

            if(aTimes.Count > 0 && aIndex < aTimes.Count)
            {
                // the player has to be able to see the hurdles ahead of time
                float startTime = beatTimes[aIndex] - beatRadiusVisualHint;
                float endTime = beatTimes[aIndex] + beatRadiusVisualHint;

                // kick off the hurdle        
                if (currentTime > startTime)
                {
                    BeatObject newHurdle = Instantiate(a.gameObject).GetComponent<BeatObject>();
                    StartCoroutine(KickOffBeatObject(aIndex, newHurdle));
                    aIndex++;
                }
            }

        }
    }
}
