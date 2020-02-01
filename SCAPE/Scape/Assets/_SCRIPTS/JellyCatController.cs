using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCatController : MonoBehaviour
{
    [Header("Outlets")]
    public Camera ArCamera;

    [Header("Outlets - Items")]
    public CucumberController Cucumber;

    [Header("Configurables")]
    public float StartSpeed;
    public float RotationSpeed;
    public float AverageDirectionChangePeriod;
    public float VariationInPeriod;
    public float CucumberClosenessThreshholdPixels;
    public float BackupAmountMeters;

    private Vector3 FORWARD = new Vector3(0, 1, 0);

    private Vector3 curDirection;
    private float curSpeed;
    private int leftOrRight = 1;
    private float curDirectionChangePeriod;
    private float curCucumberTime = 0f;
    private float curTime = 0f;

    void Start()
    {
        NewDirectionChangePeriod();
    }

    private void NewDirectionChangePeriod()
    {
        curDirectionChangePeriod = AverageDirectionChangePeriod + Random.Range(-VariationInPeriod, +VariationInPeriod);
    }

    private void BackupAndFlip()
    {
        BackupCat();
        FlipCat();
    }

    // flip around and switch direction
    private void FlipCat()
    {
        this.transform.Rotate(0.0f, 180f, 0.0f, Space.Self);
    }

    // move back a bit so you're not colliding anymore
    private void BackupCat()
    {
        this.transform.position += -this.transform.forward * BackupAmountMeters;
    }

    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > curDirectionChangePeriod)
        {
            leftOrRight *= -1;
            curTime = 0f;
            NewDirectionChangePeriod();
        }

        this.transform.Rotate(0.0f, leftOrRight * RotationSpeed * Time.deltaTime, 0.0f, Space.Self);

        this.transform.position += this.transform.forward * StartSpeed * Time.deltaTime;

        // bottom left is 0,0, bottom right is 0,1, top left is 1,0, top right is 1,1
        Vector2 screenPointOfCat = ArCamera.WorldToScreenPoint(this.transform.position);
        if (screenPointOfCat.x < 0f)
        {
            BackupAndFlip();
        }
        else if (screenPointOfCat.x > ArCamera.pixelWidth)
        {
            BackupAndFlip();
        }

        if (screenPointOfCat.y < 0f)
        {
            BackupAndFlip();
        }
        else if (screenPointOfCat.y > ArCamera.pixelHeight)
        {
            BackupAndFlip();
        }

        if (Cucumber.gameObject.activeSelf)
        {
            Vector2 screenPointOfCucumber = ArCamera.WorldToScreenPoint(Cucumber.transform.position);
            if (Mathf.Abs(Vector2.Distance(screenPointOfCat, screenPointOfCucumber)) < CucumberClosenessThreshholdPixels)
            {
                BackupAndFlip();
            }
        }

    }
}
