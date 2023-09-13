using UnityEngine;
using UnityEngine.SceneManagement;

public class ReflectionEventController : BaseCutsceneController
{

    [Header("Timing")]
    public float Blink1Time;
    public float Blink2Time;
    public float BlinkDurationSeconds;
    public float AfterDurationSeconds;

    [Header("Outlets")]
    public AudioSource BlinkSFX;
    public GameObject ReflectionOpenEyeSprite;
    public GameObject ReflectionClosedEyeSprite;

    private bool blinked1 = false;
    private bool blinked2 = false;
    private float elapsed = 0f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool eyesOpen = true;

        elapsed += Time.deltaTime;

        if(elapsed > Blink1Time && !blinked1){
            BlinkSFX.Play();
            blinked1 = true;
        }

        if (elapsed > Blink2Time && !blinked2)
        {
            BlinkSFX.Play();
            blinked2 = true;
        }

        if (blinked1 && elapsed > Blink1Time && elapsed < Blink1Time + BlinkDurationSeconds)
        {
            eyesOpen = false;
        }

        if(blinked2 && elapsed > Blink2Time && elapsed < Blink2Time + BlinkDurationSeconds)
        {
            eyesOpen = false;
        }

        if(elapsed > Blink2Time + BlinkDurationSeconds + AfterDurationSeconds)
        {
            SceneManager.LoadScene("Boot", LoadSceneMode.Single);
        }

        ReflectionOpenEyeSprite.gameObject.SetActive(eyesOpen);
        ReflectionClosedEyeSprite.gameObject.SetActive(!eyesOpen);

    }
}
