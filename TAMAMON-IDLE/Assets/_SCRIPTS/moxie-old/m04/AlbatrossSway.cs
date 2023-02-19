using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbatrossSway : MonoBehaviour
{
    [Header("Outlets")]
    [SerializeField]
    private GameObject swayee;

    [Header("Animation Constants")]
    [SerializeField]
    private float maxValue;
    [SerializeField] private float period;

    private int dir = 1;
    private float startingValue;
    private float currentTime = 0;

    private void Start()
    {
        startingValue = swayee.transform.localPosition.y;
    }

    void Set(Vector3 setee, Vector3 setVal)
    {

    }

    void Update()
    {
        if (dir == 1)
        {
            currentTime += Time.deltaTime;

            if (currentTime < period)
            {
                float timeRatio = currentTime / period;

                Vector3 newValue =
                    new Vector3(
                        swayee.transform.localPosition.x,
                        startingValue + timeRatio * maxValue,
                        swayee.transform.localPosition.z);

                swayee.transform.localPosition = newValue;
            }
            else
            {
                dir = -1;
            }
        }
        else if (dir == -1)
        {
            currentTime -= Time.deltaTime;

            if (currentTime > -period)
            {
                float timeRatio = currentTime / period;

                Vector3 newValue =
                    new Vector3(
                        swayee.transform.localPosition.x,
                        startingValue + timeRatio * maxValue,
                        swayee.transform.localPosition.z);

                swayee.transform.localPosition = newValue;
            }
            else
            {
                dir = 1;
            }
        }
        else
        {
            Debug.LogError("direction should be 1 or -1: " + dir);
        }


    }
}
