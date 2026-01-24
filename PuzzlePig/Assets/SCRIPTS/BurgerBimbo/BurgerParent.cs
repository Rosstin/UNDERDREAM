using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerParent : MonoBehaviour
{
    private List<Ingredient> myIngredients = new List<Ingredient>();

    private OrderData correctOrder;

    public void ScoreBurger()
    {

        foreach (Ingredient ingredient in myIngredients)
        {
            ingredient.Score();
        }
        
        myIngredients.Clear();
        
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ingredient.transform.SetParent(this.transform);
        myIngredients.Add(ingredient);
    }
    
}
