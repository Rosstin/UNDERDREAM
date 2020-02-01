using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBox : MonoBehaviour
{
    enum State {
        STILL,
        SHAKING,
        BROKEN,
    }

    // Keep track of the positions every .1 sec over the last 2 seconds.
    private Vector3 lastCameraVelocity = Vector3.zero;
    private Vector3 lastCameraPosition = Vector3.zero;

    private State state = State.STILL;
    private float timeSpentShaking = 0;

    public Camera ArCamera;
    
    public float SHAKE_MAGNITUDE_THRESHOLD = 1f;
    public float SHAKE_TIME_THRESHOLD = 1.5f;


    public void OnTrackingFound()
    {
        Debug.Log("ONTRACKINGFOUND lemon");

        this.lastCameraVelocity = Vector3.zero;
        this.lastCameraPosition = ArCamera.transform.position;
        this.state = State.STILL;
    }
    
    void Update()
    {
        Vector3 currentCameraPosition = ArCamera.transform.position;
        Vector3 cameraVelcocity = (currentCameraPosition - lastCameraPosition) / Time.deltaTime;

        Vector3 cameraAcceleration = cameraVelcocity - this.lastCameraVelocity;
        float magnitude = cameraAcceleration.magnitude;

        Debug.Log("Magnitude found " + magnitude);

        if (magnitude > this.SHAKE_MAGNITUDE_THRESHOLD) {
            switch (this.state) {
                case State.STILL:
                Debug.Log("Shake");
                    this.state = State.SHAKING;
                    break;
                case State.SHAKING:
                    this.timeSpentShaking += Time.deltaTime;
                    break;
            }
        } else if (magnitude < 0.1f) {
            this.state = State.STILL;
            Debug.Log("Still");
            this.timeSpentShaking = 0;
        }

        if (this.timeSpentShaking > this.SHAKE_TIME_THRESHOLD) {
            Debug.Log("BREAK");
            this.state = State.BROKEN;
            this.transform.SetParent(World.Instance.gameObject.transform);
        }

        lastCameraPosition = currentCameraPosition;
    }
}
