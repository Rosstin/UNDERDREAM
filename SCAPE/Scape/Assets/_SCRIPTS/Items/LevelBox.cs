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

    public float MINIMUM_SPAWN_SEPARATION = 10;


    public void Start()
    {
        foreach (Transform child in this.GetComponentsInChildren<Transform>(true)) {
            child.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        if (this.state == State.BROKEN) {
            return;
        }

        Vector3 currentCameraPosition = ArCamera.transform.position;
        Vector3 cameraVelcocity = (currentCameraPosition - lastCameraPosition) / Time.deltaTime;

        Vector3 cameraAcceleration = cameraVelcocity - this.lastCameraVelocity;
        float magnitude = cameraAcceleration.magnitude;

        if (magnitude > this.SHAKE_MAGNITUDE_THRESHOLD) {
            switch (this.state) {
                case State.STILL:
                    this.state = State.SHAKING;
                    break;
                case State.SHAKING:
                    this.timeSpentShaking += Time.deltaTime;
                    break;
            }
        } else if (magnitude < 0.1f) {
            this.state = State.STILL;
            this.timeSpentShaking = 0;
        }

        if (this.timeSpentShaking > this.SHAKE_TIME_THRESHOLD) {

            // When this breaks, first
            this.state = State.BROKEN;
            OnBroken();
        }

        lastCameraPosition = currentCameraPosition;
    }

    void OnBroken() {
        CreateSpawnPoints();
        this.gameObject.transform.SetParent(World.Instance.gameObject.transform);
        // establish the objects we want to spawn up
        // I guess we can make 'em children on the box?

        // we also need spawn points.
        // let's find the world's boundaries and use that as our spawn arena

        // once we have the dimensions of the world, spawn 'em        
    }

    void CreateSpawnPoints()
    {
        List<Vector2> spawnPoints = new List<Vector2>();
        Transform[] children = this.GetComponentsInChildren<Transform>(true);
        float groundPlaneY = this.gameObject.transform.position.y;
        Debug.Log("Ground floor Y " + groundPlaneY);

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

        Debug.Log(spawnPoints);

        for (int i = 0; i < spawnPoints.Count; ++i) {
            Transform child = children[i];
            Vector2 spawnPoint = spawnPoints[i];
            Debug.Log("Found point: " + spawnPoint);

            Ray spawnRay = ArCamera.ViewportPointToRay(spawnPoint);
            Vector3 direction = spawnRay.direction;
            Vector3 origin = spawnRay.origin;
            float distance = (groundPlaneY - spawnRay.origin.y) / direction.y; 
            Vector3 actualSpawnPoint = spawnRay.GetPoint(distance);

            Debug.Log("Spawn ray: " + spawnRay + "\nDistance: " + distance + "\n spawnPoint" + actualSpawnPoint);

            child.gameObject.SetActive(true);
            child.gameObject.transform.SetPositionAndRotation(actualSpawnPoint, Quaternion.identity);
            child.gameObject.transform.SetParent(World.Instance.transform);
        }
    }
}
