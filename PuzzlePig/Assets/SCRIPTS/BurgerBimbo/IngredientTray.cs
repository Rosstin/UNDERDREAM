using System.Collections.Generic;
using UnityEngine;

public class IngredientTray : MonoBehaviour
{
    public IngredientSpawner LeftIngredient;
    public IngredientSpawner CenterIngredient;
    public IngredientSpawner RightIngredient;

    public void SetIngs(List<Ingredient.IngredientTypes> ingredients)
    {
        LeftIngredient.MakeAndDisplayNewIngredient(ingredients[0]);
        CenterIngredient.MakeAndDisplayNewIngredient(ingredients[1]);
        RightIngredient.MakeAndDisplayNewIngredient(ingredients[2]);
    }

}
