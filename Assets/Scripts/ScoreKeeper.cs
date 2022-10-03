using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public List<string> npcsNeutral = new List<string>();
    public List<string> npcsLove = new List<string>();
    public List<string> npcsHate = new List<string>();
    private static ScoreKeeper INSTANCE;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (INSTANCE == null)
        {
            INSTANCE = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
