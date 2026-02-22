using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaristaGS : BaseController
{
    [Header("Outlets")]
    public Camera mainCamera;
    public Collider CollisionPlane;
    public GameObject DummyBall;
    public BobaReticle heldRet = null;

    [Header("Configs")]
    public Vector3 startPos;
    public float fireDist;
    
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
                // calculate the direction between the current position and the startpos
                Vector3 shootDirection = startPos - heldRet.transform.position;
                
                shootDirection.Normalize();

                DummyBall.transform.position = startPos + shootDirection * fireDist;
                
                
                
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
