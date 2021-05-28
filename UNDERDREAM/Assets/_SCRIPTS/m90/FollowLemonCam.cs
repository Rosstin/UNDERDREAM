using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLemonCam : MonoBehaviour
{
    [SerializeField] private GameObject lemon;
    [SerializeField] private float z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(lemon.transform.position.x, lemon.transform.position.y, z);
    }
}
