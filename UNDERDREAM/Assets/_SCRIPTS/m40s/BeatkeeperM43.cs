﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatkeeperM43 : BeatkeeperM42
{
    /// <summary>
    /// Kick off something the player needs to interact with
    /// </summary>
    /// <param name="myHurdleIndex"></param>
    /// <param name="myHurdle"></param>
    /// <returns></returns>
    private IEnumerator KickOffHurdle(int myHurdleIndex, Hurdle myHurdle, List<float> times, Transform start, Transform end, bool isFish)
    {

        Hurdle mirrorHurdle = Instantiate(myHurdle.gameObject).GetComponent<Hurdle>();
        Vector3 mirrorStart = new Vector3(-start.position.x, start.position.y, start.position.z);
        Vector3 mirrorEnd = new Vector3(-end.position.x, end.position.y, end.position.z);
        mirrorHurdle.transform.localScale = new Vector3(-1, 1, 1);

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
        }
        else if (myHurdle.CorrectCommand == "down")
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
                mirrorHurdle.MakeReady();
            }

            // success
            if (currentTime > inputStartTime && currentTime < inputEndTime &&
                (CommandsStartedThisFrame.ContainsKey(correctCode)))
            {
                success.Play();
                inputSuccess = true;

                // make the hurdle green

                myHurdle.MakeCorrect();
                mirrorHurdle.MakeCorrect();
                if (isFish)
                {
                    myHurdle.gameObject.SetActive(false);
                    mirrorHurdle.gameObject.SetActive(false);
                }

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
                mirrorHurdle.MakeWrong();

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
            myHurdle.transform.position = Vector3.Lerp(start.position, end.position , hurdleProgress);
            mirrorHurdle.transform.position = Vector3.Lerp(  mirrorStart, mirrorEnd, hurdleProgress);

            yield return 0;
        }

        if (numMistakes > allowedMistakes)
        {
            StartCoroutine(Failure());
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(myHurdle.gameObject);
        Destroy(mirrorHurdle.gameObject);
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
                    StartCoroutine(KickOffHurdle(beatIndexPlayer, newHurdle, beatTimes, rightEdge.transform, leftEdge.transform, false));
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
                    StartCoroutine(KickOffHurdle(aIndex, newHurdle, aTimes, aStart, aEnd, true));
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
