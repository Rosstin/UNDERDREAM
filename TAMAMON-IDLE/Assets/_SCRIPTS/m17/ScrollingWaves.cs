using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingWaves : MonoBehaviour
{
    public float Speed;
    public Vector2 MinMaxX;

    public SpriteRenderer[] Waves;

    void Update()
    {
        foreach (SpriteRenderer wave in Waves)
        {
            wave.transform.position +=
                new Vector3(
                    Speed * Time.deltaTime,
                    0,
                    0
                );

            if (wave.transform.position.x < MinMaxX.x)
            {
                wave.transform.position
                    =
                    new Vector3(
                        MinMaxX.y,
                        wave.transform.position.y,
                        wave.transform.position.z
                            );
                    
            }

            if (wave.transform.position.x > MinMaxX.y)
            {
                wave.transform.position
                    =
                    new Vector3(
                        MinMaxX.x,
                        wave.transform.position.y,
                        wave.transform.position.z
                    );
            }
        }
    }
}
