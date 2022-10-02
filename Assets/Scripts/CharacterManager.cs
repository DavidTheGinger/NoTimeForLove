using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> npcPrefabs;

    [SerializeField] private NPC currentNpcScript;
    [SerializeField] private float backgroundSwapTime = 1f;
    [SerializeField] private List<SpriteRenderer> bgSpriteRenderers;
    private AudioManager audioManager;



    // Start is called before the first frame update
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        MoveOnFromNpc();

    }

    public void MoveOnFromNpc()
    {
        if(npcPrefabs.Count > 0)
        {
            currentNpcScript = Instantiate(npcPrefabs[0]).GetComponentInChildren<NPC>();
            npcPrefabs.RemoveAt(0);
            SwapBackgrounds();
            audioManager.UpdateMusic(currentNpcScript.tracks);
        }
        else
        {
            TransitionToEndScreen();
        }
    }

    private void SwapBackgrounds()
    {

        if(bgSpriteRenderers[0].color.a < 175)
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

    }

    IEnumerator FadeBackground(SpriteRenderer spriteIn, SpriteRenderer spriteOut)
    {
        spriteIn.sprite = currentNpcScript.personalBackground;
        while (spriteIn.color.a < 255)
        {
            float spriteIn_a = spriteIn.color.a + Time.deltaTime * backgroundSwapTime;
            if (spriteIn_a > 255)
            {
                spriteIn_a = 255;
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
