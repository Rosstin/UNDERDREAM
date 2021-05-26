using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternPanic : MonoBehaviour
{
    public Transform StartLocation;
    public Transform EndLocation;
    public float PanicPeriod;
    public SpriteRenderer MySprite;

    private float elapsed;
    private float progress;
    private int sign = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartLocation.gameObject.SetActive(false);
        EndLocation.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MySprite.flipX = sign == -1;

        elapsed += sign*Time.deltaTime;

        if(elapsed > PanicPeriod)
        {
                elapsed = PanicPeriod;
            sign *= -1;
        }else if(elapsed < 0)
        {
            sign *= -1;
            elapsed = 0f;
        }
        progress = elapsed / PanicPeriod;

        this.transform.localPosition = Vector3.Lerp(StartLocation.localPosition, EndLocation.localPosition, progress);
    }
}
