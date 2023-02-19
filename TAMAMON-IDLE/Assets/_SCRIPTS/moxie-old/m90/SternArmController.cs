using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SternArmController: MonoBehaviour
{
    [Header("Lemon")]
    [SerializeField] private StairLemon lemon;

    [Header("Arm")]
    [SerializeField] [Range(0f, 1f)] private float armSpeed;

    [Header("Goal")]
    [SerializeField]
    private Transform handSpot;
    [SerializeField] private GameMasterM90 gameMaster;

    [Header("Arm Outlets")]
    [SerializeField] private GameObject arm;

    [Header("Set Starting Offset")]
    [SerializeField] private Vector3 offset;


    void Update()
    {
        if (handSpot.transform.position.x > lemon.transform.position.x)
        {
            //todo snatch fx
            gameMaster.LoadHintScene();
        }

        var modifiedArmSpeed = armSpeed;

            offset =
                new Vector3(
                    offset.x + Time.deltaTime * modifiedArmSpeed,
                    offset.y - Time.deltaTime * modifiedArmSpeed,
                    offset.z
                    );
        arm.transform.position = lemon.transform.position + offset;


        // dont let arm extend farther than we want
        /*
        if (arm.transform.position.x < minPosition.x)
        {
            arm.transform.position = new Vector3(minPosition.x, arm.transform.position.y, arm.transform.position.z);
        }*/

    }
}
