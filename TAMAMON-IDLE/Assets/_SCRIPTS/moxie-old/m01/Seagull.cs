using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    [SerializeField] float startDelay;

    private float elapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        animator.enabled = false;
        animator.speed = speed;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed > startDelay)
        {
            animator.enabled = true;
        }
    }
}
