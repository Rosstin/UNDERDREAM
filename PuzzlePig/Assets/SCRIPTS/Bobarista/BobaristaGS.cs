using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BobaristaGS : BaseController
{
    [Header("Outlets")]
    public Camera mainCamera;
    public Collider CollisionPlane;
    public Projectile DummyBall;
    public BobaReticle heldRet = null;
    
    [Header("Positional References For Projectile")]
    public GameObject LeftWall;
    public GameObject Center;
    public GameObject RightWall;
    
    
    [Header("Configs")]
    public Vector3 startPos;
    public float speed;
    
    private void Awake()
    {
        DummyBall.Init(LeftWall, Center, RightWall);
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

                //DummyBall.transform.position = startPos + shootDirection * fireDist;
                
                DummyBall.transform.position = startPos;
                
                DummyBall.Shoot(shootDirection, speed);
                
                
                
                
                // we want to actually fire the ball, have it bounce from walls, stop when it hits another ball
                
                
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
