using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beatkeeper : MonoBehaviour
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
    [SerializeField] private GameObject hurdle;
    [SerializeField] private GameObject leftEdge;
    [SerializeField] private GameObject rightEdge;

    [Header("Mistakes")]
    [SerializeField] private int allowedMistakes;

    private float lastTime = 0f;
    private int beatIndexPlayer = 0;
    private int beatIndexInternal = 0;
    private bool itsOver = false;

    private int numMistakes = 0;

    private float currentTime = 0f;

    private IEnumerator KickOffHurdle(int myHurdleIndex, GameObject myHurdle)
    {
        float visualStartTime = beatTimes[myHurdleIndex] - beatRadiusVisualHint;
        float visualEndTime = beatTimes[myHurdleIndex] + beatRadiusVisualHint;

        float inputStartTime = beatTimes[myHurdleIndex] - beatInputLead;
        float inputEndTime = beatTimes[myHurdleIndex] + beatInputLag;

        bool inputSuccess = false;

        while (currentTime < visualEndTime)
        {
            if(currentTime > inputStartTime && currentTime < inputEndTime &&  
                Input.GetKeyDown(KeyCode.UpArrow)){
                success.Play();
                inputSuccess = true;
            }

            float hurdleProgress = (currentTime - visualStartTime) / (visualEndTime-visualStartTime);
            myHurdle.transform.position = Vector3.Lerp(rightEdge.transform.position, leftEdge.transform.position, hurdleProgress);

            yield return 0;
        }

        if (!inputSuccess)
        {
            // todo toss the hurdle
            numMistakes++;
            mistake.Play();
        }

        if(numMistakes > allowedMistakes)
        {
            wipeout.Play();
            yield return new WaitForSeconds(wipeout.clip.length+wipeoutBuffer);
            Restart();
        }

        Destroy(myHurdle);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        lastTime = currentTime;
        currentTime = song.time;

        if (!song.isPlaying)
        {
            // restart // todo go to next scene instead
            Restart();
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
                    GameObject newHurdle = Instantiate(hurdle);
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
