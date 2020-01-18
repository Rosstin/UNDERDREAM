using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonController : MonoBehaviour
{
    public Transform Parent;

    private Vector3 lemonInitialLocalPosition;
    private Vector3 lemonInitialLocalScale;
    private Quaternion lemonInitialLocalRotation;

    private void Start()
    {
        lemonInitialLocalPosition = this.gameObject.transform.localPosition;
        lemonInitialLocalScale = this.gameObject.transform.localScale;
        lemonInitialLocalRotation = this.gameObject.transform.localRotation;
    }

    public void RepatriateLemon()
    {
        /*
        this.transform.SetParent(Parent);
        this.gameObject.transform.localPosition = lemonInitialLocalPosition;
        this.gameObject.transform.localScale = lemonInitialLocalScale;
        this.gameObject.transform.localRotation = lemonInitialLocalRotation;
        */
    }
}
