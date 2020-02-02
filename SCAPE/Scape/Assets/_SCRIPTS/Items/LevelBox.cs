using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBox : MonoBehaviour
{
    public enum BoxState {
        STILL,
        SHAKING,
        BROKEN,
    }

    // Keep track of the positions every .1 sec over the last 2 seconds.
    private Vector3 lastCameraVelocity = Vector3.zero;
    private Vector3 lastCameraPosition = Vector3.zero;

    public BoxState state = BoxState.STILL;
    public float timeSpentShaking = 0;

    public Camera ArCamera;
    
    public Sprite openBoxSprite;

    public float SHAKE_MAGNITUDE_THRESHOLD = 1f;
    public float SHAKE_TIME_THRESHOLD = 1.5f;

    public float MINIMUM_SPAWN_SEPARATION = 10;


    public void Start()
    {
        foreach (Transform child in this.GetComponentsInChildren<Transform>(true)) {
            child.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        if (this.state == BoxState.BROKEN) {
            return;
        }

        Vector3 currentCameraPosition = ArCamera.transform.position;
        Vector3 cameraVelcocity = (currentCameraPosition - lastCameraPosition) / Time.deltaTime;

        Vector3 cameraAcceleration = cameraVelcocity - this.lastCameraVelocity;
        float magnitude = cameraAcceleration.magnitude;

        Debug.Log("Magnitude: " + magnitude);
        switch (this.state) {
            case BoxState.STILL: {
                if (magnitude > this.SHAKE_MAGNITUDE_THRESHOLD) {
                    this.state = BoxState.SHAKING;
                    this.timeSpentShaking = 0;
                }
                break;
            }
            case BoxState.SHAKING: {
                this.timeSpentShaking += 
                    (magnitude > this.SHAKE_MAGNITUDE_THRESHOLD ? 1 : -0.3f) * Time.deltaTime;
                break;
            }
            case BoxState.BROKEN:
                return;
        }

        if (this.timeSpentShaking > this.SHAKE_TIME_THRESHOLD) {
            this.state = BoxState.BROKEN;
            OnBroken();
        } else if (this.timeSpentShaking < 0) {
            this.state = BoxState.STILL;
        }

        lastCameraPosition = currentCameraPosition;
    }

    void OnBroken() {
        Transform[] children = this.GetComponentsInChildren<Transform>(true);
        List<Vector2> spawnPoints = CreateSpawnPoints(children);

        float groundPlaneY = this.gameObject.transform.position.y;

        for (int i = 0; i < spawnPoints.Count; ++i) {
            Transform child = children[i];
            Vector2 spawnPoint = spawnPoints[i];

            Ray spawnRay = ArCamera.ViewportPointToRay(spawnPoint);
            Vector3 direction = spawnRay.direction;
            Vector3 origin = spawnRay.origin;
            float distance = (groundPlaneY - spawnRay.origin.y) / direction.y; 
            Vector3 actualSpawnPoint = spawnRay.GetPoint(distance);

            GameObject explosion = new GameObject();
            explosion.transform.position = this.transform.position;
            explosion.AddComponent<LevelBoxExplosion>().Initialize(
                child.transform,
                actualSpawnPoint
            );
        }

        this.GetComponent<SpriteRenderer>().sprite = openBoxSprite;
        this.transform.SetParent(World.Instance.gameObject.transform);  
    }

    List<Vector2> CreateSpawnPoints(Transform[] children)
    {
        List<Vector2> spawnPoints = new List<Vector2>();

        // in theory this is super dangerous :D :D :D
        while (spawnPoints.Count < children.Length) {
            Vector2 candidateSpawn = new Vector2(
                Random.Range(.1f, .9f),
                Random.Range(.1f, .9f)
            );

            bool isValidSpawn = true;
            foreach (Vector2 spawnPoint in spawnPoints) {
                if (Vector2.Distance(candidateSpawn, spawnPoint) < MINIMUM_SPAWN_SEPARATION/100f) {
                    isValidSpawn = false;
                    break;
                }
            }

            if (isValidSpawn) {
                spawnPoints.Add(candidateSpawn);
            }
        }

        return spawnPoints;
    }
}
