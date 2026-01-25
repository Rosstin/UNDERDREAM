using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BurgerParent : MonoBehaviour
{
    public BurgGS gamestate;
    public AudioSource triumphSfx;
    public AudioSource flingSfx;
    public GameObject confetti;
    
    private List<Ingredient> myIngredients = new List<Ingredient>();

    
    private TextMeshProUGUI scoreTextRef;
    
    #region scoring
    private Dictionary<Ingredient.IngredientTypes,int> actualTypesToNumbers = new Dictionary<Ingredient.IngredientTypes,int>();
    #endregion
    
    private OrderData correctOrder;

    public void ScoreBurger(Dictionary<Ingredient.IngredientTypes,int> idealTypesToNumbers, TextMeshProUGUI scoreText)
    {
        // play a triumphant sfx
        // calculate accuracy score
        // collection of parts vs what's in the burger

        scoreTextRef = scoreText;

        int idealTotalIngs = 0;
        int totalCorrectIngs = 0;
        foreach (var key in idealTypesToNumbers.Keys)
        {
            var ingType = key;
            int idealNumOfType = idealTypesToNumbers[key];

            int actualNumOfType = 0;
            if (actualTypesToNumbers.ContainsKey(ingType) == false)
            {
                // skip - must not have obtained
            }
            else
            {
                actualNumOfType = actualTypesToNumbers[key];
            }

            totalCorrectIngs += Mathf.Min(actualNumOfType, idealNumOfType);
            idealTotalIngs += idealNumOfType;
        }

        
        float correctPercent = (((float)totalCorrectIngs / (float)idealTotalIngs));
        string formattedPercent = String.Format("{0:P0}.", correctPercent);

        scoreTextRef.text = "ACCURACY: " + totalCorrectIngs + " / " + idealTotalIngs + " : " + formattedPercent;
        
        StartCoroutine(ScoreCoroutine());
        
        
    }

    private IEnumerator ScoreCoroutine()
    {
        triumphSfx.Play();
        confetti.SetActive(true);
        // particles

        yield return new WaitForSeconds(2f);

        
        // generate fling force
        float leftRightDir = 1f;
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            leftRightDir = -1f;
        }
        Vector3 flingForce =
            new Vector3(
                (leftRightDir*UnityEngine.Random.Range(0f,100f)), 
            300f, 
            1 * UnityEngine.Random.Range(400f, 600f)
        );
        
        foreach (Ingredient ingredient in myIngredients)
        {
            // give a little fuzzing for each ingredient

            var individualFlingForce = new Vector3(flingForce.x + Random.Range(-50f,50f), flingForce.y + Random.Range(-50f,50f), flingForce.z + Random.Range(-50f,50f));
            
            
            flingSfx.Play();
            ingredient.Fling(individualFlingForce);
        }
        
        yield return new WaitForSeconds(2f);

        confetti.SetActive(false);

        foreach (Ingredient ingredient in myIngredients)
        {
            ingredient.Score();
        }
        
        myIngredients.Clear();
        actualTypesToNumbers.Clear();

        scoreTextRef.text = "";

        gamestate.DoneScoring();

    }
    

    public void AddIngredient(Ingredient ingredient)
    {
        ingredient.transform.SetParent(this.transform);
        myIngredients.Add(ingredient);
        
        if (!actualTypesToNumbers.TryAdd(ingredient.GetIngredientType(), 1))
        {
            actualTypesToNumbers[ingredient.GetIngredientType()] += 1;
        }
    }
    
}
