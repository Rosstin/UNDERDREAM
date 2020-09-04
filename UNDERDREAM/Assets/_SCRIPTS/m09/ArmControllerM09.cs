using System.Collections;
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

    private float clenchedElapsed = 0f;
    private float jitterElapsed = 0f;
    private bool clenched = false;

    private bool startedClenched = false;

    // Update is called once per frame
    void Update()
    {
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
        camJitter.enabled = true;
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
        if (!startedClenched)
        {
            StartPostClench();
        }

        //LoadNextScene();
    }

    void UpdatePreClench()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x,
                    arm.transform.localPosition.y + Time.deltaTime * armSpeed,
                    arm.transform.localPosition.z
                    );
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x,
                    arm.transform.localPosition.y - Time.deltaTime * armSpeed,
                    arm.transform.localPosition.z
                    );
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x - Time.deltaTime * armSpeed,
                    arm.transform.localPosition.y,
                    arm.transform.localPosition.z
                    );
        }

        if (Input.GetKey(KeyCode.RightArrow))
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
