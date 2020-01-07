using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float armSpeed;
    [SerializeField] private float maxHeight;

    [SerializeField] private GameObject arm;
    [SerializeField] private GameObject clenchedArm;

    [SerializeField] private float jitterPeriod;
    [SerializeField] private Vector3 jitter;

    private float jitterElapsed = 0;

    // Update is called once per frame
    void Update()
    {
        jitterElapsed += Time.deltaTime;

        if (jitterElapsed > jitterPeriod)
        {
            jitterElapsed = 0f;
            this.gameObject.transform.localPosition += new Vector3(
                    Random.Range(-jitter.x, jitter.x),
                    Random.Range(-jitter.y, jitter.y),
                    Random.Range(-jitter.z, jitter.z));
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x,
                    arm.transform.localPosition.y + Time.deltaTime * armSpeed,
                    arm.transform.localPosition.z
                    );
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x,
                    arm.transform.localPosition.y - Time.deltaTime * armSpeed,
                    arm.transform.localPosition.z
                    );
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x - Time.deltaTime * armSpeed,
                    arm.transform.localPosition.y,
                    arm.transform.localPosition.z
                    );
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            arm.transform.localPosition =
                new Vector3(
                    arm.transform.localPosition.x + Time.deltaTime * armSpeed,
                    arm.transform.localPosition.y,
                    arm.transform.localPosition.z
                    );
        }

        if(arm.transform.position.y > maxHeight)
        {
            arm.SetActive(false);
            clenchedArm.SetActive(true);

            clenchedArm.transform.position = new Vector3(
                arm.transform.position.x,
                maxHeight,
                arm.transform.position.z
                );
        } 

    }
}
