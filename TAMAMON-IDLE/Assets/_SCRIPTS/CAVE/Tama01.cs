using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tama01 : MonoBehaviour
{
    [Header("Animation Outlets")]
    public GameObject WalkAnimation;
    public GameObject IdleAnimation;
    public enum Tama01AnimationState
    {
        WALK,
        IDLE
    }

    public void StartAnimation(Tama01AnimationState animationState)
    {
        WalkAnimation.SetActive(false);
        IdleAnimation.SetActive(false);
        switch (animationState)
        {
            case Tama01AnimationState.WALK:
                WalkAnimation.gameObject.SetActive(true);
                break;
            case Tama01AnimationState.IDLE:
                IdleAnimation.gameObject.SetActive(true);
                break;
        }
    }


}
