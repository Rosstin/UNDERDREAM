using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class ArmControllerM90 : MonoBehaviour
{
    public BaseController BaseController;

    [Header("Lemon")]
    [SerializeField]private StairLemon lemon;
    [SerializeField] private Transform handSpot;
    [SerializeField] private GameMasterM90 gm;

    [Header("Arm")]
    [SerializeField] [Range(0f, 1f)] private float armSpeed;

    [Header("Arm Outlets")]
    [SerializeField] private GameObject arm;

    [Header("Set Starting Offset")]
    [SerializeField] private Vector3 offset;

    void Update()
    {
        if(this.handSpot.position.x > lemon.transform.position.x)
        {
            gm.LoadNextScene();
        }

        var modifiedArmSpeed = armSpeed;

        if (BaseController.CommandsHeldThisFrame.ContainsKey(BaseController.Command.Down) || BaseController.CommandsHeldThisFrame.ContainsKey(BaseController.Command.Right))
        {
            offset =
                new Vector3(
                    offset.x + Time.deltaTime * modifiedArmSpeed,
                    offset.y - Time.deltaTime * modifiedArmSpeed,
                    offset.z
                    );
        }

        else if (BaseController.CommandsHeldThisFrame.ContainsKey(BaseController.Command.Up) || BaseController.CommandsHeldThisFrame.ContainsKey(BaseController.Command.Left))
        {
            offset =
                new Vector3(
                    offset.x - Time.deltaTime * modifiedArmSpeed,
                    offset.y + Time.deltaTime * modifiedArmSpeed,
                    offset.z
                    );
        }

        arm.transform.position = lemon.transform.position + offset;

    }
}
