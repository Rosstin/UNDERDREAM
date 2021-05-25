using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [Header("Outlet: Top and Bottom Anchors")]
    [SerializeField] private Transform topAnchor;
    [SerializeField] private Transform botAnchor;

    [Header("Top Front Housings")]
    [SerializeField] private GameObject topFrontFoot;
    [SerializeField] private GameObject topFrontSheath;
    [SerializeField] private LineRenderer topFrontLR;

    [Header("Top Back Housings")]
    [SerializeField] private GameObject topBackFoot;
    [SerializeField] private GameObject topBackSheath;
    [SerializeField] private LineRenderer topBackLR;

    [Header("Bot Front Housings")]
    [SerializeField] private GameObject botFrontFoot;
    [SerializeField] private GameObject botFrontSheath;
    [SerializeField] private LineRenderer botFrontLR;

    [Header("Bot Back Housings")]
    [SerializeField] private GameObject botBackFoot;
    [SerializeField] private GameObject botBackSheath;
    [SerializeField] private LineRenderer botBackLR;

    private float upRatio = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(botAnchor.position, topAnchor.position, upRatio);

        topFrontLR.SetPosition(0, topFrontFoot.transform.position);
        topFrontLR.SetPosition(1, topFrontSheath.transform.position);

        topBackLR.SetPosition(0, topBackFoot.transform.position);
        topBackLR.SetPosition(1, topBackSheath.transform.position);

        botFrontLR.SetPosition(0, botFrontFoot.transform.position);
        botFrontLR.SetPosition(1, botFrontSheath.transform.position);

        botBackLR.SetPosition(0, botBackFoot.transform.position);
        botBackLR.SetPosition(1, botBackSheath.transform.position);

    }
}
