using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class endingStatsScript : MonoBehaviour
{

    private ScoreKeeper score;
    private TextMeshProUGUI bubble_dialogue;
    public enum Reactions { neutral, happy, negative }
    public Reactions reaction = Reactions.neutral;

    // Start is called before the first frame update
    void Awake()
    {
        score = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>();
        bubble_dialogue = GetComponent<TextMeshProUGUI>();
        switch (reaction)
        {
            case Reactions.neutral:
                SetText(score.npcsNeutral);
                break;
            case Reactions.happy:
                SetText(score.npcsLove);
                break;
            case Reactions.negative:
                SetText(score.npcsHate);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void SetText(List<string> names)
    {
        foreach (string name in names)
        {
            bubble_dialogue.text += name + "\n";
        }
    }
}
