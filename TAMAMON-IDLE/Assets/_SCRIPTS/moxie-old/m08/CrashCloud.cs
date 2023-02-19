using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashCloud : MonoBehaviour
{
    [Header("Parameters")]
    public float ExpandTime;
    public float DisappearTime;

    [Header("Outlets")]
    public Animator Expanding;
    public Animator Looping;

    private float timeElapsed =0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed > ExpandTime)
        {
            Expanding.gameObject.SetActive(false);
            Looping.gameObject.SetActive(true);
        }

        if(timeElapsed > DisappearTime)
        {
            Looping.gameObject.SetActive(false);
        }
    }
}
