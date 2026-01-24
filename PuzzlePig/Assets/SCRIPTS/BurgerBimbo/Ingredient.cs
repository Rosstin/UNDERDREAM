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
    
    private GameObject fallingIngsParent;
    
    private IngredientTypes MyIngredientType;

    private IngredientState myState;
    
    public enum IngredientState
    {
        Unset,
        Tray,
        Held,
        Falling,
        Burger
    }
    
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

    private void EnableAllColliders(bool enabled)
    {
        BunTop.GetComponent<Collider>().enabled = enabled;
        Tomato.GetComponent<Collider>().enabled = enabled;
        Onion.GetComponent<Collider>().enabled = enabled;
        Lettuce.GetComponent<Collider>().enabled = enabled;
        Cheese.GetComponent<Collider>().enabled = enabled;
        Meat.GetComponent<Collider>().enabled = enabled;
        BunBottom.GetComponent<Collider>().enabled = enabled;
    }
    
    public void SetState(IngredientState newState)
    {
        var oldState = myState;
        
        switch (newState)
        {
            case IngredientState.Unset:
                myState = IngredientState.Unset;
                break;
            case IngredientState.Tray:
                this.EnableAllColliders(false);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;
                myState = IngredientState.Tray;
                break;
            case IngredientState.Held:
                this.transform.SetParent(fallingIngsParent.transform);
                this.transform.localRotation = Quaternion.identity;
                this.EnableAllColliders(false);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;
                myState = IngredientState.Held;
                break;
            case IngredientState.Falling:
                this.EnableAllColliders(true);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = false;
                myState = IngredientState.Falling;
                break;
            case IngredientState.Burger:
                myState = IngredientState.Burger;
                break;
        }
        
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

    public void Initialize(GameObject fallingParent)
    {
        this.fallingIngsParent = fallingParent;
    }
}
