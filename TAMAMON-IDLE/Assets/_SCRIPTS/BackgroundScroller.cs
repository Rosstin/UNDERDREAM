using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{

    public List<SpriteRenderer> BgPanels;
    public float Speed;
    public Vector2 MinMaxX;

    private float X_MARGIN = 0.02f;

    private bool scrollingEnabled = false;

    public void EnableScrolling(bool enabled)
    {
        scrollingEnabled = enabled;
    }

    private void Start()
    {
        // todo shuffle list
        //BgPanels

        for(int i = 0; i < BgPanels.Count; i++)
        {
            var cPanel = BgPanels[i];


            if (i == 0)
            {
                cPanel.transform.position = new Vector3(MinMaxX.x, cPanel.transform.position.y, cPanel.transform.position.z);
            }
            else
            {
                var prevPanel = BgPanels[i-1];

                cPanel.transform.position = new Vector3(
                    prevPanel.transform.position.x+(prevPanel.bounds.size.x/2f)+(cPanel.bounds.size.x/2f) - X_MARGIN, 
                    cPanel.transform.position.y, cPanel.transform.position.z);
            }
        }

    }

    void Update()
    {

        if (!scrollingEnabled)
        {
            return;
        }

        // when the panel goes off screen, place it at the back of the current last panel
        for (int i = 0; i < BgPanels.Count; i++)
        {
            var cPanel = BgPanels[i];
            var prevPanel = (i == 0) ? BgPanels[BgPanels.Count-1] : BgPanels[i - 1];
            var nextPanel = (i == BgPanels.Count - 1) ? BgPanels[0] : BgPanels[i + 1];

            cPanel.transform.position += new Vector3( Speed * Time.deltaTime, 0);

            if (cPanel.transform.position.x < MinMaxX.x)
            {
                cPanel.transform.position
                    =
                    new Vector3(prevPanel.transform.position.x + (prevPanel.bounds.size.x / 2f) + (cPanel.bounds.size.x / 2f) - X_MARGIN,
                    cPanel.transform.position.y,
                    cPanel.transform.position.z);
            }

            if (cPanel.transform.position.x > MinMaxX.y)
            {
                cPanel.transform.position
                    =
                    new Vector3(nextPanel.transform.position.x - (nextPanel.bounds.size.x / 2f) - (cPanel.bounds.size.x / 2f) + X_MARGIN,
                        cPanel.transform.position.y,
                        cPanel.transform.position.z
                    );
            }
        }
    }
}
