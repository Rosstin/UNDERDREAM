using UnityEngine;

public class RotateGem : MonoBehaviour
{
    [Range(-360,360)] public float dpsUp = 10f; // degrees per second around up
    [Range(-360,360)] public float dpsLeft = 10f; // degrees per second around left
    [Range(-360,360)] public float dpBack = 10f; // degrees per second around back

    void Update()
    {
        transform.Rotate(Vector3.up, dpsUp * Time.deltaTime);
        transform.Rotate(Vector3.left, dpsLeft * Time.deltaTime);
        transform.Rotate(Vector3.back, dpBack * Time.deltaTime);
    } 
}
