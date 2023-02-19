using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D col;

    [Header("Bonk Moxie Movement")]
    [SerializeField] private Transform MoxieHead;
    [SerializeField] private AnimationCurve TravelCurve;
    [SerializeField] private float TravelDuration;
    [SerializeField] private MaoxunM23 Moxie;

    [Header("After Bonk")]
    public float AfterBonkTorque;
    public Vector2 AfterBonkForce;


    private Vector3 initPos;

    private void Start()
    {
        rb.gravityScale = 0f;
    }

    public void Drop()
    {
        //rb.gravityScale = 1f;
        this.transform.parent = null;

        // home in on moxie's head, when you hit it, knock her into the sand
        initPos = this.transform.position;
        StartCoroutine(BonkMoxie(initPos, MoxieHead));
    }

    private IEnumerator BonkMoxie(Vector3 start, Transform end)
    {
        float elapsed = 0;
        bool hitMoxie = false;
        while (!hitMoxie)
        {
            elapsed += Time.deltaTime;
            float progress = TravelCurve.Evaluate(elapsed / TravelDuration);
            this.transform.position = Vector3.Lerp(start, end.transform.position, progress);

            if(progress >= 1)
            {
                hitMoxie = true;
                Moxie.GetBonked();
            }

            yield return 0;
        }

        rb.AddForce(AfterBonkForce);
        rb.AddTorque(AfterBonkTorque);
        rb.gravityScale = 1f;


        // after you hit moxie, apply force and go flying to the right
    }

}
