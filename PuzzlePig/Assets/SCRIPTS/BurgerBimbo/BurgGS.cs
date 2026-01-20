using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BurgGS : BaseController
{
    [Header("Outlets")]
    public IngredientTray IngredientTray;

    public Camera mainCamera;
    
    private ShiftData currentShift;

    private int currentOrderIndex = 0;
    private int currentIngredientIndex = 0;

    private static System.Random rng = new System.Random();

    void Start()
    {
        base.Start();
        
        currentShift = GetSampleShift();
        currentOrderIndex = -1;
        StartNextOrder();
    }

    void Update()
    {
        BaseUpdate();

        if (CommandsStartedThisFrame.ContainsKey(Command.Fire))
        {
            var mouseX = Input.mousePosition.x / Screen.width;
            var mouseY = Input.mousePosition.y / Screen.height;

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
            
                Debug.Log(objectHit.name);
            }
        }
        

    }

    private void StartNextOrder()
    {
        currentOrderIndex++;
        currentIngredientIndex = -1;
        StartNextIngredient();
    }

    private void StartNextIngredient()
    {
        currentIngredientIndex++;
        var correctIng = currentShift.Orders[currentOrderIndex].Recipe[currentIngredientIndex];

        ShowIngredientOptions(GetIngredientTypeForString(correctIng));
        
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

    private ShiftData GetSampleShift()
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

        ShiftData shift1 = new ShiftData();

        var orders = new List<OrderData>();
        orders.Add(order1);
        orders.Add(order2);

        shift1.Orders = orders;
        
        return shift1;

    }
    
    
    
}
