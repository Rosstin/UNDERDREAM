using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBox : MonoBehaviour
{
    public Boxable[] SpawnableChildren;

    public enum BoxState {
        STILL,
        SHAKING,
        BROKEN,
    }

    // Keep track of the positions every .1 sec over the last 2 seconds.
    private Vector2 lastBoxVelocity = Vector3.zero;
    private Vector2 lastBoxPosition = Vector3.zero;

    private BoxState state = BoxState.STILL;
    private float timeSpentShaking = 0;

    public Camera ArCamera;
    
    public Sprite openBoxSprite;

    public float SHAKE_MAGNITUDE_THRESHOLD = 1f;
    public float SHAKE_TIME_THRESHOLD = 1.5f;

    public float MINIMUM_SPAWN_SEPARATION = 20;

    public void Start()
    {
        foreach (Boxable child in SpawnableChildren) {
            child.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        if (this.state == BoxState.BROKEN) {
            return;
        }

        Vector2 screenPointOfBox = ArCamera.WorldToScreenPoint(this.transform.position);

        Vector2 boxVelocity = (screenPointOfBox - lastBoxPosition) / Time.deltaTime;

        Vector2 boxAcceleration = boxVelocity - this.lastBoxVelocity;

        float magnitude = boxAcceleration.magnitude;

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

        lastBoxPosition = screenPointOfBox;
    }

    private Transform[] GetTransformsFromBoxables(Boxable[] boxables)
    {
        Transform[] transforms = new Transform[boxables.Length];
        for(int i = 0; i < boxables.Length; i++)
        {
            transforms[i] = boxables[i].transform;
        }
        return transforms;
    }

    void OnBroken() {

        foreach(Boxable boxed in SpawnableChildren)
        {
            boxed.UnBox();
        }

        Transform[] transforms = GetTransformsFromBoxables(SpawnableChildren);

        List<Vector2> spawnPoints = CreateSpawnPoints(transforms);

        float groundPlaneY = this.gameObject.transform.position.y;

        for (int i = 0; i < spawnPoints.Count; ++i) {
            Transform child = transforms[i];
            Vector2 spawnPoint = spawnPoints[i];

            Ray spawnRay = ArCamera.ViewportPointToRay(spawnPoint);
            Vector3 direction = spawnRay.direction;
            Vector3 origin = spawnRay.origin;
            float distance = (groundPlaneY - spawnRay.origin.y) / direction.y; 
            Vector3 actualSpawnPoint = spawnRay.GetPoint(distance);

            GameObject explosion = new GameObject("EXPLOSION");
            explosion.transform.SetParent(World.Instance.gameObject.transform);
            explosion.transform.position = this.transform.position;
            explosion.AddComponent<LevelBoxExplosion>().Initialize(
                child.transform,
                actualSpawnPoint
            );
        }

        this.GetComponent<SpriteRenderer>().sprite = openBoxSprite;
        GameObject tracker = (this.transform.parent.gameObject);
        this.transform.SetParent(World.Instance.gameObject.transform);
        Destroy(tracker);
        this.gameObject.AddComponent<TimeToDie>();
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
