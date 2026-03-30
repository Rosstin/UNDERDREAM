using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BobaBall : MonoBehaviour
{
    public enum BobaColor
    {
        Unset,
        Emerald,
        Ruby,
        Sapphire,
    }

    [Header("Boba Materials")]
    public Material EmeraldMaterial;
    public Material RubyMaterial;
    public Material SapphireMaterial;

    [Header("My Renderer")] 
    public MeshRenderer Renderer;
    
    
    public void SetBobaColor(BobaColor color)
    {
        switch (color)
        {
            case BobaColor.Emerald:
                Renderer.material = EmeraldMaterial;
                break;
            case BobaColor.Ruby:
                Renderer.material = RubyMaterial;
                break;
            case BobaColor.Sapphire:
                Renderer.material = SapphireMaterial;
                break;
            case BobaColor.Unset:
                Debug.LogError("bobacolor is unset!");
                break;
        }
        
    }






}
