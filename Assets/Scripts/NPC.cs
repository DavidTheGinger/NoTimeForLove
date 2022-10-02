using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [SerializeField] private BubbleManager bubbleManager;
    [SerializeField] private List<Sprite> talkingSprites;
    [SerializeField] private Sprite neutralNpcSprite;
    [SerializeField] private Sprite happyNpcSprite;
    [SerializeField] private Sprite negativeNpcSprite;
    [SerializeField] private Sprite neutralEmojiSprite;
    [SerializeField] private Sprite happyEmojiSprite;
    [SerializeField] private Sprite negativeEmojiSprite;
    [SerializeField] private SpriteRenderer silhouette;
    [SerializeField] private float silhouette_removal_speed = 1;
    [SerializeField] private float npc_movespeed = 5;
    [SerializeField] private float talkingDelay = .3f;
    [SerializeField] private GameObject spawn_point;
    [SerializeField] private GameObject chat_target;
    [SerializeField] private GameObject end_target;


    [SerializeField] private SpriteRenderer image_displayed;
    GameObject target;
    private float journeyLength;
    private float startTime;
    private bool reacting = false;

    [SerializeField] private bool tutorial = false;

    public enum NpcState { entering, talking, leaving, waiting}
    NpcState npcState = NpcState.entering;

    enum Reactions { neutral, happy, negative}
    Reactions reaction = Reactions.neutral;

    void Awake()
    {
        transform.position = spawn_point.transform.position;
        setTarget(chat_target);
    }

    private void FixedUpdate()
    {
        switch (npcState)
        {
            case NpcState.entering:
                moveTowardsTarget();
                if (Vector3.Distance(transform.position, target.transform.position) < .1f)
                {
                    BeginTalking();
                }
                break;
            case NpcState.talking:
                if (tutorial)
                {

                }
                break;
            case NpcState.waiting:
                if (tutorial)
                {
                    if (reacting)
                    {
                        tutorial = false;
                        StartCoroutine(TutoiralReactTimer());
                    }
                }
                
                break;
            case NpcState.leaving:
                moveTowardsTarget();
                if (Vector3.Distance(transform.position, target.transform.position) < .1f)
                {
                    CleanupNPC();
                }
                break;
            default:
                break;
        }

    }

    public void setTarget(GameObject target_temp)
    {
        startTime = Time.time;
        target = target_temp;
        journeyLength = Vector3.Distance(transform.position, target.transform.position);
        //Debug.Log("Set target to: " + target.name);
    }

    public void BeginTalking()
    {
        npcState = NpcState.talking;
        //Debug.Log("NPC: begin talking");
        bubbleManager.beginTalking();
        StartCoroutine(RemoveSilhouette());
        StartCoroutine(TalkTimer());
        StartCoroutine(TalkSpriteUpdater());
        
    }

    public void Leave()
    {
        npcState = NpcState.leaving;
        setTarget(end_target);
    }

    public void Wait()
    {
        npcState = NpcState.waiting;
    }

    public void React(char char_reaction)
    {
        switch (char_reaction)
        {
            case 'h':
                reaction = Reactions.happy;
                ActivateReactionParticles(happyEmojiSprite);
                image_displayed.sprite = happyNpcSprite;
                break;
            case 'n':
                reaction = Reactions.negative;
                ActivateReactionParticles(negativeEmojiSprite);
                image_displayed.sprite = negativeNpcSprite;
                break;
            default:
                reaction = Reactions.neutral;
                ActivateReactionParticles(neutralEmojiSprite);
                image_displayed.sprite = neutralNpcSprite;
                break;

        }
        reacting = true;
    }

    private void ActivateReactionParticles(Sprite particle_sprite)
    {

    }

    private void moveTowardsTarget()
    {
        //Debug.Log("MOVING TOWARDS: " + target.name);

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * npc_movespeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(transform.position, target.transform.position, fractionOfJourney);

        // Debug.Log("distCovered: " + distCovered);
        // Debug.Log("fractionOfJourney: " + fractionOfJourney);
        // Debug.Log("JourneyLength: " + journeyLength);
    }

    IEnumerator RemoveSilhouette()
    {

        while(silhouette.color.a != 0)
        {
            float silhouette_a = silhouette.color.a - Time.deltaTime * silhouette_removal_speed;
            if (silhouette_a < 0)
            {
                silhouette_a = 0;
            }
            silhouette.color = new Color(silhouette.color.r, silhouette.color.g, silhouette.color.b, silhouette_a);
            yield return null;
        }
        
    }

    IEnumerator TalkTimer()
    {
        yield return new WaitForSeconds(10);
        if (!tutorial)
        {
            Leave();
        }
        else
        {
            Wait();
        }
    }

    IEnumerator TutoiralReactTimer()
    {

        yield return new WaitForSeconds(2);
        Leave();
    }

    IEnumerator TalkSpriteUpdater()
    {
        int sprite_index = 0;
        while(npcState == NpcState.talking && !reacting)
        {
            Debug.Log("sprite index: " + sprite_index);
            sprite_index = sprite_index < talkingSprites.Count - 1 ? sprite_index + 1 : 0;
            image_displayed.sprite = talkingSprites[sprite_index];
            yield return new WaitForSeconds(talkingDelay);
        }
        if (!reacting)
        {
            image_displayed.sprite = talkingSprites[0];
        }

    }


    private void CleanupNPC()
    {
        bubbleManager.CleanupBubbles();
        Destroy(gameObject);
    }


}
