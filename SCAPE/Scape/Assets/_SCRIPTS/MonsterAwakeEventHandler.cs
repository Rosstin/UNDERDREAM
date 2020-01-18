using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAwakeEventHandler : DefaultTrackableEventHandler
{
    [Header("Outlets")]
    public GameObject Monster;

    #region PROTECTED_METHODS

    protected override void OnTrackingFound()
    {
        Debug.Log("ONTRACKINGFOUND");

        Monster.gameObject.SetActive(true);

        base.OnTrackingFound();
    }

    #endregion // PROTECTED_METHODS
}
