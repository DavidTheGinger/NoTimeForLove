using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [SerializeField] private BubbleManager bubbleManager;
    private BubbleBehavior bubbleBehavior;
    [SerializeField] private List<Sprite> talkingSprites;
    [SerializeField] private Sprite neutralNpcSprite;
    [SerializeField] private Sprite happyNpcSprite;
    [SerializeField] private Sprite negativeNpcSprite;
    [SerializeField] private Sprite neutralEmojiParticle;
    [SerializeField] private Sprite happyEmojiParticle;
    [SerializeField] private Sprite negativeEmojiParticle;
    [SerializeField] private SpriteRenderer silhouette;
    public Sprite personalBackground;
    [SerializeField] private float silhouette_removal_speed = 1;
    [SerializeField] private float npc_movespeed = 5;
    [SerializeField] private float talkingDelay = .3f;
    [SerializeField] private float pitch_min = 1;
    [SerializeField] private float pitch_max = 1;
    [SerializeField] private GameObject spawn_point;
    [SerializeField] private GameObject chat_target;
    [SerializeField] private GameObject end_target;

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private SpriteRenderer image_displayed;
    GameObject target;
    private float journeyLength;
    private float startTime;
    private bool reacting = false;
    [SerializeField] private float avgWordTime = .2f;
    private float wordTimeVarianceMult = 1.2f;

    [SerializeField] private bool tutorial = false;


    public enum NpcState { entering, talking, leaving, waiting}
    public NpcState npcState = NpcState.entering;

    public enum Reactions { neutral, happy, negative}
    public Reactions reaction = Reactions.neutral;
    public Reactions minReactionForLove = Reactions.happy;

    [SerializeField] public bool[] tracks;
    [SerializeField] private AudioClip[] talkSounds;
    [SerializeField] private AudioClip wooshSound;
    [SerializeField] private AudioClip wooshSoundReversed;
    [SerializeField] private AudioSource source;
    [SerializeField] private float volumeBase = 1;
    [SerializeField] private float volumeFadeSpeed = 10;

    private float nextTalkTime = 0;
    private int sprite_index = 0;

    private float clockNoonOffset = -10f;
    private GameObject timerHand;

    private CharacterManager characterManager;

    private GameObject mockSceneParent;

    private IEnumerator clockHandCoroutine;

    private void Awake()
    {
        characterManager = GameObject.FindGameObjectWithTag("CharacterManager").GetComponent<CharacterManager>();
        mockSceneParent = GameObject.FindGameObjectWithTag("MockSceneParent");
        timerHand = GameObject.FindGameObjectWithTag("TimerHand");
        clockHandCoroutine = MoveTimerHands();
        if (tutorial)
        {
            bubbleBehavior = GameObject.FindGameObjectWithTag("LongestText").GetComponent<BubbleBehavior>();
        }

        source.pitch = 1;
        source.PlayOneShot(wooshSoundReversed, volumeBase);
        transform.parent.parent = mockSceneParent.transform;
        transform.SetAsFirstSibling();
    }

    void Start()
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
                if (!reacting)
                {
                    Talk();
                }
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
                    else
                    {
                        Talk();
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
        bubbleManager.initializeData();
        bubbleManager.beginTalking();
        StartCoroutine(RemoveSilhouette());
        StartCoroutine(TalkTimer());
        StartCoroutine(TalkSpriteUpdater());
        
    }

    public void Leave()
    {
        source.PlayOneShot(wooshSound, volumeBase);
        npcState = NpcState.leaving;
        setTarget(end_target);
    }

    public void Wait()
    {
        npcState = NpcState.waiting;
    }

    public void React(char char_reaction)
    {
        Debug.Log("I, the "+ gameObject.transform.parent.name +", am about to react to '" + char_reaction + "' reaction input");

        source.pitch = 1;
        switch (char_reaction)
        {   
            case 'h':
                //Debug.Log("I, the NPC, have reacted happily");
                reaction = Reactions.happy;
                ActivateReactionParticles(happyEmojiParticle);
                image_displayed.sprite = happyNpcSprite;
                break;
            case 'n':
                //Debug.Log("I, the NPC, have reacted negativly");
                reaction = Reactions.negative;
                ActivateReactionParticles(negativeEmojiParticle);
                image_displayed.sprite = negativeNpcSprite;
                break;
            default:
                //Debug.Log("I, the NPC, have reacted neutrally");
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
        StartCoroutine(clockHandCoroutine);
        yield return new WaitForSeconds(10);
        StopCoroutine(clockHandCoroutine);
        timerHand.transform.rotation = Quaternion.Euler(0f, 0f, clockNoonOffset);
        if (!tutorial)
        {
            Leave();
        }
        else
        {
            while (!bubbleBehavior.finisehdDialogue && !reacting)
            {
                yield return null;
            }
            Wait();
        }
    }

    IEnumerator MoveTimerHands()
    {

        while (true)
        {
            timerHand.transform.rotation = Quaternion.Euler(0f, 0f, timerHand.transform.rotation.eulerAngles.z - 36 * Time.deltaTime);
            yield return null;
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
            // sprite_index = sprite_index < talkingSprites.Count - 1 ? sprite_index + 1 : 0;
            //image_displayed.sprite = talkingSprites[sprite_index];
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
        characterManager.MoveOnFromNpc();
        Destroy(transform.parent.gameObject);
    }

    private void Talk()
    {
        if(nextTalkTime < Time.time)
        {
            sprite_index = sprite_index < talkingSprites.Count - 1 ? sprite_index + 1 : 0;
            image_displayed.sprite = talkingSprites[sprite_index];

            int index = Random.Range(0, talkSounds.Length);
            source.pitch = Random.Range(pitch_min, pitch_max);
            AudioClip clip = talkSounds[index];
            //source.clip = clip;
            /* play sounds based on when sounds finish
            nextTalkTime = Time.time + source.clip.length / source.pitch;
            source.PlayOneShot(talkSounds[index]);
            */
            //play sounds based on random interval, with interruptions
            //float delay = Mathf.Min(source.clip.length / source.pitch, Random.Range(avgWordTime - (avgWordTime - avgWordTime * wordTimeVarianceMult), avgWordTime * wordTimeVarianceMult));
            float delay = Random.Range(avgWordTime - (avgWordTime - avgWordTime * wordTimeVarianceMult), avgWordTime * wordTimeVarianceMult);
            nextTalkTime = Time.time + delay;
            //if(Random.Range(0, 2) == 1) { source.pitch *= -1; }
            source.PlayOneShot(talkSounds[index], volumeBase);
            //coroutine to QUICKLY fade out each audio clip to avoid audio popping
            //StartCoroutine(PlayVoice(index, delay));
            //End of interruptions on random interval
        }
    }

    IEnumerator PlayVoice(int index, float fadeoutTime)
    {
        while(source.volume > 0)
        {
            source.volume -= Time.deltaTime * volumeFadeSpeed;
            if (source.volume < 0)
            {
                source.volume = 0;
            }
            yield return null;
        }
        source.Stop();
        source.volume = volumeBase;
        source.clip = talkSounds[index];
        source.Play();
    }

}
