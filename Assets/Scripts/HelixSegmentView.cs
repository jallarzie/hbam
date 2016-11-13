using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelixSegmentView : MonoBehaviour {

    public Image background;
    public Image cg;
    public Image at;

    private void Awake()
    {
        if (cg != null)
        {
            cg.CrossFadeAlpha(0f, 0f, false);
        }
        if (at != null)
        {
            at.CrossFadeAlpha(0f, 0f, false);
        }
    }
}
