using UnityEngine;
using UnityEngine.SceneManagement;

public class WinRocket : MonoBehaviour
{
    [Header("Outlets")]
    public SpriteRenderer Rocket;
    public GameObject Cloud;
    public Transform RocketDestination;

    [Header("Animation")]
    public AnimationCurve Curve;
    public AnimationCurve RocketCurve;

    [Header("Time Threshholds")]
    public float SmokeDisappearDuration;
    public float RocketMoveDuration;

    private float curTime = 0f;
    private Vector3 cloudStartScale;
    private Vector3 rocketStartPosition;

    void Start()
    {
        cloudStartScale= Cloud.transform.localScale;
        rocketStartPosition = Rocket.transform.localPosition;
    }

    void Update()
    {
        curTime += Time.deltaTime;

        Cloud.transform.localScale = Vector3.Lerp(cloudStartScale, Vector3.zero, Curve.Evaluate(curTime / SmokeDisappearDuration));

        Rocket.transform.localPosition = Vector3.Lerp(rocketStartPosition, RocketDestination.localPosition, RocketCurve.Evaluate(curTime / RocketMoveDuration));

        if(curTime > RocketMoveDuration)
        {
            SceneManager.LoadScene("JellyCatD2");
        }

    }
}
