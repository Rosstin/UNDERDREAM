using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private List<SoundRune> soundRunes;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int index = 0;

    private void Start()
    {
        IncrementIndex();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Debug.Log("inc");
            IncrementIndex();
            Debug.Log("index " + index);
        }
        else if (Input.GetKeyDown(KeyCode.PageUp))
        {
            Debug.Log("dec");
            DecrementIndex();
            Debug.Log("index " + index);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("B");
        }
        else if (Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape or F5");
        }
    }

    private void IncrementIndex()
    {
        index++;
        if (index >= soundRunes.Count)
        {
            index = 0;
        }

        spriteRenderer.sprite = soundRunes[index].RuneSprite;
    }

    private void DecrementIndex()
    {
        index--;
        if (index < 0)
        {
            index = soundRunes.Count - 1;
        }

        spriteRenderer.sprite = soundRunes[index].RuneSprite;
    }

}
