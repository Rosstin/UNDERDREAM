using UnityEngine;

public class StretchyTape : MonoBehaviour
{
    [Header("Outlets")]
    public StretchyTapeBody Body;

    private float initialLength;
    private Vector3 initialLocalScale;

    public bool visible;

    public void SetVisibility(bool vis)
    {
        this.visible = vis;

        Body.gameObject.SetActive(this.visible);
    }

    void Start()
    {
        initialLength = Body.transform.localScale.x;
        initialLocalScale = Body.transform.localScale;
    }

    public void DrawTapeBetween(Vector3 a, Vector3 b)
    {
        // deform it
        float length = Mathf.Abs(Vector3.Distance(a, b));
        Body.transform.localScale = new Vector3(initialLocalScale.x, initialLocalScale.y, length);

        // place at midpoint
        Body.transform.position = (a + b) / 2f;

        // rotate it // its forward transform should point to one endpoint
        Body.transform.LookAt(a);

        Body.gameObject.SetActive(visible);
    }

}
