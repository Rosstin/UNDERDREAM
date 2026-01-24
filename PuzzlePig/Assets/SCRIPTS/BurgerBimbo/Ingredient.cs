using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [Header("Ingredient Outlets")] 
    public IngredientMesh BunTop;
    public IngredientMesh Tomato;
    public IngredientMesh Onion;
    public IngredientMesh Lettuce;
    public IngredientMesh Cheese;
    public IngredientMesh Meat;
    public IngredientMesh BunBottom;

    private AudioSource splatSFX;
    private ServingPlate myServingPlate;
    private GameObject fallingIngsParent;
    private BurgGS gamestate;
    private BurgerParent burgerParent;
    private MissedParent missedParent;
    private ScoredParent scoredParent;
    
    private IngredientTypes MyIngredientType;

    private IngredientState myState;
    
    public enum IngredientState
    {
        Unset,
        Tray,
        Held,
        Falling,
        Burger,
        Scored,
        Missed
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
        BunTop.gameObject.SetActive(false);
        Tomato.gameObject.SetActive(false);
        Onion.gameObject.SetActive(false);
        Lettuce.gameObject.SetActive(false);
        Cheese.gameObject.SetActive(false);
        Meat.gameObject.SetActive(false);
        BunBottom.gameObject.SetActive(false);
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
                this.gameObject.SetActive(true);
                this.EnableAllColliders(false);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;
                myState = IngredientState.Tray;
                break;
            case IngredientState.Held:
                this.gameObject.SetActive(true);
                this.transform.SetParent(fallingIngsParent.transform);
                this.transform.localRotation = Quaternion.identity;
                this.EnableAllColliders(false);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;
                myState = IngredientState.Held;
                break;
            case IngredientState.Falling:
                this.gameObject.SetActive(true);
                this.EnableAllColliders(true);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = false;
                myState = IngredientState.Falling;
                break;
            case IngredientState.Burger:
                // make a splat sound
                this.gameObject.SetActive(true);
                splatSFX.Play();

                this.burgerParent.AddIngredient(this);
                
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;
                myState = IngredientState.Burger;

                this.gamestate.StartNextIngredient();
                
                break;
            case IngredientState.Missed:
                this.gamestate.StartNextIngredient();
                this.gameObject.SetActive(false);
                this.transform.parent = missedParent.transform;
                break;
            case IngredientState.Scored:
                this.transform.parent = scoredParent.transform;
                this.gameObject.SetActive(false);
                break;
        }
        
    }
    
    
    private GameObject GetGOForIngredient(IngredientTypes type)
    {
        switch (type)
        {
            case IngredientTypes.BunTop:
                return BunTop.gameObject;
                break;
            case IngredientTypes.Tomato:
                return Tomato.gameObject;
                break;
            case IngredientTypes.Onion:
                return Onion.gameObject;
                break;
            case IngredientTypes.Lettuce:
                return Lettuce.gameObject;
                break;
            case IngredientTypes.Cheese:
                return Cheese.gameObject;
                break;
            case IngredientTypes.Meat:
                return Meat.gameObject;
                break;
            case IngredientTypes.BunBottom:
                return BunBottom.gameObject;
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


    // Update is called once per frame
    void Update()
    {
        switch (myState)
        {
            case IngredientState.Unset:
                break;
            case IngredientState.Tray:
                break;
            case IngredientState.Held:
                break;
            case IngredientState.Falling:
                
                //if(myServingPlate.GetComponent<Collider>().touc)
                /*
                if(myServingPlate.GetComponent<ServingPlate>().isKinematic)
                
                if(GetGOForIngredient(MyIngredientType).GetComponent<Collider>()())
                */
                break;
            case IngredientState.Burger:
                break;
            case IngredientState.Scored:
                break;
        }
        
    }

    public void Initialize(GameObject fallingParent, BurgerParent burgerParent, ServingPlate servingPlate, AudioSource splatSFX, BurgGS burggs, MissedParent missedParent, ScoredParent scoredParent)
    {
        this.gamestate = burggs;
        this.splatSFX = splatSFX;
        this.fallingIngsParent = fallingParent;
        this.burgerParent = burgerParent;
        this.myServingPlate = servingPlate;
        this.missedParent = missedParent;
        this.scoredParent = scoredParent;
    }
    
    public void CollidedWith(Collision collision)
    {
        var servingPlate = collision.gameObject.GetComponent<ServingPlate>();
        var ingredientMesh = collision.gameObject.GetComponent<IngredientMesh>();

        if (ingredientMesh != null || servingPlate != null)
        {

            if (myState == IngredientState.Burger)
            {
                // ignore
            }
            else
            {
                SetState(IngredientState.Burger);
            }
            
        }
        
    }

    public void Score()
    {
        SetState(IngredientState.Scored);
    }
}
