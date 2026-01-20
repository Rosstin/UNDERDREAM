using System;
using System.Collections.Generic;

[Serializable]
public class ShiftData
{
    public List<OrderData> Orders;
}

[Serializable]
public class OrderData
{
    public string Description;
    public List<string> Recipe;

    public override string ToString()
    {
        string stringout = "Desc: " + Description + ". {";
        for(int i = 0; i < Recipe.Count; i++)
        {
            string ing = Recipe[i];

            stringout += " " + ing + ",";
        }

        stringout += "}";
        return stringout;
    }
}