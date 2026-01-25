using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrderPreviewWindow : MonoBehaviour
{
    [Header("Prefabs")]
    public Ingredient ingredientPrefab;
    
    [Header("Outlets")]
    public GameObject ingredientParent;
    
    private List<Ingredient> displayedIngredients = new List<Ingredient>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear()
    {
        foreach (var ing in displayedIngredients)
        {
            ing.DestroySelf();
        }
        displayedIngredients.Clear();
    }

    public void AddIngredient(Ingredient.IngredientTypes type)
    {
        Ingredient ing = GameObject.Instantiate(ingredientPrefab).GetComponent<Ingredient>();
        
        ing.SetType(type);
        
        ing.transform.parent = ingredientParent.transform;
        
        ing.transform.localPosition = new Vector3(0, 0 + displayedIngredients.Count * 0.3f, 0);
        
        displayedIngredients.Add(ing);
        
    }
}
