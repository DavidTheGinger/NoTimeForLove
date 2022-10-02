using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleManager : MonoBehaviour
{
    [SerializeField] protected float wpm = 200;
    [SerializeField] protected float avg_word_len = 4.7f;
    [SerializeField] protected float bubble_speed = 3f;
    protected float TextDelaySeconds = 0f;
    protected float startTime = 0f;
    protected Vector3 startPositionCamSpace;

    [SerializeField] protected List<Sprite> bubble_sprites;
    [SerializeField] protected Camera cam;
    [SerializeField] private List<BubbleBehavior> bubble_behaviors;
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private GameObject mouth_obj;
    public TMP_FontAsset font;

    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void initializeData()
    {
        Debug.Log(this.gameObject.name + " is initializing (in bubble manager)");
        TextDelaySeconds = 1 / (wpm / 60f) / avg_word_len;
        startTime = Time.time;
        startPositionCamSpace = cam.WorldToScreenPoint(mouth_obj.transform.position);
        randomizeList(targets);
        foreach (BubbleBehavior b in bubble_behaviors)
        {
            b.InitializeData();
            b.TextDelaySeconds = TextDelaySeconds;
        }
    }

    public void beginTalking()
    {

        setInitialBubblePositions();
        foreach (BubbleBehavior b in bubble_behaviors)
        {
            b.talking = true;
            b.source.pitch = Random.Range(0.85f, 1.4f);
            b.source.PlayDelayed(Random.Range(0f, 0.2f));
        }
    }

    private void setInitialBubblePositions()
    {
        Debug.Log("BubbleManager: setInitialBubblePositions");
        int i = 0;
        foreach (BubbleBehavior b in bubble_behaviors)
        {
            b.gameObject.transform.position = cam.WorldToScreenPoint(mouth_obj.transform.position);
            b.setTarget(targets[i]);
            i = i < (targets.Count - 1) ? i + 1 : 0;

            //set the bubble's font while we're at it
            if(font != null)
            {
                b.bubble_dialogue.font = font;
            }
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

    public void StopTalking()
    {
        foreach (BubbleBehavior b in bubble_behaviors)
        {
            b.Shuddup();
        }
    }

    public void CleanupBubbles()
    {
        foreach (BubbleBehavior b in bubble_behaviors)
        {
            Destroy(b.gameObject);
        }
        Destroy(gameObject);
    }
}
