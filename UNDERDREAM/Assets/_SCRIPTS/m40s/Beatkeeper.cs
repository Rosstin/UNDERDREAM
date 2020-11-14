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
    private float currentTime = 0f;
    private int beatIndex = 0;
    private bool itsOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!song.isPlaying)
        {
            // restart
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (!itsOver)
        {
            lastTime = currentTime;
            currentTime = song.time;

            float startTime = beatTimes[beatIndex] - beatRadiusPlayerInput;
            float endTime = beatTimes[beatIndex] + beatRadiusPlayerInput;

            if (currentTime > startTime && currentTime < endTime)
            {
                float hurdleProgress = (currentTime - startTime) / (beatRadiusPlayerInput);
                hurdle.transform.position = Vector3.Lerp(rightEdge.transform.position, leftEdge.transform.position, hurdleProgress);
            }

            // play a beat on the beat
            if (
                currentTime > beatTimes[beatIndex]
                &&
                lastTime < beatTimes[beatIndex]
                )
            {
                success.Play();
                beatIndex++;
                if (beatIndex >= beatTimes.Count)
                {
                    itsOver = true;
                }
            }
        }
    }
}
