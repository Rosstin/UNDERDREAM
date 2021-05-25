using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] private Transform topAnchor;
    [SerializeField] private Transform botAnchor;

    private float upRatio = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(botAnchor.position, topAnchor.position, upRatio);
    }
}
