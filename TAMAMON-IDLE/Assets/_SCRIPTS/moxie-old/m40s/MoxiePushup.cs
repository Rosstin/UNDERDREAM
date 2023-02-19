using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoxiePushup : MonoBehaviour
{
    [SerializeField] private GameObject downSprite;
    [SerializeField] private GameObject upSprite;
    [SerializeField] private float hangTime;

    [SerializeField] private BaseController baseController;

    private void Up()
    {
        downSprite.gameObject.SetActive(false);
        upSprite.gameObject.SetActive(true);
    }

    private void Down()
    {
        downSprite.gameObject.SetActive(true);
        upSprite.gameObject.SetActive(false);
    }

    void Start()
    {
        Down();
    }

    void Update()
    {
        if (baseController.CommandsStartedThisFrame.ContainsKey(BaseController.Command.Down))
        {
            Down();
        }else if (baseController.CommandsStartedThisFrame.ContainsKey(BaseController.Command.Up))
        {
            Up();
        }
    }
}
