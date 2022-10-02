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
    [SerializeField] private Sprite neutralEmojiParticle;
    [SerializeField] private Sprite happyEmojiParticle;
    [SerializeField] private Sprite negativeEmojiParticle;
    [SerializeField] private SpriteRenderer silhouette;
    [SerializeField] private float silhouette_removal_speed = 1;
    [SerializeField] private float npc_movespeed = 5;
    [SerializeField] private float talkingDelay = .3f;
    [SerializeField] private GameObject spawn_point;
    [SerializeField] private GameObject chat_target;
    [SerializeField] private GameObject end_target;

    [SerializeField] private ParticleSystem particles;
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

    [SerializeField] private bool[] tracks;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip[] talkSounds;
    [SerializeField] private AudioSource source;

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
                Talk();
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
        Debug.Log("I, the NPC, am about to react to '" + char_reaction + "' reaction input");
        switch (char_reaction)
        {
            
            case 'h':
                Debug.Log("I, the NPC, have reacted happily");
                reaction = Reactions.happy;
                ActivateReactionParticles(happyEmojiParticle);
                image_displayed.sprite = happyNpcSprite;
                break;
            case 'n':
                Debug.Log("I, the NPC, have reacted negativly");
                reaction = Reactions.negative;
                ActivateReactionParticles(negativeEmojiParticle);
                image_displayed.sprite = negativeNpcSprite;
                break;
            default:
                Debug.Log("I, the NPC, have reacted neutrally");
                reaction = Reactions.neutral;
                ActivateReactionParticles(neutralEmojiParticle);
                image_displayed.sprite = neutralNpcSprite;
                break;

        }
        reacting = true;
    }

    private void ActivateReactionParticles(Sprite particle)
    {
        particles.Clear();
        particles.textureSheetAnimation.SetSprite(0, particle);
        particles.Play();
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
        PlayMusic();
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
        while((npcState == NpcState.talking || npcState == NpcState.leaving) && !reacting)
        {
            //Debug.Log("sprite index: " + sprite_index);
            sprite_index = sprite_index < talkingSprites.Count - 1 ? sprite_index + 1 : 0;
            image_displayed.sprite = talkingSprites[sprite_index];
            yield return new WaitForSeconds(talkingDelay);
        }
        if (!reacting)
        {
            image_displayed.sprite = talkingSprites[0];
        }
        else
        {
            bubbleManager.StopTalking();
        }

    }


    private void CleanupNPC()
    {
        bubbleManager.CleanupBubbles();
        Destroy(gameObject);
    }

    private void PlayMusic()
    {
        for(int i = 0; i < tracks.Length; i++)
        {
            if(tracks[i]) { source.PlayOneShot(clips[i]); }
        }
    }

    private void Talk()
    {
        int index = Random.Range(0, talkSounds.Length);
        
    }

}
