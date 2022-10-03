using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOptions : MonoBehaviour
{
    private MenuScript menu;

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("SceneTransitionManager").GetComponent<MenuScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeScene()
    {
        menu.ChangeScene();
    }

    public void ExitGame()
    {
        menu.Quit();
    }
}
