using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHurdle : Hurdle
{
    public Vector3 GoSpot;

    public void SetGoSpot(Vector3 goSpot)
    {
        this.GoSpot = goSpot;
    }

    public override void MakeCorrect()
    {
        base.MakeCorrect();

        // todo make it fly in moxie's mouth

        StartCoroutine(GetEaten());
    }

    private IEnumerator GetEaten()
    {
        // todo go to moxie's mouth then disappear
        float elapsed = 0f;
        const float PERIOD = 0.2f;

        //todo the fish should rotate and add an ease function

        Vector3 startPos = this.transform.position;

        while(elapsed < PERIOD)
        {
            elapsed += Time.deltaTime;
            this.transform.position = Vector3.Lerp(startPos, GoSpot, elapsed / PERIOD);

            yield return 0;// new WaitForSeconds(0.02f);
        }
        this.gameObject.SetActive(false);
    }

}
