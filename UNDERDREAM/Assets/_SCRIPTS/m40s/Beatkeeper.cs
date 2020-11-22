using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beatkeeper : BaseController
{
    [Header("Timing")]
    [SerializeField] private List<float> beatTimes;
    [SerializeField] private float beatRadiusVisualHint;
    [SerializeField] private float beatInputLead;
    [SerializeField] private float beatInputLag;
    [SerializeField] private float wipeoutBuffer;

    [Header("Song")]
    [SerializeField] private AudioSource song;

    [Header("SFX")]
    [SerializeField] private AudioSource success;
    [SerializeField] private AudioSource mistake;
    [SerializeField] private AudioSource wipeout;

    [Header("Hurdle")]
    [SerializeField] private Hurdle hurdle;
    [SerializeField] private GameObject leftEdge;
    [SerializeField] private GameObject rightEdge;

    [Header("Mistakes")]
    [SerializeField] private int allowedMistakes;

    [Header("Blackout")]
    [SerializeField] private Cloudloop blackout;

    [Header("Output Text")]
    [SerializeField] private TMPro.TextMeshPro outputText;
    [SerializeField] private List<string> successShouts;
    [SerializeField] private List<string> missShouts;
    [SerializeField] private List<string> failureShouts;

    private int lastSuccessShout;
    private int lastMissShout;
    private int lastFailureShout;

    private float lastTime = 0f;
    private int beatIndexPlayer = 0;
    private int beatIndexInternal = 0;
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

    private IEnumerator KickOffHurdle(int myHurdleIndex, Hurdle myHurdle)
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
                myHurdle.MakeYellow(); 
            }

            // success
            if(currentTime > inputStartTime && currentTime < inputEndTime &&  
                Input.GetKeyDown(KeyCode.UpArrow)){
                success.Play();
                inputSuccess = true;

                // make the hurdle green
                myHurdle.MakeGreen();

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
                myHurdle.MakeRed();

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

            // play a beat on the beat
            /*
            if (
                currentTime > beatTimes[beatIndexInternal]
                &&
                lastTime < beatTimes[beatIndexInternal]
                )
            {
                success.Play();
                beatIndexInternal++;
                if (beatIndexInternal >= beatTimes.Count)
                {
                    itsOver = true;
                }
            }
            */
        }
    }
}
