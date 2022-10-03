using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> npcPrefabs;

    [SerializeField] private NPC currentNpcScript;
    private float backgroundSwapTime = 1f;
    [SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
    private AudioManager audioManager;
    private ScoreKeeper scoreKeeper;

    public List<string> npcsNeutral = new List<string>();
    public List<string> npcsLove = new List<string>();
    public List<string> npcsHate = new List<string>();

    bool preFirstNpc = true;

    // Start is called before the first frame update
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        scoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>();
        MoveOnFromNpc();

    }

    public void MoveOnFromNpc()
    {
        if(npcPrefabs.Count > 0)
        {
            if (!preFirstNpc)
            {
                ObserveNpcReaction();
            }
            currentNpcScript = Instantiate(npcPrefabs[0]).GetComponentInChildren<NPC>();
            npcPrefabs.RemoveAt(0);
            SwapBackgrounds();
            audioManager.UpdateMusic(currentNpcScript.tracks);
            preFirstNpc = false;
        }
        else
        {
            ObserveNpcReaction();
            TransitionToEndScreen();
        }
    }

    private void ObserveNpcReaction()
    {
        string name = ParseName(currentNpcScript.gameObject.transform.parent.name);

        switch (currentNpcScript.reaction)
        {
            case NPC.Reactions.neutral:
                npcsNeutral.Add(name);
                break;
            case NPC.Reactions.happy:
                npcsLove.Add(name);
                break;
            case NPC.Reactions.negative:
                npcsHate.Add(name);
                break;
        }
    }

    private string ParseName(string unParsedName)
    {
        string name = "";
        foreach (string word in unParsedName.Split(' '))
        {
            if (!word.Contains("("))
            {
                name += word + " ";
            }
        }
        return name;
    }

    private void SwapBackgrounds()
    {

        if(bgSpriteRenderers[0].color.a < .5f)
        {
            StartCoroutine(FadeBackground(bgSpriteRenderers[0], bgSpriteRenderers[1]));
        }
        else
        {
            StartCoroutine(FadeBackground(bgSpriteRenderers[1], bgSpriteRenderers[0]));
        }
        
    }
    private void TransitionToEndScreen()
    {
        /*
        foreach(string name in npcsNeutral)
        {
            Debug.Log("Neutral towards you: " + name);
        }
        foreach (string name in npcsLove)
        {
            Debug.Log("Love towards you: " + name);
        }
        foreach (string name in npcsHate)
        {
            Debug.Log("Hate towards you: " + name);
        }
        */

        scoreKeeper.npcsHate = npcsHate;
        scoreKeeper.npcsLove = npcsLove;
        scoreKeeper.npcsNeutral = npcsNeutral;
    }

    IEnumerator FadeBackground(SpriteRenderer spriteIn, SpriteRenderer spriteOut)
    {
        spriteIn.sprite = currentNpcScript.personalBackground;
        while (spriteIn.color.a < 1)
        {
            float spriteIn_a = spriteIn.color.a + Time.deltaTime * backgroundSwapTime;
            if (spriteIn_a > 1)
            {
                spriteIn_a = 1;
            }
            spriteIn.color = new Color(spriteIn.color.r, spriteIn.color.g, spriteIn.color.b, spriteIn_a);
            yield return null;
        }

        while (spriteOut.color.a != 0)
        {
            float spriteOut_a = spriteOut.color.a - Time.deltaTime * backgroundSwapTime;
            if (spriteOut_a < 0)
            {
                spriteOut_a = 0;
            }
            spriteOut.color = new Color(spriteOut.color.r, spriteOut.color.g, spriteOut.color.b, spriteOut_a);
            yield return null;
        }

    }

}
