using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BubbleBehavior : NPC
{

    [SerializeField] private string dialogue_string;

    [SerializeField] private TextMeshProUGUI bubble_dialogue;
    [SerializeField] private Image image_displayed;
    [SerializeField] private int sprite_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        image_displayed.alphaHitTestMinimumThreshold = 0.5f;
        TextDelaySeconds = 1/(wpm / 60f)/avg_word_len;
        dialogue_string = bubble_dialogue.text;
        bubble_dialogue.text = "";
        sprite_index = Random.Range(0, bubble_sprites.Count - 1);


        DisplayText();
        StartCoroutine(SwitchSprite());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText()
    {
        StartCoroutine(TypeDialogue(dialogue_string));
    }

    IEnumerator TypeDialogue (string dialogue)
    {
        bubble_dialogue.text = "";
        foreach(char letter in dialogue.ToCharArray())
        {
            
            bubble_dialogue.text += letter;
            yield return new WaitForSeconds(TextDelaySeconds);
        }
    }

    IEnumerator SwitchSprite()
    {
        while (true)
        {
            sprite_index = sprite_index < bubble_sprites.Count - 1 ? sprite_index + 1 : 0;
            image_displayed.sprite = bubble_sprites[sprite_index];
            yield return new WaitForSeconds(SpriteDelay * TextDelaySeconds);
        }
        

    }
}
