using System.Collections.Generic;
using UnityEngine;

public class IngredientTray : MonoBehaviour
{
    public IngredientSpawner LeftIngredient;
    public IngredientSpawner CenterIngredient;
    public IngredientSpawner RightIngredient;

    public void SetIngs(List<Ingredient.IngredientTypes> ingredients)
    {
        LeftIngredient.MakeIngredient(ingredients[0]);
        CenterIngredient.MakeIngredient(ingredients[1]);
        RightIngredient.MakeIngredient(ingredients[2]);
    }
}
