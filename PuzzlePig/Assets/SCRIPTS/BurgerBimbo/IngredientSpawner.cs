using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [Header("Outlets")]
    public Ingredient myIngredient;
    public GameObject refCube;
    public BoxCollider myCollider;
    
    void Start()
    {
        refCube.gameObject.SetActive(false);
    }

    void Update()
    {
        //if(myCollider)
    }
    
    public void MakeIngredient(Ingredient.IngredientTypes ingredient)
    {
        myIngredient.SetType(ingredient);
        myIngredient.transform.localPosition = new Vector3(0,0,0);
    }
}
