using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [SerializeField] protected float wpm = 200;
    [SerializeField] protected float avg_word_len = 4.7f;
    protected float TextDelaySeconds = 0;
    [SerializeField] protected float SpriteDelay = 1f;
    [SerializeField] protected List<Sprite> bubble_sprites;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
