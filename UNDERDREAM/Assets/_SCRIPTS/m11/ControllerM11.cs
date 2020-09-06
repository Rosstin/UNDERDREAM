using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerM11 : BaseController
{
    public float SceneTime;

    private float elapsed;

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();
        elapsed += Time.deltaTime;
        if (elapsed > SceneTime)
        {
            elapsed = -100000f;
            LoadNextScene();
        }
    }
}
