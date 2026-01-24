using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [Header("Outlets")]
    public GameObject refCube;
    public AudioSource splatsfx;
    
    [Header("Refs for Ingredients")]
    public GameObject fallingIngsParent;
    public BurgerParent burgerParent;
    public ServingPlate servingPlate;
    public BurgGS burgGs;
    public MissedParent missedParent;
    public ScoredParent scoredParent;
    
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
            // chuck it. rosstintodo - funny animation? fling it away?
            
            // or -- reuse it?
        }
        else
        {
            displayedIngredient = GameObject.Instantiate(ingredientPrefab).GetComponent<Ingredient>();
        }
        
        displayedIngredient.Initialize(fallingParent: fallingIngsParent, burgerParent: burgerParent, servingPlate: servingPlate, splatSFX: splatsfx, burggs: burgGs, missedParent: missedParent, scoredParent:scoredParent);
        
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
