using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToDie : MonoBehaviour {
    public float totalTime = 2f;
    public float deltaTime = 0f;

    private Vector3 startingScale;

    void Start() {
        this.startingScale = this.transform.localScale;
    }
    void Update() {
        float progress = deltaTime / totalTime;
        this.transform.localScale = Vector3.Lerp(this.startingScale, Vector3.zero, progress);

        if (deltaTime >= totalTime) {
            this.gameObject.SetActive(false);
        } else {
            deltaTime += Time.deltaTime;
        }
    }
}