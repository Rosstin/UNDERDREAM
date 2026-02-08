using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BurgerParent : MonoBehaviour
{
    [Header("Outlets")]
    public BurgGS gamestate;
    public AudioSource triumphSfx;
    public AudioSource flingSfx;
    public GameObject confetti;
    
    private List<Ingredient> myIngredients = new List<Ingredient>();
    private List<Ingredient> missedIngs = new List<Ingredient>();
    private List<Ingredient> shiftMissedIngs = new List<Ingredient>();
    
    #region scoring
    private Dictionary<Ingredient.IngredientTypes,int> actualTypesToNumbers = new Dictionary<Ingredient.IngredientTypes,int>();
    #endregion
    
    private OrderData correctOrder;

    public IEnumerator ScoreShift()
    {

        gamestate.missedText.text = "Missed: ";
        
        bool completeBurgerMa = OrganizeMissedIngredients();
        
        for(int i =0; i < shiftMissedIngs.Count;i++)
        {
            var ing =  shiftMissedIngs[i];
            
            gamestate.orderPreviewWindow.AddIngredient(ing.GetIngredientType());
            gamestate.soundManager.PlayPop();

            gamestate.missedText.text = "Missed: " + (i+1);
            
            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(ScoreShiftCoroutine());
        
        if (completeBurgerMa)
        {
            gamestate.bonusText.text = "Bonus!\n  Stole burger!";
        }


    }

    
    public IEnumerator ScoreBurger(Dictionary<Ingredient.IngredientTypes,int> idealTypesToNumbers, TextMeshProUGUI scoreText, TextMeshProUGUI missedText)
    {
        gamestate.PauseTimer();
        
        // play a triumphant sfx
        // calculate accuracy score
        // collection of parts vs what's in the burger

        
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
        string formattedPercent = String.Format("{0:P0}", correctPercent);


        gamestate.orderPreviewWindow.Clear();
        
        gamestate.scoreText.text = "" + totalCorrectIngs + " / " + idealTotalIngs + " : " + formattedPercent;
        gamestate.missedText.text = "Missed: " + missedIngs.Count.ToString();

        shiftMissedIngs.AddRange(missedIngs);
        
        StartCoroutine(ScoreCoroutine());

        yield return 0;
    }
    
    

    /// <summary>
    /// Organize the ingredients, and check if we assembled a complete burger
    /// </summary>
    /// <returns></returns>
    private bool OrganizeMissedIngredients()
    {
        List<Ingredient> bunBots = new List<Ingredient>();
        List<Ingredient> bunTops = new List<Ingredient>();
        List<Ingredient> otherIngs = new List<Ingredient>();
        List<Ingredient> sortedIngs = new List<Ingredient>();
        
        foreach (Ingredient ingredient in shiftMissedIngs)
        {
            if(ingredient.GetIngredientType() == Ingredient.IngredientTypes.BunBottom)
            {
                bunBots.Add(ingredient);
            }else if (ingredient.GetIngredientType() == Ingredient.IngredientTypes.BunTop)
            {
                bunTops.Add(ingredient);
            }
            else
            {
                otherIngs.Add(ingredient);
            }
        }
        
        sortedIngs.AddRange(bunBots);
        sortedIngs.AddRange(otherIngs);
        sortedIngs.AddRange(bunTops);

        shiftMissedIngs = sortedIngs;

        if (bunTops.Count > 0 && bunBots.Count > 0 && otherIngs.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    private IEnumerator ScoreShiftCoroutine()
    {
        triumphSfx.Play();
        confetti.SetActive(true);
        yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(2f);

        confetti.SetActive(false);
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
        
        
        ResetState();

        gamestate.DoneScoring();

    }

    private void ResetState()
    {
        missedIngs.Clear();
        myIngredients.Clear();
        actualTypesToNumbers.Clear();
        gamestate.ClearScoreText();
    }
    

    public void AddIngredient(Ingredient ingredient)
    {
        ingredient.transform.SetParent(gamestate.servingPlate.transform);
        myIngredients.Add(ingredient);
        
        if (!actualTypesToNumbers.TryAdd(ingredient.GetIngredientType(), 1))
        {
            actualTypesToNumbers[ingredient.GetIngredientType()] += 1;
        }
    }

    public void MissIngredient(Ingredient ingredient)
    {
        missedIngs.Add(ingredient);
    }
}
