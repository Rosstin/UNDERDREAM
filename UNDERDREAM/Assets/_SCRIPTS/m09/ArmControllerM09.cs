﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmControllerM09 : BaseController
{
    [Header("Face Outlets")]
    [SerializeField] private GameObject face;
    [SerializeField] private GameObject surprisedFace;

    [Header("Lemon")]
    [SerializeField] private FlyingLemon lemon;
    [SerializeField] private Rigidbody2D lemonRb;

    [Header("Arm")]
    [SerializeField] [Range(0f, 1f)] private float armSpeed;
    [SerializeField] private float maxHeight;

    [Header("Arm Outlets")]
    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject clenchedArm;

    [Header("Jitter")]
    [SerializeField] private float jitterPeriod;
    [SerializeField] private Vector3 jitter;
    [SerializeField] [Range(1, 10)] private int jitterIntensityIncreaseAfterClench;

    [Header("SFX")]
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource whoopsSound;

    [Header("Clench")]
    [SerializeField] [Range(1f, 15f)] private float clenchDuration;

    [Header("Camera Jitter")]
    [SerializeField] private Jitter camJitter;

    [Header("Post Clench Next Scene Time")]
    [SerializeField] [Range(1f,9f)] private float nextSceneTime;

    private float jitterElapsed = 0f;
    private bool clenched = false;

    private bool startedClenched = false;

    private float postClenchElapsed;

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        jitterElapsed += Time.deltaTime;

        if (jitterElapsed > jitterPeriod)
        {
            jitterElapsed = 0f;
            this.gameObject.transform.localPosition += new Vector3(
                    Random.Range(-jitter.x, jitter.x),
                    Random.Range(-jitter.y, jitter.y),
                    Random.Range(-jitter.z, jitter.z));
        }

        if (!clenched)
        {
            UpdatePreClench();
        }
        else
        {
            UpdatePostClench();
        }

    }


    private void StartPostClench()
    {
        camJitter.SetJitter(true);
        whoopsSound.Play();
        startedClenched = true;
        face.gameObject.SetActive(false);
        surprisedFace.gameObject.SetActive(true);
        lemon.gameObject.transform.parent = this.transform;
        lemon.enabled = true;
        lemonRb.gravityScale = 1;
    }

    void UpdatePostClench()
    {
        postClenchElapsed += Time.deltaTime;

        if (!startedClenched)
        {
            StartPostClench();
        }

        if (postClenchElapsed > nextSceneTime)
        {
            LoadNextScene();
        }
    }

    void UpdatePreClench()
    {

        if (CommandsHeldThisFrame.ContainsKey(Command.Up))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x,
                    arm.transform.localPosition.y + Time.deltaTime * armSpeed,
                    arm.transform.localPosition.z
                    );
        }

        if (CommandsHeldThisFrame.ContainsKey(Command.Down))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x,
                    arm.transform.localPosition.y - Time.deltaTime * armSpeed,
                    arm.transform.localPosition.z
                    );
        }

        if (CommandsHeldThisFrame.ContainsKey(Command.Left))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x - Time.deltaTime * armSpeed,
                    arm.transform.localPosition.y,
                    arm.transform.localPosition.z
                    );
        }

        if (CommandsHeldThisFrame.ContainsKey(Command.Right))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x + Time.deltaTime * armSpeed,
                    arm.transform.localPosition.y,
                    arm.transform.localPosition.z
                    );
        }

        if (arm.transform.position.y > maxHeight)
        {
            clickSound.Play();

            arm.SetActive(false);
            clenchedArm.SetActive(true);

            clenchedArm.transform.position = new Vector3(
                arm.transform.position.x,
                maxHeight,
                arm.transform.position.z
                );

            clenched = true;

            jitter *= jitterIntensityIncreaseAfterClench;
            jitterPeriod /= jitterIntensityIncreaseAfterClench;
        }
    }



}
