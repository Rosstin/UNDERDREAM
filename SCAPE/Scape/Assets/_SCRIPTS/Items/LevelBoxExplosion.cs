using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoxExplosion : MonoBehaviour {
    public Transform objectToExplode;

    public Vector3 destinationPosition;
    private Vector3 startPosition;

    public float totalTime = 2f;
    public float deltaTime = 0f;

    public void Initialize(
        Transform objectToExplode,
        Vector3 destinationPosition
    ) {
        objectToExplode.gameObject.SetActive(true);
        objectToExplode.SetParent(this.transform);
        this.startPosition = this.transform.position;
        this.destinationPosition = destinationPosition;
        this.transform.SetParent(World.Instance.transform);
    }

    void Update() {
        float progress = deltaTime / totalTime;
        this.transform.position = Vector3.Lerp(startPosition, destinationPosition, progress);
        this.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);

        if (deltaTime >= totalTime) {
            // unparent all children
            foreach (Transform child in this.gameObject.GetComponentsInChildren<Transform>(true)) {
                child.SetParent(World.Instance.transform);
                child.position = this.transform.position;
            }
            Destroy(this.gameObject);
        } else {
            deltaTime += Time.deltaTime;
        }
    }
}