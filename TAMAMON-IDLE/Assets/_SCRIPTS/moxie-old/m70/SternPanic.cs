using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SternPanic : MonoBehaviour
{
    public Transform StartLocation;
    public Transform EndLocation;
    public float PanicPeriod;
    public SpriteRenderer MySprite;
    public SpriteRenderer OwnedSprite;
    public Hat Hat; 

    private bool lostHat = false;
    private float elapsed;
    private float progress;
    private int sign = 1;

    // hat goes flying off and seeks shanty
    public void LoseHat()
    {
        if (!lostHat)
        {
            // play whoops sound
            Hat.gameObject.SetActive(true);
            Hat.Fly();
            lostHat = true;
            this.MySprite.enabled = false;
            OwnedSprite.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MySprite.enabled = true;
        OwnedSprite.gameObject.SetActive(false);
        Hat.gameObject.SetActive(false);
        StartLocation.gameObject.SetActive(false);
        EndLocation.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!lostHat)
        {

        MySprite.flipX = sign == -1;

        elapsed += sign*Time.deltaTime;

        if(elapsed > PanicPeriod)
        {
            sign *= -1;
            elapsed = PanicPeriod;
        }
        else if(elapsed < 0)
        {
            sign *= -1;
            elapsed = 0f;
        }
        progress = elapsed / PanicPeriod;

        this.transform.localPosition = Vector3.Lerp(StartLocation.localPosition, EndLocation.localPosition, progress);
        }
    }
}
