using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatkeeperM45 : Beatkeeper
{
    [Header("Specific to m45")]
    /// <summary>
    /// for the punching bag, in the middle it's all the way down
    /// </summary>
    [SerializeField] private AnimationCurve hurdleCurve;
    [SerializeField] private PunchingBag punchingBag;
    [SerializeField] private SwingingMine swingingMine;

    new void Start()
    {
        base.Start();

        punchingBag.transform.position = rightEdge.transform.position;
    }

    // Update is called once per frame
    new void Update()
    {
        BaseUpdate();

        lastTime = currentTime;
        currentTime = Data.GetTigerSong().time;

        if (currentTime > songSegmentEndTime)
        {
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
                    StartCoroutine(KickOffHurdle(beatIndexPlayer, punchingBag, beatTimes, rightEdge.transform, leftEdge.transform));
                    beatIndexPlayer++;
                }
            }


            // ball and chain
            /*
            if (aTimes.Count > 0 && aIndex < aTimes.Count)
            {
                // the player has to be able to see the hurdles ahead of time
                float startTime = aTimes[aIndex] - beatRadiusVisualHint;
                float endTime = aTimes[aIndex] + beatRadiusVisualHint;

                // kick off the hurdle        
                if (currentTime > startTime)
                {
                    Hurdle newHurdle = Instantiate(a.gameObject).GetComponent<Hurdle>();
                    StartCoroutine(KickOffHurdle(aIndex, punchingBag, aTimes, aStart, aEnd));
                    aIndex++;
                }
            }
            */



        }
    }


    /// <summary>
    /// Kick off something the player needs to interact with
    /// </summary>
    /// <param name="myHurdleIndex"></param>
    /// <param name="myHurdle"></param>
    /// <returns></returns>
    private IEnumerator KickOffHurdle(int myHurdleIndex, PunchingBag myHurdle, List<float> times, Transform start, Transform end)
    {
        bool madeYellow = false;

        float visualStartTime = times[myHurdleIndex] - beatRadiusVisualHint;
        float visualEndTime = times[myHurdleIndex] + beatRadiusVisualHint;

        float inputStartTime = times[myHurdleIndex] - beatInputLead;
        float inputEndTime = times[myHurdleIndex] + beatInputLag;

        bool inputSuccess = false;
        bool missed = false;

        Command correctCode = Command.Fire;
        if (myHurdle.CorrectCommand == "up")
        {
            correctCode = Command.Up;
        }else if(myHurdle.CorrectCommand == "down")
        {
            correctCode = Command.Down;
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
                (CommandsStartedThisFrame.ContainsKey(correctCode)))
            {
                success.Play();
                inputSuccess = true;

                // make the hurdle green
                myHurdle.MakeCorrect();
                myHurdle.TakeDamage();

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

            // the punching bag hurdle moves up and then down, it's at the end in middle time

            float hurdleProgress = (currentTime - visualStartTime) / (visualEndTime - visualStartTime);

            float prog = hurdleCurve.Evaluate(hurdleProgress);

            myHurdle.transform.position = Vector3.Lerp(start.position, end.position, prog);

            yield return 0;
        }

        if (numMistakes > allowedMistakes)
        {
            StartCoroutine(Failure());
        }

        yield return new WaitForSeconds(0.2f);
    }

}
