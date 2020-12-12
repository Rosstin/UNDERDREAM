using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBag : Hurdle
{
    [SerializeField] private List<SpriteRenderer> damageSprites;

    private int totalDamage = 0;

    private void Start()
    {
        SetDamage(0);
    }

    public void TakeDamage()
    {
        SetDamage(totalDamage + 1);
    }

    public void SetDamage(int totalDam)
    {
        this.totalDamage = totalDam;
        foreach (SpriteRenderer s in damageSprites)
        {
            s.gameObject.SetActive(false);
        }

        damageSprites[totalDam].gameObject.SetActive(true);
    }

}
