using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatkeeperM42 : BaseController
{
    [Header("Timing")]
    [SerializeField]
    private List<float> beatTimes;
    [SerializeField] private List<float> aTimes;
    [SerializeField] private float beatRadiusVisualHint;
    [SerializeField] private float beatInputLead;
    [SerializeField] private float beatInputLag;
    [SerializeField] private float wipeoutBuffer;

    [Header("Song")]
    [SerializeField]
    private AudioSource song;

    [Header("SFX")]
    [SerializeField]
    private AudioSource success;
    [SerializeField] private AudioSource mistake;
    [SerializeField] private AudioSource wipeout;

    [Header("Hurdle")]
    [SerializeField]
    private Hurdle hurdle;
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    [Header("A Object")]
    [SerializeField]
    private Hurdle a;
    [SerializeField] private Transform aStart;
    [SerializeField] private Transform aEnd;

    [Header("Mistakes")]
    [SerializeField]
    private int allowedMistakes;

    [Header("Blackout")]
    [SerializeField]
    private Cloudloop blackout;

    [Header("Output Text")]
    [SerializeField]
    private TMPro.TextMeshPro outputText;
    [SerializeField] private List<string> successShouts;
    [SerializeField] private List<string> missShouts;
    [SerializeField] private List<string> failureShouts;

    private int lastSuccessShout;
    private int lastMissShout;
    private int lastFailureShout;

    private float lastTime = 0f;

    private int beatIndexPlayer = 0;
    private int beatIndexInternal = 0;

    private int aIndex = 0;

    private bool itsOver = false;

    private int numMistakes = 0;

    private float currentTime = 0f;

    private void Start()
    {
        base.Start();
        blackout.gameObject.SetActive(false);
        blackout.enabled = false;
    }

    private IEnumerator Failure()
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
        song.Stop();
        blackout.gameObject.SetActive(true);
        blackout.enabled = true;
        wipeout.Play();
        yield return new WaitForSeconds(wipeout.clip.length + wipeoutBuffer);
        Restart();
    }

    /*
    /// <summary>
    /// kick off a visual flair on time to beat
    /// </summary>
    /// <param name="index"></param>
    /// <param name="beatObject"></param>
    /// <returns></returns>
    private IEnumerator KickOffBeatObject(int index, BeatObject beatObject)
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
    */

    /// <summary>
    /// Kick off something the player needs to interact with
    /// </summary>
    /// <param name="myHurdleIndex"></param>
    /// <param name="myHurdle"></param>
    /// <returns></returns>
    private IEnumerator KickOffHurdle(int myHurdleIndex, Hurdle myHurdle, List<float> times, Transform start, Transform end)
    {
        bool madeYellow = false;

        float visualStartTime = times[myHurdleIndex] - beatRadiusVisualHint;
        float visualEndTime = times[myHurdleIndex] + beatRadiusVisualHint;

        float inputStartTime = times[myHurdleIndex] - beatInputLead;
        float inputEndTime = times[myHurdleIndex] + beatInputLag;

        bool inputSuccess = false;
        bool missed = false;

        KeyCode correctCode = KeyCode.Space;
        if (myHurdle.CorrectCommand == "up")
        {
            correctCode = KeyCode.UpArrow;
        }else if(myHurdle.CorrectCommand == "down")
        {
            correctCode = KeyCode.DownArrow;
        }
        else
        {
            Debug.LogError("unhandled code!");
        }

        while (currentTime < visualEndTime)
        {
            if (currentTime > inputStartTime && !madeYellow)
            {
                madeYellow = true;
                myHurdle.MakeReady();
            }

            // success
            if (currentTime > inputStartTime && currentTime < inputEndTime &&
                (Input.GetKeyDown(correctCode)))
            {
                success.Play();
                inputSuccess = true;

                // make the hurdle green
                myHurdle.MakeCorrect();

                // shout a success shout
                int successShoutIndex = Random.Range(0, successShouts.Count);
                if (lastSuccessShout == successShoutIndex)
                {
                    successShoutIndex++;
                    if (successShoutIndex >= successShouts.Count)
                    {
                        successShoutIndex = 0;
                    }
                }
                outputText.text = successShouts[successShoutIndex];
                lastSuccessShout = successShoutIndex;
            }

            // miss..
            else if (currentTime > inputEndTime && !inputSuccess && !missed)
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

            float hurdleProgress = (currentTime - visualStartTime) / (visualEndTime - visualStartTime);
            myHurdle.transform.position = Vector3.Lerp(start.position, end.position, hurdleProgress);

            yield return 0;
        }

        if (numMistakes > allowedMistakes)
        {
            StartCoroutine(Failure());
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(myHurdle.gameObject);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        lastTime = currentTime;
        currentTime = song.time;

        if (!song.isPlaying)
        {
            // restart // todo go to next scene instead
            LoadNextScene();
        }

        if (!itsOver)
        {

            if (beatIndexPlayer < beatTimes.Count)
            {
                // the player has to be able to see the hurdles ahead of time
                float startTime = beatTimes[beatIndexPlayer] - beatRadiusVisualHint;
                float endTime = beatTimes[beatIndexPlayer] + beatRadiusVisualHint;

                // kick off the hurdle        
                if (currentTime > startTime)
                {
                    Hurdle newHurdle = Instantiate(hurdle.gameObject).GetComponent<Hurdle>();
                    StartCoroutine(KickOffHurdle(beatIndexPlayer, newHurdle, beatTimes, start.transform, end.transform));
                    beatIndexPlayer++;
                }
            }

            if (aTimes.Count > 0 && aIndex < aTimes.Count)
            {
                // the player has to be able to see the hurdles ahead of time
                float startTime = aTimes[aIndex] - beatRadiusVisualHint;
                float endTime = aTimes[aIndex] + beatRadiusVisualHint;

                // kick off the hurdle        
                if (currentTime > startTime)
                {
                    Hurdle newHurdle = Instantiate(a.gameObject).GetComponent<Hurdle>();
                    StartCoroutine(KickOffHurdle(aIndex, newHurdle, aTimes, aStart, aEnd));
                    aIndex++;
                }
            }

        }
    }
}
