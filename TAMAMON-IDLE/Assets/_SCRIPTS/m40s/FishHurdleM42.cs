using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHurdleM42 : FishHurdle
{
    public override void MakeCorrect()
    {
        base.MakeCorrect();

        // todo make it fly in moxie's mouth

        StartCoroutine(GetEaten());

    }

    private IEnumerator GetEaten()
    {
        // disappear with a mask after entering moxie's mouth
        // should it speed up into her mouth?

        // todo go to moxie's mouth then disappear
        float elapsed = 0f;
        const float PERIOD = 0.2f;

        //todo the fish should rotate and add an ease function

        Vector3 startPos = this.transform.position;

        while(elapsed < PERIOD)
        {
            elapsed += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPos, GoSpot, elapsed / PERIOD);

            Debug.LogWarning("this.transform.position " + this.transform.position + "   GoSpot: " + GoSpot + "  elapsed = " + elapsed);

            yield return 0;// new WaitForSeconds(0.02f);
        }
        this.gameObject.SetActive(false);
    }

}
