using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Beatkeeper : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private List<float> beatTimes;
    [SerializeField] private float beatRadiusPlayerInput;

    [Header("Song")]
    [SerializeField] private AudioSource song;

    [Header("SFX")]
    [SerializeField] private AudioSource success;
    [SerializeField] private AudioSource mistake;

    [Header("Hurdle")]
    [SerializeField] private GameObject hurdle;
    [SerializeField] private GameObject leftEdge;
    [SerializeField] private GameObject rightEdge;

    private float lastTime = 0f;
    private int beatIndexPlayer = 0;
    private int beatIndexInternal = 0;
    private bool itsOver = false;

    private float currentTime = 0f;

    private IEnumerator KickOffHurdle(int myHurdleIndex, GameObject myHurdle)
    {
        while (true)
        {
            float startTime = beatTimes[myHurdleIndex] - beatRadiusPlayerInput;
            float endTime = beatTimes[myHurdleIndex] + beatRadiusPlayerInput;

            float hurdleProgress = (currentTime - startTime) / (endTime-startTime);
            myHurdle.transform.position = Vector3.Lerp(rightEdge.transform.position, leftEdge.transform.position, hurdleProgress);

            if(currentTime > endTime)
            {
                Destroy(myHurdle);
                break;
            }

            yield return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lastTime = currentTime;
        currentTime = song.time;

        if (!song.isPlaying)
        {
            // restart
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (!itsOver)
        {

            if(beatIndexPlayer < beatTimes.Count)
            {
                // the player has to be able to see the hurdles ahead of time
                float startTime = beatTimes[beatIndexPlayer] - beatRadiusPlayerInput;
                float endTime = beatTimes[beatIndexPlayer] + beatRadiusPlayerInput;

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
