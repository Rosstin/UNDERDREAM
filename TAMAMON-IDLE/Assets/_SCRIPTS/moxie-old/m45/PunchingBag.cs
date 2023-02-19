using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBag : Hurdle
{
    [SerializeField] private List<SpriteRenderer> damageSprites;
    [SerializeField] [Range(1,4)] private int hitsPerSprite;

    private int totalDamage = 0;
    private int subDamage = 0;

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
        int spriteIndex = totalDam / hitsPerSprite;

        this.totalDamage = totalDam;
        foreach (SpriteRenderer s in damageSprites)
        {
            s.gameObject.SetActive(false);
        }

        if(totalDam < damageSprites.Count)
        {
            damageSprites[spriteIndex].gameObject.SetActive(true);
        }
    }

}
