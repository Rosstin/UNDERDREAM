using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

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

    private List<IngredientMesh> listOfIngredientMeshes = null;
    
    private AudioSource popSfx;
    private GameObject fallingIngsParent;
    private BurgGS gamestate;
    private BurgerParent burgerParent;
    
    private IngredientTypes MyIngredientType;

    private IngredientState myState;

    private Vector3 flingForce = Vector3.negativeInfinity;
    
    public enum IngredientState
    {
        Unset,
        Tray,
        Held,
        Falling,
        Burger,
        Scored,
        Missed,
        Fling,
        Displayed,
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

    private void Awake()
    {
        listOfIngredientMeshes =  new List<IngredientMesh>(){BunTop, Tomato, Onion, Lettuce, Cheese, Meat, BunBottom};
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

    private void SetRigidbodyZFrozen(bool frozen)
    {
        foreach (var im in listOfIngredientMeshes)
        {
            var rb = im.gameObject.GetComponent<Rigidbody>();
            if (frozen)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionZ;
            }
            else
            {
                rb.constraints = RigidbodyConstraints.None;
            }

        }
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
                SetRigidbodyZFrozen(true);
                myState = IngredientState.Tray;
                this.gameObject.SetActive(true);
                this.EnableAllColliders(false);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;
                break;
            case IngredientState.Held:
                gamestate.SetActiveIngredient(this);
                SetRigidbodyZFrozen(true);
                myState = IngredientState.Held;
                this.gameObject.SetActive(true);
                this.transform.SetParent(fallingIngsParent.transform);
                this.transform.localRotation = Quaternion.identity;
                this.EnableAllColliders(false);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;
                break;
            case IngredientState.Falling:
                
                // todo - start next ingredient early!
                
                SetRigidbodyZFrozen(true);
                myState = IngredientState.Falling;
                this.transform.SetParent(fallingIngsParent.transform);
                this.gameObject.SetActive(true);
                this.EnableAllColliders(true);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = false;
                break;
            case IngredientState.Burger:
                SetRigidbodyZFrozen(true);
                myState = IngredientState.Burger;
                
                gamestate.UnsetActiveIngredient();

                this.gameObject.SetActive(true);
                popSfx.Play();

                this.burgerParent.AddIngredient(this);
                
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;

                this.gamestate.StartNextIngredient();
                
                break;
            case IngredientState.Missed:
                
                this.burgerParent.MissIngredient(this);

                gamestate.UnsetActiveIngredient();
                SetRigidbodyZFrozen(true);
                myState = IngredientState.Missed;
                this.gamestate.StartNextIngredient();
                this.gameObject.SetActive(false);
                this.transform.parent = gamestate.missedParent.transform;
                break;
            case IngredientState.Fling:
                myState = IngredientState.Fling;
                SetRigidbodyZFrozen(false);
                var rb = GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>();
                rb.isKinematic = false;
                
                rb.AddForce(flingForce, ForceMode.Force);
                break;
            case IngredientState.Scored:
                myState = IngredientState.Scored;
                SetRigidbodyZFrozen(true);

                StartCoroutine(HideAndDestroy());
                
                break;
            case IngredientState.Displayed:
                myState = IngredientState.Displayed;
                SetRigidbodyZFrozen(true);
                GetGOForIngredient(MyIngredientType).GetComponent<Rigidbody>().isKinematic = true;
                EnableAllColliders(false);
                break;
        }
        
    }

    private IEnumerator HideAndDestroy()
    {
        yield return new WaitForSeconds(4f);
        DestroySelf();
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



    public void Initialize(GameObject fallingParent, BurgerParent burgerParent, AudioSource splatSFX, BurgGS burggs, MissedParent missedParent)
    {
        this.gamestate = burggs;
        this.popSfx = splatSFX;
        this.fallingIngsParent = fallingParent;
        this.burgerParent = burgerParent;
    }
    
    public void CollidedWith(Collision collision)
    {
        var servingPlate = collision.gameObject.GetComponent<ServingPlate>();
        var ingredientMesh = collision.gameObject.GetComponent<IngredientMesh>();

        if (ingredientMesh != null || servingPlate != null)
        {
            if(myState == IngredientState.Falling)
            {
                SetState(IngredientState.Burger);
            }
            else
            {
                // ignore
            }
            
        }
        
    }

    public void Score()
    {
        SetState(IngredientState.Scored);
    }

    public void Fling(Vector3 force)
    {
        this.flingForce = force;
        SetState(IngredientState.Fling);
    }

    public void DestroySelf()
    {
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        
        Destroy(gameObject);
    }

    public bool IsBelowScreen(GameObject fallzonePos)
    {
        if (GetGOForIngredient(MyIngredientType).transform.position.y < fallzonePos.transform.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
