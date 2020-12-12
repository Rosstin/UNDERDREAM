using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShantyVertBite : MonoBehaviour
{

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform upPosition;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float hangTime;

    [SerializeField] private SpriteRenderer openMouthSprite;
    [SerializeField] private SpriteRenderer closedMouthSprite;


    private bool jumping;

    // when you press up, she quickly ascends to a specified position and holds it for a specified time

    void Start()
    {
        this.transform.position = (startPosition.position);

        MouthOpen(false);
        jumping = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !jumping)
        {
            // start jump
            jumping = true;
            StartCoroutine(Jump());
        }
    }

    private void MouthOpen(bool open)
    {
        closedMouthSprite.gameObject.SetActive(!open);
        openMouthSprite.gameObject.SetActive(open);
    }

    private IEnumerator Jump()
    {
        MouthOpen(true);
        float animTime = 0f;
        while (animTime < jumpDuration)
        {
            animTime += Time.deltaTime;
            float upProgress = jumpCurve.Evaluate(animTime / jumpDuration);
            this.transform.position = Vector3.Lerp(startPosition.position, upPosition.position, upProgress);
            yield return 0;
        }
        yield return new WaitForSeconds(hangTime);
        animTime = 0f;
        while (animTime < jumpDuration)
        {
            animTime += Time.deltaTime;
            float downProg = jumpCurve.Evaluate(animTime / jumpDuration);
            this.transform.position = Vector3.Lerp(upPosition.position, startPosition.position, downProg);
            yield return 0;
        }
        jumping = false;
        MouthOpen(false);
    }
}
