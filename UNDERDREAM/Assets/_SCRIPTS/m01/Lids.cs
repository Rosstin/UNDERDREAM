using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lids : BaseController
{
    [SerializeField] private Lid topLid;
    [SerializeField] private Lid botLid;
    [SerializeField] private GameObject sky;

    [SerializeField] private float maxClosedness;
    [SerializeField] private float startingClosedness;

    [SerializeField] [Range(0f, 3f)] private float eyeOpeningPowerPerKeystroke;
    [SerializeField] [Range(0f, 3f)] private float closingPower;

    private float closedness;

    new public void Start()
    {
        base.Start();
        closedness = startingClosedness;
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        closedness += Time.deltaTime * closingPower;

        if(closedness > maxClosedness)
        {
            closedness = maxClosedness;
        }

        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.UpArrow)
            )
        {
            closedness -= eyeOpeningPowerPerKeystroke;
        }

        if(closedness < 0f)
        {
            LoadNextScene();
        }

        topLid.transform.position= 
            Vector3.Lerp
            (topLid.openPosition.position, 
            topLid.closedPosition.position, 
            closedness/maxClosedness);

        botLid.transform.position =
            Vector3.Lerp
            (botLid.openPosition.position,
            botLid.closedPosition.position,
            closedness / maxClosedness);
    }
}
