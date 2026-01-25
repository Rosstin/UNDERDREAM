using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerParent : MonoBehaviour
{
    public BurgGS gamestate;
    public AudioSource triumphSfx;
    public AudioSource flingSfx;
    public GameObject confetti;
    
    private List<Ingredient> myIngredients = new List<Ingredient>();

    private OrderData correctOrder;

    public void ScoreBurger()
    {
        // play a triumphant sfx
        
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

        gamestate.DoneScoring();

    }
    

    public void AddIngredient(Ingredient ingredient)
    {
        ingredient.transform.SetParent(this.transform);
        myIngredients.Add(ingredient);
    }
    
}
