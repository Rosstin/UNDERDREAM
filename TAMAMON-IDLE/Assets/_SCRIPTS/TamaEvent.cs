using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TamaEvent : MonoBehaviour
{
    public abstract GameObject PreviewObject { get; }

    public abstract bool EventCanFire();
    public abstract void Play();
}
