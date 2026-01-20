using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [Header("Ingredient Outlets")] 
    public GameObject BunTop;
    public GameObject Tomato;
    public GameObject Onion;
    public GameObject Lettuce;
    public GameObject Cheese;
    public GameObject Meat;
    public GameObject BunBottom;
    
    [Header("Parameters")]
    private IngredientTypes MyIngredientType;

    public enum IngredientTypes
    {
        Unset,
        BunTop,
        Tomato,
        Onion,
        Lettuce,
        Cheese,
        Meat,
        BunBottom,
    }

    public IngredientTypes GetIngredientType()
    {
        return MyIngredientType;
    }
    
    private void HideAll()
    {
        BunTop.SetActive(false);
        Tomato.SetActive(false);
        Onion.SetActive(false);
        Lettuce.SetActive(false);
        Cheese.SetActive(false);
        Meat.SetActive(false);
        BunBottom.SetActive(false);
    }

    public void SetFrozen(bool freeze)
    {
        GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = freeze;
    }

    private GameObject GetGOForIngredient(IngredientTypes type)
    {
        switch (type)
        {
            case IngredientTypes.BunTop:
                return BunTop;
                break;
            case IngredientTypes.Tomato:
                return Tomato;
                break;
            case IngredientTypes.Onion:
                return Onion;
                break;
            case IngredientTypes.Lettuce:
                return Lettuce;
                break;
            case IngredientTypes.Cheese:
                return Cheese;
                break;
            case IngredientTypes.Meat:
                return Meat;
                break;
            case IngredientTypes.BunBottom:
                return BunBottom;
                break;
            case IngredientTypes.Unset:
                Debug.LogError("Unset Ingredient");
                return null;
                break;
            default:
                Debug.LogError("default in ingredient.cs");
                return null;
                break;
        }
    }
    
    public void SetType(IngredientTypes type)
    {
        HideAll();
        this.MyIngredientType = type;
        
        GetGOForIngredient(type).SetActive(true);
        
    }

    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
