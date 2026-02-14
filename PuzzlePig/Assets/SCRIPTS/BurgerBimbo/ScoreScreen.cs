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

    public void HideAllStars()
    {
        foreach (ScoreStar star in scoreStars)
        {
            star.gameObject.SetActive(false);
        }
        
        secretStar.gameObject.SetActive(false);
    }
    
    public void SetScore(int starsToAward, bool gotSecretStar)
    {
        Debug.Log("stars to award: " + starsToAward + " and secret " + gotSecretStar);
        
        HideAllStars();
        
        if (starsToAward >= 1)
        {
            scoreStars[0].gameObject.SetActive(true);
        }
        if (starsToAward >= 2)
        {
            scoreStars[1].gameObject.SetActive(true);
        }
        if (starsToAward >= 3)
        {
            scoreStars[2].gameObject.SetActive(true);
        }

        if (gotSecretStar)
        {
            secretStar.gameObject.SetActive(true);
        }
        
        
    }
}
