using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuBubbles : MonoBehaviour
{



    [SerializeField] public TextMeshProUGUI bubbleText;
    [SerializeField] private Image imageDisplayed;
    [SerializeField] private float spriteDelay = .33f;
    [SerializeField] private int sprite_index = 0;
    [SerializeField] protected List<Sprite> bubble_sprites;
    // Start is called before the first frame update
    void Start()
    {
        spriteDelay = Random.Range(spriteDelay - .15f, spriteDelay + .05f);
        StartCoroutine(SwitchSprite());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SwitchSprite()
    {
        while (true)
        {
            sprite_index = sprite_index < bubble_sprites.Count - 1 ? sprite_index + 1 : 0;
            imageDisplayed.sprite = bubble_sprites[sprite_index];
            yield return new WaitForSeconds(spriteDelay);
        }


    }
}
