using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoxieAndLemonSpinning : MonoBehaviour
{
    [Header("Outlets")]
    public Rigidbody2D Body;
    public BoxCollider2D MyCollider;
    public BoxCollider2D Ground;
    public GameObject Cloud;
    public AudioSource ThudSound;
    public AudioSource ExplosionSound;
    public GameObject SkiddingMoxie;
    public GameObject Lemon;

    [Header("Starting Motion")]
    public float StartingTorque;
    public Vector2 StartingForce;

    [Header("Lemon Offset")]
    public Vector3 LemonOffset;

    // Start is called before the first frame update
    void Start()
    {
        Body.AddForce(StartingForce);
        Body.AddTorque(StartingTorque);
    }

    // Update is called once per frame
    void Update()
    {
        if (Ground.IsTouching(MyCollider))
        {
            // generate the explosion and deactivate yourself
            this.gameObject.SetActive(false);
            Cloud.transform.position = this.gameObject.transform.position;
            Cloud.SetActive(true);
            ThudSound.Play();
            ExplosionSound.Play();
            SkiddingMoxie.gameObject.transform.position = this.transform.position;
            SkiddingMoxie.gameObject.SetActive(true);

            Lemon.gameObject.transform.position = this.transform.position + LemonOffset;
            Lemon.gameObject.SetActive(true);

        }
    }
}
