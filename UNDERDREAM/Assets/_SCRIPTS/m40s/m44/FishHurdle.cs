using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHurdle : Hurdle
{
    public Vector3 GoSpot;

    public override void MakeCorrect()
    {
        Debug.LogWarning("do fish flippy thing");
        //myState = HurdleState.Wrong;
        //spriteRenderer.color = wrongColor;

        // todo make it fly in moxie's mouth

        StartCoroutine(GetEaten());

    }

    private IEnumerator GetEaten()
    {
        // todo go to moxie's mouth then disappear
        float elapsed = 0f;
        const float PERIOD = 0.2f;
        elapsed += Time.deltaTime;

        Vector3 startPos = this.transform.position;

        if(elapsed < PERIOD)
        {
            Vector3.Lerp(startPos, GoSpot, elapsed / PERIOD);
            yield return new WaitForSeconds(0.02f);
        }
        //this.gameObject.SetActive(false);
    }

}
