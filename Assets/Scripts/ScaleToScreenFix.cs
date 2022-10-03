using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToScreenFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResizeSpriteToScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void  ResizeSpriteToScreen()
    {

        

        float width = transform.localScale.x;
        float height = transform.localScale.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);
    }
}
