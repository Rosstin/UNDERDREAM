using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ArmControllerM10 : BaseController
{
    [Header("Small Margin Of Closeness To Cut Scene")]
    [SerializeField] [Range(0f, 0.1f)] private float sceneBreakMargin;

    [Header("Camera")]
    [SerializeField] private Camera camera;
    [SerializeField] private AnimationCurve cameraCurve;
    [SerializeField] private Vector2 minMaxCameraZ;

    [Header("Arm")]
    [SerializeField] [Range(0f, 1f)] private float armSpeed;
    [SerializeField] private AnimationCurve armSpeedCurve;

    [Header("Arm Outlets")]
    [SerializeField] private GameObject arm;

    [Header("Jitter")]
    [SerializeField] private float jitterPeriod;
    [SerializeField] private Vector3 jitter;

    [Header("Scene Time")]
    [SerializeField] [Range(1f, 20f)] private float sceneTime;

    [Header("Min Pos")]
    [SerializeField] private Vector2 minPosition;

    [Header("SFX")]
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource whoopsSound;
    [SerializeField] private AudioSource banjoSound;
    [SerializeField] private AudioSource stringBreak;
    [SerializeField] private Vector2 minMaxPitch;
    [SerializeField] private AnimationCurve curve;

    private float jitterElapsed = 0f;
    private float postClenchElapsed;

    private bool nextScene = false;
    
    void Update()
    {
        BaseUpdate();

        if (nextScene)
        {
            return;
        }

        var armProgress = arm.transform.position.y / minPosition.y;

        banjoSound.pitch = minMaxPitch.x + minMaxPitch.y * curve.Evaluate(armProgress);

        var modifiedArmSpeed = armSpeedCurve.Evaluate(armProgress) * armSpeed;

        // zoom in as you get closer
        camera.gameObject.transform.position =
            new Vector3(camera.gameObject.transform.position.x,
                camera.gameObject.transform.position.y,
                minMaxCameraZ.x - ( minMaxCameraZ.y * cameraCurve.Evaluate(armProgress))
                );

        jitterElapsed += Time.deltaTime;

        if (jitterElapsed > jitterPeriod)
        {
            jitterElapsed = 0f;
            this.gameObject.transform.localPosition += new Vector3(
                    Random.Range(-jitter.x, jitter.x),
                    Random.Range(-jitter.y, jitter.y),
                    Random.Range(-jitter.z, jitter.z));
        }

        if (CommandsHeldThisFrame.ContainsKey(Command.Up) || CommandsHeldThisFrame.ContainsKey(Command.Right))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x + Time.deltaTime * modifiedArmSpeed,
                    arm.transform.localPosition.y + Time.deltaTime * modifiedArmSpeed,
                    arm.transform.localPosition.z
                    );
        }

        if (CommandsHeldThisFrame.ContainsKey(Command.Down) || CommandsHeldThisFrame.ContainsKey(Command.Left))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x - Time.deltaTime * modifiedArmSpeed,
                    arm.transform.localPosition.y - Time.deltaTime * modifiedArmSpeed,
                    arm.transform.localPosition.z
                    );
        }

        // dont let arm extend farther than we want
        if (arm.transform.position.x < minPosition.x)
        {
            arm.transform.position = new Vector3(minPosition.x, arm.transform.position.y, arm.transform.position.z);
        }

        if (arm.transform.position.y < minPosition.y + sceneBreakMargin)
        {
            arm.transform.position = new Vector3(arm.transform.position.x, minPosition.y, arm.transform.position.z);
            stringBreak.Play();
            LoadNextScene();
            nextScene = true;
        }
    }
}
