using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [Header("Outlets")]
    public SpriteRenderer OhNoSprite;
    public Animator OhNoAnimator;
    public AudioSource OhNoSfx;

    private float elapsed = 0f;

    private float OhNoAnimTime = 1f;
    private float OhNoAnimSoundTime = 1.6f;

    private bool havePlayedOhNo = false;
    private bool havePlayedOhNoSFX = false;

    public void Start()
    {
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > OhNoAnimTime && !havePlayedOhNo)
        {
            havePlayedOhNo = true;
            PlayOhNoAnim();
        }
        if(elapsed > OhNoAnimSoundTime && !havePlayedOhNoSFX)
        {
            havePlayedOhNoSFX = true;
            OhNoSfx.Play();
        }
    }

    public void PlayOhNoAnim()
    {
        OhNoSprite.gameObject.SetActive(true);
        OhNoAnimator.SetTrigger("OhNoBounce");
    }
}
