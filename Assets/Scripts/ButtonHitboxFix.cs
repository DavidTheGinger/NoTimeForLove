using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHitboxFix : MonoBehaviour
{
    private Image image_displayed;
    // Start is called before the first frame update
    void Start()
    {
        image_displayed = GetComponent<Image>();
        image_displayed.alphaHitTestMinimumThreshold = 0.5f;
    }

}
