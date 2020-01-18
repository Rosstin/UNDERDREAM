using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemonAwakeEventHandler : DefaultTrackableEventHandler
{
    [Header("Outlets")]
    public LemonController Lemon;

    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        Debug.Log("ONTRACKINGFOUND lemon");

        Lemon.gameObject.SetActive(true);

        base.OnTrackingFound();
    }

    #endregion // PROTECTED_METHODS
}
