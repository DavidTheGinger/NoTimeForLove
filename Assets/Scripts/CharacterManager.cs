using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> npcPrefabs;
    [SerializeField] private GameObject tutorialNpcPrefab;

    [SerializeField] private NPC currentNpcScript;
    private float backgroundSwapTime = 1f;
    [SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
    private AudioManager audioManager;
    private ScoreKeeper scoreKeeper;

    public List<string> npcsNeutral = new List<string>();
    public List<string> npcsLove = new List<string>();
    public List<string> npcsHate = new List<string>();

    private MenuScript menu;

    [SerializeField] public bool[] tracks;

    bool preFirstNpc = true;

    // Start is called before the first frame update
    void Awake()
    {
        menu = GameObject.FindGameObjectWithTag("SceneTransitionManager").GetComponent<MenuScript>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        scoreKeeper = GameObject.FindGameObjectWithTag("ScoreKeeper").GetComponent<ScoreKeeper>();
        randomizeList(npcPrefabs);
        npcPrefabs.Insert(0, tutorialNpcPrefab);
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
            UpdateFinalMusic();
            audioManager.UpdateMusic(tracks);
            TransitionToEndScreen();
        }
    }

    private void ObserveNpcReaction()
    {
        string name = ParseName(currentNpcScript.gameObject.transform.parent.name);
        Debug.Log("Recorded " + name + "'s reaction");

        switch (currentNpcScript.reaction)
        {
            case NPC.Reactions.neutral:
                if(currentNpcScript.minReactionForLove == NPC.Reactions.neutral || currentNpcScript.minReactionForLove == NPC.Reactions.negative)
                {
                    npcsLove.Add(name);
                }
                else
                {
                    npcsNeutral.Add(name);
                }
                break;
            case NPC.Reactions.happy:
                npcsLove.Add(name);
                break;
            case NPC.Reactions.negative:
                if (currentNpcScript.minReactionForLove == NPC.Reactions.negative)
                {
                    npcsLove.Add(name);
                }
                else
                {
                    npcsHate.Add(name);
                }
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

    private void UpdateFinalMusic()
    {
        switch (npcsLove.Count)
        {
            case 0:
                tracks[5] = true;
                break;
            case 1:
                tracks[11] = true;
                break;
            case 2:
                tracks[9] = true;
                break;
            case 3:
                tracks[9] = true;
                tracks[10] = true;
                break;
            case 4:
                tracks[9] = true;
                tracks[10] = true;
                tracks[1] = true;
                break;
            case 5:
                tracks[9] = true;
                tracks[10] = true;
                tracks[1] = true;
                tracks[4] = true;
                break;
            case 6:
                tracks[9] = true;
                tracks[10] = true;
                tracks[1] = true;
                tracks[4] = true;
                tracks[8] = true;
                break;
            case 7:
                tracks[9] = true;
                tracks[10] = true;
                tracks[1] = true;
                tracks[4] = true;
                tracks[8] = true;
                tracks[0] = true;
                break;
            default:
                tracks[9] = true;
                tracks[10] = true;
                tracks[1] = true;
                tracks[4] = true;
                tracks[8] = true;
                tracks[0] = true;
                break;
        }
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
        

        scoreKeeper.npcsHate = npcsHate;
        scoreKeeper.npcsLove = npcsLove;
        scoreKeeper.npcsNeutral = npcsNeutral;
        menu.ChangeScene();

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

    private void randomizeList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

}
