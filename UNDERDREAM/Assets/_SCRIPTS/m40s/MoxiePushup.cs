using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoxiePushup : MonoBehaviour
{
    [SerializeField] private GameObject downSprite;
    [SerializeField] private GameObject upSprite;
    [SerializeField] private float hangTime;

    //private bool jumping;

    private void Up()
    {
        downSprite.gameObject.SetActive(false);
        upSprite.gameObject.SetActive(true);
    }

    private void Down()
    {
        downSprite.gameObject.SetActive(true);
        upSprite.gameObject.SetActive(false);
    }

    void Start()
    {
        Down();
        //jumping = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Down();
        }else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Up();
        }

        /*
        if (Input.GetKeyDown(KeyCode.UpArrow) && !jumping)
        {
            // start jump
            jumping = true;
            StartCoroutine(Jump());
        }
        */
    }


    /*
    private IEnumerator Jump()
    {
        float animTime = 0f;
        yield return new WaitForSeconds(hangTime);
        jumping = false;
    }
    */
}
