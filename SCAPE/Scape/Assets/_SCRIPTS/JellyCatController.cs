using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCatController : MonoBehaviour
{
    [Header("Outlets")]
    public Camera ArCamera;

    [Header("Configurables")]
    public float StartSpeed;
    public float RotationSpeed;
    public float AverageDirectionChangePeriod = 5f;
    public float VariationInPeriod = 2f;

    private Vector3 forward = new Vector3(0, 1, 0);
    private Vector3 curDirection;
    private float curSpeed;
    private int leftOrRight = 1;
    private float curDirectionChangePeriod;

    private float curTime = 0f;

    void Start()
    {
        NewDirectionChangePeriod();
    }

    private void NewDirectionChangePeriod()
    {
        curDirectionChangePeriod = AverageDirectionChangePeriod + Random.Range(-VariationInPeriod, +VariationInPeriod);
    }

    private void FlipCat()
    {
        this.transform.Rotate(0.0f, 180f, 0.0f, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;

        if(curTime > curDirectionChangePeriod)
        {
            leftOrRight *= -1;
            curTime = 0f;
            NewDirectionChangePeriod();
        }

        this.transform.Rotate(0.0f, leftOrRight* RotationSpeed * Time.deltaTime, 0.0f, Space.Self);

        this.transform.position += this.transform.forward * StartSpeed * Time.deltaTime;

        // bottom left is 0,0, bottom right is 0,1, top left is 1,0, top right is 1,1
        Vector3 screenPointOfCat = ArCamera.WorldToScreenPoint(this.transform.position);
        if (screenPointOfCat.x < 0f)
        {
            Debug.Log("cat off screen to left");
            FlipCat();
        }
        else if (screenPointOfCat.x > ArCamera.pixelWidth)
        {
            Debug.Log("cat off screen to right");
            FlipCat();
        }

        if (screenPointOfCat.y < 0f)
        {
            Debug.Log("cat off screen to bot");
            FlipCat();
        }
        else if (screenPointOfCat.y > ArCamera.pixelHeight)
        {
            Debug.Log("cat off screen to top");
            FlipCat();
        }


        // determine where the cat is in screen space
        // find the distance of the cat to the top, the bottom, the left, and the right, do things with it

    }
}
