using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beatkeeper : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private List<float> beatTimes;
    [SerializeField] private float beatSpan;

    [Header("SFX")]
    [SerializeField] private AudioSource success;
    [SerializeField] private AudioSource mistake;

    [Header("Prefabs")]
    [SerializeField] private GameObject hurdle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
