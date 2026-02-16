using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaristaGS : BaseController
{
    [Header("Outlets")]
    public Camera mainCamera;
    public Collider CollisionPlane;

    public BobaReticle heldRet = null;

    public Vector3 startPos;
    
    private void Awake()
    {
        
    }

    private void Update()
    {
        BaseUpdate();
        
        


        if (CommandsStartedThisFrame.ContainsKey(Command.Fire))
        {

            
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
            
                BobaReticle bobaRet = objectHit.gameObject.GetComponent<BobaReticle>();

                if (bobaRet != null)
                {
                    heldRet = bobaRet;
                }
            }
                    
        }
        
        if (heldRet != null)
        {
            // dropping ing
            if (!CommandsHeldThisFrame.ContainsKey(Command.Fire))
            {

                heldRet.transform.position = startPos;
                heldRet = null;
            }
            //holding ing
            else
            {

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                RaycastHit hit;
                
                if(CollisionPlane.Raycast(ray, out hit, 100f))
                {

                    heldRet.transform.position = hit.point;
                }
            }
                    
        }

        
    }
}
