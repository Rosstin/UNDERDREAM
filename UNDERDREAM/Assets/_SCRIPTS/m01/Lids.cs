using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lids : MonoBehaviour
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
        closedness += Time.deltaTime;

        if(closedness > maxClosedness)
        {
            closedness = maxClosedness;
        }

        if (Input.anyKeyDown)
        {
            closedness -= 1f;
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
