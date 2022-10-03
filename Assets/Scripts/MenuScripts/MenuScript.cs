using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    enum FadeState { Idle, FadeOut, FadeIn }

    private FadeState f = FadeState.Idle;
    private float FadeTime = 0;
    [SerializeField] private float FadeDuration = 0.5f;
    [SerializeField] private string SceneName;

    private float Transparency = 0;
    [SerializeField] private Image fader;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        fader = GameObject.FindGameObjectWithTag("FadeMask").GetComponent<Image>();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        switch(f)
        {
            case FadeState.FadeOut:
                float fade = (Time.time - FadeTime) / FadeDuration;
                Debug.Log(fade);
                if(fade > 1) {
                    Transparency = 1;
                    SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
                    f = FadeState.FadeIn;
                    FadeTime = Time.time;
                }
                else
                {
                    Transparency = fade;
                }
                
                break;

            case FadeState.FadeIn:
                float fade2 = (Time.time - FadeTime) / FadeDuration;
                if(fade2 > 1) {
                    Transparency = 0;
                    f = FadeState.Idle;
                }
                else
                {
                    Transparency = 1 - fade2;
                }
                break;
            default:
                FadeTime = Time.time;
                Transparency = 0;
                break;
        }

        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, Transparency);
        
    }

    public void Quit()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        Application.Quit();
    }
    
    public void Play()
    {
        f = FadeState.FadeOut;
    }
}
