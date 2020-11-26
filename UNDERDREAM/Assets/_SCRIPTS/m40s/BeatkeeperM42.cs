using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatkeeperM42 : Beatkeeper
{

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
                    Hurdle newHurdle = Instantiate(hurdle.gameObject).GetComponent<Hurdle>();
                    StartCoroutine(KickOffHurdle(beatIndexPlayer, newHurdle, beatTimes, rightEdge.transform, leftEdge.transform));
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
