using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    [SerializeField] protected float wpm = 200;
    [SerializeField] protected float avg_word_len = 4.7f;
    [SerializeField] protected float target_pull_force = 1f;
    [SerializeField] protected float bubble_speed = 3f;
    [SerializeField] protected float SpriteDelay = 1f;
    protected float TextDelaySeconds = 0;
    protected float startTime = 0f;
    protected Vector3 startPositionCamSpace;

    [SerializeField] protected List<Sprite> bubble_sprites;
    [SerializeField] protected Camera cam;
    [SerializeField] private List<BubbleBehavior> bubble_behaviors;
    // Targets for the speech bubbles to gravitate towards
    // should exist to the left and right of the npc.
    // There should be one target per speech bubble.
    [SerializeField] private List<GameObject> targets;
    [SerializeField] protected GameObject mouth_obj;



    void Awake()
    {
        TextDelaySeconds = 1 / (wpm / 60f) / avg_word_len;
        initializeData();
        beginTalking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initializeData()
    {
        startTime = Time.time;
        startPositionCamSpace = cam.WorldToScreenPoint(mouth_obj.transform.position);
        randomizeList(targets);
    }

    private void beginTalking()
    {
        setInitialBubblePositions();
    }


    private void setInitialBubblePositions()
    {
        int i = 0;
        foreach (BubbleBehavior b in bubble_behaviors)
        {
            b.gameObject.transform.position = cam.WorldToScreenPoint(mouth_obj.transform.position);
            b.setTarget(targets[i]);
            i = i < bubble_sprites.Count - 1 ? i + 1 : 0;
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
