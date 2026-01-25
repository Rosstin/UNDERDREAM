using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/*
public class TrackedScore
{
    private int missedIngredients = 0;
    private float
}
*/

public class BurgGS : BaseController
{
    [Header("Outlets")]
    public IngredientTray IngredientTray;
    public Collider CollisionPlane;
    public Camera mainCamera;
    public BurgerParent burgerParent;
    public OrderPreviewWindow orderPreviewWindow;
    public TextMeshProUGUI scoreText;
    
    private ShiftData currentShift;

    private float Z_INTERACTION_ZONE = 0f; // interactions should happen at z 0 
    
    private int currentOrderIndex = 0;
    private int currentIngredientIndex = 0;

    private static System.Random rng = new System.Random();

    private Ingredient heldIng = null;

    #region scoring
    private Dictionary<Ingredient.IngredientTypes,int> idealTypesToNumbers = new Dictionary<Ingredient.IngredientTypes,int>();
    #endregion
    
    
    
    void Start()
    {
        base.Start();

        Shift1();
        
    }

    void Shift1()
    {
        // todo music
        // todo - customers say the orders they want
        // todo - scoring system
        // todo opponent?
        
        currentShift = GetShift1();
        currentOrderIndex = -1;
        StartNextOrder();
    }

    void Update()
    {
        BaseUpdate();

        if (heldIng != null)
        {
            if (!CommandsHeldThisFrame.ContainsKey(Command.Fire))
            {
                heldIng.SetState(Ingredient.IngredientState.Falling);
                heldIng = null;
                //StartNextIngredient();
            }
            else
            {

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                RaycastHit hit;
                
                if(CollisionPlane.Raycast(ray, out hit, 100f))
                {
                    heldIng.transform.position = hit.point;
                }

                
                
                
                
            }
        }
        
        if (CommandsStartedThisFrame.ContainsKey(Command.Fire))
        {

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                
                IngredientSpawner ingSpawner = objectHit.gameObject.GetComponent<IngredientSpawner>();
                //Ingredient ing = objectHit.gameObject.GetComponent<Ingredient>();

                if (ingSpawner != null)
                {
                    heldIng = ingSpawner.ReleaseDisplayedIngredient();
                }
                
            }
        }
        

    }

    private void StartNextOrder()
    {
        
        
        
        
        currentOrderIndex++;

        if (currentOrderIndex >= currentShift.Orders.Count)
        {
            Debug.Log("rosstintodo FINISHED!");
        }
        else
        {
            // set current ingredient index to top 
            var curOrd = currentShift.Orders[currentOrderIndex];
            currentIngredientIndex = curOrd.Recipe.Count;

            // populate the preview window and fill scoring dictionary
            PopulatePreviewWindowAndFillScoringDictionary();

            // populate the tray at the top of the screen
            StartNextIngredient();
        }
        
    }

    private void PopulatePreviewWindowAndFillScoringDictionary()
    {
        orderPreviewWindow.Clear();
        var curOrd = currentShift.Orders[currentOrderIndex];
        idealTypesToNumbers.Clear();
        for(int i = curOrd.Recipe.Count-1; i >= 0; i--)
        {
            var type = GetIngredientTypeForString(curOrd.Recipe[i]);
            orderPreviewWindow.AddIngredient(type);

            if (!idealTypesToNumbers.TryAdd(type, 1))
            {
                idealTypesToNumbers[type] += 1;
            }
        }
        
    }

    public void StartNextIngredient()
    {
        currentIngredientIndex--;

        if (currentIngredientIndex < 0)
        {
            burgerParent.ScoreBurger(idealTypesToNumbers: idealTypesToNumbers, scoreText: scoreText);
            
            
            
        }
        else
        {
            var correctIng = currentShift.Orders[currentOrderIndex].Recipe[currentIngredientIndex];
            ShowIngredientOptions(GetIngredientTypeForString(correctIng));
        }
        
        
    }

    public void DoneScoring()
    {
        StartNextOrder();
    }

    private Ingredient.IngredientTypes GetIngredientTypeForString(string typeAsString)
    {
        Ingredient.IngredientTypes myEnum = (Ingredient.IngredientTypes) Enum.Parse(typeof(Ingredient.IngredientTypes), typeAsString, true);
        return myEnum;
    }
    
    private void ShowIngredientOptions(Ingredient.IngredientTypes correctIngredient)
    {
        // generate two distinct ingredients from the correct ingredient
        List<Ingredient.IngredientTypes> ingOps = new List<Ingredient.IngredientTypes>();
        
        List<Ingredient.IngredientTypes> remainingIngs = new List<Ingredient.IngredientTypes>();
        remainingIngs.Add(Ingredient.IngredientTypes.Meat);
        remainingIngs.Add(Ingredient.IngredientTypes.Cheese);
        remainingIngs.Add(Ingredient.IngredientTypes.Lettuce);
        remainingIngs.Add(Ingredient.IngredientTypes.Tomato);
        remainingIngs.Add(Ingredient.IngredientTypes.Onion);
        
        remainingIngs.Remove(correctIngredient);
        
        int choice2 = Random.Range(0, remainingIngs.Count);
        
        var ing2 = remainingIngs[choice2];
        remainingIngs.Remove(ing2);
        
        int choice3 = Random.Range(0, remainingIngs.Count);
        var ing3 = remainingIngs[choice3];
        
        ingOps.Add(correctIngredient);
        ingOps.Add(ing2);
        ingOps.Add(ing3);

        var shuffledList = ingOps.OrderBy( x => Random.value ).ToList( );
        
        IngredientTray.SetIngs(shuffledList);
    }

    private ShiftData GetShift1()
    {
        OrderData order1 = new OrderData();
        order1.Description = "Just a plain burger, please!";
        order1.Recipe = new List<string>
        {
            "BunTop",
            "Meat",
            "BunBottom",
        };

        OrderData order2 = new OrderData();
        order2.Description = "Gimme da works, toots.";
        order2.Recipe = new List<string>
        {
            "BunTop",
            "Onion",
            "Meat",
            "Lettuce",
            "Cheese",
            "Tomato",
            "BunBottom",
        };

        
        OrderData o3 = new OrderData();
        o3.Description = "I want the Big Bimbo!";
        o3.Recipe = new List<string>
        {
            "BunTop",
            "Cheese",
            "Meat",
            "Lettuce",
            "BunBottom",
            "Cheese",
            "Meat",
            "Lettuce",
            "Onion",
            "BunBottom",
        };

        
        ShiftData shift1 = new ShiftData();

        var orders = new List<OrderData>();
        orders.Add(order1);
        orders.Add(order2);
        orders.Add(o3);

        shift1.Orders = orders;
        
        return shift1;

    }
    
    
    
}
