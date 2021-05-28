using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ArmControllerM90 : MonoBehaviour
{
    [Header("Lemon")]
    [SerializeField]
    private StairLemon lemon;

    [Header("Arm")]
    [SerializeField] [Range(0f, 1f)] private float armSpeed;
    [SerializeField] private AnimationCurve armSpeedCurve;

    [Header("Arm Outlets")]
    [SerializeField] private GameObject arm;

    [Header("Set Starting Offset")]
    [SerializeField] private Vector3 offset;


    void Update()
    {
        var modifiedArmSpeed = armSpeed;

        


       // var armProgress = arm.transform.position.y / minPosition.y;
    

        //var modifiedArmSpeed = armSpeedCurve.Evaluate(armProgress) * armSpeed;

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            offset =
                new Vector3(
                    offset.x + Time.deltaTime * modifiedArmSpeed,
                    offset.y - Time.deltaTime * modifiedArmSpeed,
                    offset.z
                    );
        }

        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            offset =
                new Vector3(
                    offset.x - Time.deltaTime * modifiedArmSpeed,
                    offset.y + Time.deltaTime * modifiedArmSpeed,
                    offset.z
                    );
        }

        arm.transform.position = lemon.transform.position + offset;

        // dont let arm extend farther than we want
        /*
        if (arm.transform.position.x < minPosition.x)
        {
            arm.transform.position = new Vector3(minPosition.x, arm.transform.position.y, arm.transform.position.z);
        }*/

    }
}
