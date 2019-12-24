using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lids : GameController
{
    [SerializeField] private AnimationCurve lidCurve;
    [SerializeField] private Lid topLid;
    [SerializeField] private Lid botLid;
    [SerializeField] private GameObject sky;

    [SerializeField] private float maxClosedness;
    [SerializeField] private float startingClosedness;

    private float closedness;

    private void Start()
    {
        closedness = startingClosedness;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        closedness += Time.deltaTime;

        if(closedness > maxClosedness)
        {
            closedness = maxClosedness;
        }

        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetMouseButtonDown(0)
            || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.UpArrow)
            )
        {
            closedness -= 1f;
        }

        if(closedness < 0f)
        {
            SceneManager.LoadScene("m03");
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
