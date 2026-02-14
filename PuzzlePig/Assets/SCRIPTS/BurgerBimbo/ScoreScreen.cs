using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    [Header("Score Stars")]
    public List<ScoreStar> scoreStars  = new List<ScoreStar>();
    public ScoreStar secretStar;

    [Header("Star Rotation")]
    public Vector3 rotationSpeed; 

    
    // Update is called once per frame
    void Update()
    {
        // rotate the stars
        foreach (ScoreStar star in scoreStars)
        {
            star.transform.Rotate(rotationSpeed*Time.deltaTime);
        }
        secretStar.transform.Rotate(rotationSpeed*Time.deltaTime);
    }
}
