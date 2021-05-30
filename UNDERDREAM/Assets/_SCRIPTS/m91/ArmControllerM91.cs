using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArmControllerM91 : BaseController
{
    [Header("End")]
    [SerializeField]
    private float endTime;

    [Header("SFX")]
    [SerializeField] private AudioSource gulp;

    [Header("Face Outlets")]
    [SerializeField] private GameObject face;
    [SerializeField] private GameObject puckerAnim;

    [Header("Lemon")]
    [SerializeField] private FlyingLemon lemon;

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

    [Header("Camera Jitter")]
    [SerializeField] private Jitter camJitter;

    private float jitterElapsed = 0f;
    private bool clenched = false;

    private bool startedClenched = false;

    private float postClenchElapsed;

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        jitterElapsed += Time.deltaTime;

        if (jitterElapsed > jitterPeriod && clenched==false)
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
        startedClenched = true;
        face.gameObject.SetActive(false);
        puckerAnim.gameObject.SetActive(true);
        lemon.gameObject.SetActive(false);
    }

    void UpdatePostClench()
    {
        postClenchElapsed += Time.deltaTime;

        if (!startedClenched)
        {
            StartPostClench();
        }

        if (postClenchElapsed > endTime)
        {
            LoadNextScene();
        }
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
            gulp.Play();

            arm.SetActive(false);
            clenchedArm.SetActive(true);

            clenchedArm.transform.position = new Vector3(
                arm.transform.position.x,
                maxHeight,
                arm.transform.position.z
                );

            clenched = true;


            jitterPeriod = 500000f;
                jitter /= 90000f;
            //jitter *= jitterIntensityIncreaseAfterClench;
            //jitterPeriod /= jitterIntensityIncreaseAfterClench;
        }
    }



}
