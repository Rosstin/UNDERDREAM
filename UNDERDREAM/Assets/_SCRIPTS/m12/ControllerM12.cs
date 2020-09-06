using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerM12 : BaseController
{
    [Header("Phase Times")]
    public float[] Times;

    [Header("Sprites")]
    public GameObject NewHole;
    public GameObject NewSmoke;
    public GameObject Boat;

    [Header("Sfx")]
    public AudioSource CannonSound;

    private float elapsed;
    private bool[] flags = new []{false,false,false};

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        elapsed += Time.deltaTime;

        if (elapsed > Times[0] && !flags[0])
        {
            flags[0] = true;
            NewHole.SetActive(true);
            CannonSound.Play();
        }
        else if (elapsed > Times[1] && !flags[1])
        {
            flags[1] = true;
            NewSmoke.SetActive(true);
        }
        else if (elapsed > Times[2] && !flags[2])
        {
            flags[2] = true;
            LoadNextScene();
        }

    }
}
