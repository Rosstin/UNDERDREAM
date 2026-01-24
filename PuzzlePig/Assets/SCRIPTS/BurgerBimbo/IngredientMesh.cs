using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IngredientMesh : MonoBehaviour
{
    public Ingredient Parent;

    void OnCollisionEnter(Collision collision)
    {
        Parent.CollidedWith(collision);
    }

}
