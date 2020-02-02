using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeAwakeEventHandler : DefaultTrackableEventHandler
{
    [Header("Outlets")]
    public GameObject Tape;

    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        Tape.gameObject.SetActive(true);

        base.OnTrackingFound();
    }

    protected override void OnTrackingLost()
    {
        Tape.gameObject.SetActive(false);

        base.OnTrackingLost();
    }


    #endregion // PROTECTED_METHODS
}
