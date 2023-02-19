using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRocking : MonoBehaviour
{
    [Header("Outlets")]
    [SerializeField] private GameObject deck;

    [Header("Animation Constants")]
    [SerializeField] private float maxRockAngle;
    [SerializeField] private float rockPeriodSeconds;

    private int rockDirection = 1;
    private float startingLocalRotZ;
    private float currentTime = 0;

    private void Start()
    {
        startingLocalRotZ = deck.transform.localRotation.z;
    }

    void Update()
    {
        if (rockDirection == 1)
        {
            currentTime += Time.deltaTime;

            if (currentTime < rockPeriodSeconds)
            {
                float timeRatio = currentTime / rockPeriodSeconds;

                float localRotQuatZ =
                    startingLocalRotZ + timeRatio * maxRockAngle;

                deck.transform.localRotation =
                    new Quaternion(
                        deck.transform.localRotation.x,
                        deck.transform.localRotation.y,
                        localRotQuatZ,
                        deck.transform.localRotation.w
                    );
            }
            else
            {
                rockDirection = -1;
            }
        }
        else if (rockDirection == -1)
        {
            currentTime -= Time.deltaTime;

            if (currentTime > -rockPeriodSeconds)
            {
                float timeRatio = currentTime / rockPeriodSeconds;

                float localRotQuatZ =
                    startingLocalRotZ + timeRatio * maxRockAngle;

                deck.transform.localRotation =
                    new Quaternion(
                        deck.transform.localRotation.x,
                        deck.transform.localRotation.y,
                        localRotQuatZ,
                        deck.transform.localRotation.w
                    );
            }
            else
            {
                rockDirection = 1;
            }
        }
        else
        {
            Debug.LogError("Rock direction should be 1 or -1: " + rockDirection);
        }


    }
}
