using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTimer : MonoBehaviour
{
    [Header("Configurables")]
    public float TitleDuration;

    private float curTime = 0f;

    void Start()
    {
        curTime = 0f;
    }

    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > TitleDuration)
        {
            SceneManager.LoadScene("JellyCatD2");
        }
    }
}
