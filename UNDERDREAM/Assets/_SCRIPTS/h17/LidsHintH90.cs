using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LidsHintH90 : BaseController
{

    [SerializeField] private Lid topLid;
    [SerializeField] private Lid botLid;
    [SerializeField] private GameObject sky;

    [SerializeField] private float maxClosedness;
    [SerializeField] private float startingClosedness;

    [SerializeField] [Range(0f, 3f)] private float eyeOpeningPowerPerKeystroke;
    [SerializeField] [Range(0f, 3f)] private float closingPower;

    private float closedness;

    public new void Start()
    {
        base.Start();
        closedness = startingClosedness;
    }

    // Update is called once per frame
    private void Update()
    {
        BaseUpdate();

        if (Input.GetKey(KeyCode.Space) &&
            (
            Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
            )
            )
        {
            LoadRegularScene();
        }



        closedness += Time.deltaTime * closingPower;

        closedness = Mathf.Clamp(closedness, 0, maxClosedness);

        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.UpArrow)
        )
        {
            closedness -= eyeOpeningPowerPerKeystroke;
        }

        topLid.transform.position =
            Vector3.Lerp
            (topLid.openPosition.position,
                topLid.closedPosition.position,
                closedness / maxClosedness);

        botLid.transform.position =
            Vector3.Lerp
            (botLid.openPosition.position,
                botLid.closedPosition.position,
                closedness / maxClosedness);
    }
}

