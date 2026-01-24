using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [Header("Outlets")]
    public GameObject refCube;
    public BoxCollider myCollider;
    public GameObject fallingIngsParent;
    
    [Header("Prefabs")]
    public Ingredient ingredientPrefab;

    private Ingredient displayedIngredient = null;
    private List<Ingredient> releasedIngredients = new List<Ingredient>();
    
    void Start()
    {
        refCube.gameObject.SetActive(false);
    }

    void Update()
    {
        //if(myCollider)
    }
    
    
    public void MakeAndDisplayNewIngredient(Ingredient.IngredientTypes ingredient)
    {
        if (displayedIngredient != null)
        {
            Debug.LogError("Making new ingredient before releasing!?");
            return;
        }
        
        displayedIngredient = GameObject.Instantiate(ingredientPrefab).GetComponent<Ingredient>();

        displayedIngredient.Initialize(fallingIngsParent);
        
        
        displayedIngredient.transform.SetParent(this.transform);
        displayedIngredient.SetType(ingredient);
        displayedIngredient.transform.localPosition = new Vector3(0,0,0);
        displayedIngredient.transform.localRotation = Quaternion.identity;

        displayedIngredient.SetState(Ingredient.IngredientState.Tray);
    }

    public Ingredient ReleaseDisplayedIngredient()
    {
        var toRelease = displayedIngredient;
        
        toRelease.SetState(Ingredient.IngredientState.Held);
        releasedIngredients.Add(toRelease);

        displayedIngredient = null;
        
        return toRelease;
    }
}
