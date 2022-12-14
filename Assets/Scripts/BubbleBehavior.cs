using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BubbleBehavior : BubbleManager
{

    [SerializeField] private string dialogue_string;
    [SerializeField] public TextMeshProUGUI bubble_dialogue;
    [SerializeField] private Button myButton;
    [SerializeField] private Image image_displayed;
    [SerializeField] private int sprite_index = 0;
    [SerializeField] private Collider2D text_collider;
    [SerializeField] private GameObject target;

    [SerializeField] private NPC myNpc;
    [SerializeField] private HandScript handScript;

    // - is neutral, h is happy, n is negative
    [SerializeField] private char happyReaction = '-';
    [SerializeField] private char lustReaction = '-';
    [SerializeField] private char sadReaction = '-';
    [SerializeField] private char angryReaction = '-';

    

    [SerializeField] private float spriteDelay =13;
    [SerializeField] private float bubbleGrowthSpeed = 10; 
    private float journeyLength;
    private float bubbleSize = 0f;

    public bool talking = false;
    //FOR TUTORIAL ONLY
    public bool finisehdDialogue = false;

    private IEnumerator typingCoroutine;

    [SerializeField] public AudioSource source;


    // Start is called before the first frame update
    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myButton = GetComponent<Button>();
        handScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HandScript>();
        source = GetComponent<AudioSource>();
    }

    public void InitializeData()
    {
        //Debug.Log(gameObject.name + " is initializing 1%");
        image_displayed.alphaHitTestMinimumThreshold = 0.5f;
        dialogue_string = bubble_dialogue.text;
        bubble_dialogue.text = "";
        //Debug.Log(gameObject.name + " is initializing 50%");
        sprite_index = Random.Range(0, bubble_sprites.Count - 1);
        spriteDelay = Random.Range(spriteDelay - 2, spriteDelay + 2);
        //Debug.Log(gameObject.name + " is initializing 100%");
        DisplayText();
        //Debug.Log(gameObject.name + "Theoretically called Display text");
        StartCoroutine(SwitchSprite());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (talking)
        {
            moveTowardsTarget();
            if (bubbleSize < 1)
            {
                growBubbles();
            }
        }
        
    }

    public void DisplayText()
    {
        //Debug.Log("Diplaying Text");
        typingCoroutine = TypeDialogue(dialogue_string);
        StartCoroutine(typingCoroutine);
    }

    public void setTarget(GameObject target_temp)
    {
        target = target_temp;
        journeyLength = Vector3.Distance(transform.position, cam.WorldToScreenPoint(target.transform.position));
    }

    private void moveTowardsTarget()
    {
        //Vector3 forceDirection = transform.position - cam.WorldToScreenPoint(target.transform.position);
        //text_collider.attachedRigidbody.AddForce(forceDirection * target_pull_force * Time.fixedDeltaTime);

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * bubble_speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(transform.position, cam.WorldToScreenPoint(target.transform.position), fractionOfJourney);
        
        // Debug.Log("distCovered: " + distCovered);
        // Debug.Log("fractionOfJourney: " + fractionOfJourney);
        // Debug.Log("JourneyLength: " + journeyLength);
    }
    private void growBubbles()
    {
        bubbleSize = bubbleSize + Time.deltaTime * bubbleGrowthSpeed;
        if (bubbleSize > 1)
        {
            bubbleSize = 1;
        }
        transform.localScale = new Vector3(bubbleSize,bubbleSize,bubbleSize);
    }

    public void ReacToReaction()
    {
        //Debug.Log("A textbox has been chosen while '" + handScript.emojiHeld + "' was being held");
        switch (handScript.emojiHeld)
        {
            case HandScript.EmojiHeld.lust:
                myNpc.React(lustReaction);
                break;
            case HandScript.EmojiHeld.happy:
                myNpc.React(happyReaction);
                break;
            case HandScript.EmojiHeld.sad:
                myNpc.React(sadReaction);
                break;
            case HandScript.EmojiHeld.angry:
                myNpc.React(angryReaction);
                break;
            default:
                break;
        }
        handScript.PlaceEmoji(transform);
    }

    public void Shuddup()
    {
        StopCoroutine(typingCoroutine);
        myButton.interactable = false;
    }

    IEnumerator TypeDialogue (string dialogue)
    {
        //Debug.Log("In the Type Dialogue coroutine");
        bubble_dialogue.text = "";
        foreach(string word in dialogue.Split(' '))
        {
            
            bubble_dialogue.text += word + " ";
            yield return new WaitForSeconds(TextDelaySeconds * word.Length);
        }
        finisehdDialogue = true;
    }

    IEnumerator SwitchSprite()
    {
        while (true)
        {
            sprite_index = sprite_index < bubble_sprites.Count - 1 ? sprite_index + 1 : 0;
            image_displayed.sprite = bubble_sprites[sprite_index];
            yield return new WaitForSeconds(spriteDelay * TextDelaySeconds);
        }
        

    }


}
