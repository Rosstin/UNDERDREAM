using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLemonCam : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject lemon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(offset.x+lemon.transform.position.x, offset.y+lemon.transform.position.y, offset.z);
    }
}
