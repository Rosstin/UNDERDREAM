using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShantyVertBite : MonoBehaviour
{

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform upPosition;
    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpDuration;

    [SerializeField] private SpriteRenderer openMouthSprite;
    [SerializeField] private SpriteRenderer closedMouthSprite;

    void Start()
    {
        this.transform.position = (startPosition.position);

        MouthOpen(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(JumpUp());
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(JumpDown());
        }
    }

    private void MouthOpen(bool open)
    {
        closedMouthSprite.gameObject.SetActive(!open);
        openMouthSprite.gameObject.SetActive(open);
    }

    private IEnumerator JumpUp()
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
    }

    private IEnumerator JumpDown()
    {
        float animTime = 0f;
        while (animTime < jumpDuration)
        {
            animTime += Time.deltaTime;
            float downProg = jumpCurve.Evaluate(animTime / jumpDuration);
            this.transform.position = Vector3.Lerp(upPosition.position, startPosition.position, downProg);
            yield return 0;
        }
        MouthOpen(false);
    }
}
