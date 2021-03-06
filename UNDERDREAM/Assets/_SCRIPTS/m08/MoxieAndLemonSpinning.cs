﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoxieAndLemonSpinning : MonoBehaviour
{
    [Header("Outlets")]
    public Rigidbody2D Body;
    public BoxCollider2D MyCollider;
    public BoxCollider2D Ground;
    public GameObject Cloud;
    public AudioSource ThudSound;
    public AudioSource ExplosionSound;
    public GameObject SkiddingMoxie;
    public GameObject Lemon;
    public Maoxun08 Controller;
    public GameObject[] Contained;
    [SerializeField] private BaseController baseController;

    [Header("Starting Motion")]
    public float StartingTorque;
    public Vector2 StartingForce;

    [Header("Lemon Offset")]
    public Vector3 LemonOffset;

    [Header("Controllable After")]
    [SerializeField] [Range(1,9)] private float ControllableAfterSeconds;

    private bool explosionTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        Controller.SetVisible(false);
        Body.AddForce(StartingForce);
        Body.AddTorque(StartingTorque);
    }

    // Update is called once per frame
    void Update()
    {
        if (Ground.IsTouching(MyCollider) && !explosionTriggered)
        {
            explosionTriggered = true;
            StartCoroutine(ActivateControls());

            // generate the explosion and deactivate yourself
            foreach(GameObject g in Contained)
            {
                g.SetActive(false);
            }
            Cloud.transform.position = this.gameObject.transform.position;
            Cloud.SetActive(true);
            ThudSound.Play();
            ExplosionSound.Play();
            SkiddingMoxie.gameObject.transform.position = this.transform.position;
            SkiddingMoxie.gameObject.SetActive(true);

            Lemon.gameObject.transform.position = this.transform.position + LemonOffset;
            Lemon.gameObject.SetActive(true);
        }
    }

    private IEnumerator ActivateControls()
    {
        yield return new WaitForSeconds(ControllableAfterSeconds);

        bool activated = false;
        while (!activated)
        {
            foreach(var key in baseController.CommandsHeldThisFrame.Keys)
            {
                Debug.LogWarning("key down " + key);
            }

            if (baseController.CommandsHeldThisFrame.ContainsKey(BaseController.Command.Up) || baseController.CommandsHeldThisFrame.ContainsKey(BaseController.Command.Fire))
            {
                SkiddingMoxie.gameObject.SetActive(false);
                Controller.SetVisible(true);
                activated = true;
            }
            yield return null;
        }
    }
}
