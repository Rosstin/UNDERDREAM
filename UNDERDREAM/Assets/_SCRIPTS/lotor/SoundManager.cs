using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private List<SoundRune> soundRunes;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SoundRune thunder;

    private int index = 0;

    private void Start()
    {
        IncrementIndex();
    }

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.RightArrow) )
        {
            IncrementIndex();
        }
		else if (Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DecrementIndex();
        }
		else if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Space))
        {
			PlayRune();
            Debug.Log("B");
        }
        else if (Input.GetKeyDown(KeyCode.F5) || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape or F5");
            PlayThunder();
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

	private void PlayRune(){
        int randomIndex = Random.Range(0, soundRunes[index].Sounds.Count - 1);
        
        soundRunes[index].Sounds[randomIndex].Play();
		//soundRunes[index].Sounds.Count
	}

    private void PlayThunder()
    {
        int randomIndex = Random.Range(0, thunder.Sounds.Count - 1);

        thunder.Sounds[randomIndex].Play();

    }

}
