using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSilhouettes : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private SpriteRenderer silhouetteLeaving;
    [SerializeField] private SpriteRenderer silhouetteEntering;
    [SerializeField] private GameObject spawn_point;
    [SerializeField] private GameObject chat_target;
    [SerializeField] private GameObject end_target;
    public MenuScript menu;

    private float startTime;
    private float npc_movespeed = 5f;
    private float journeyLengthLeaving;
    private float journeyLengthEntering;
    private int spriteIndex;

    void Start()
    {
        menu.gameObject.SetActive(true);

        journeyLengthLeaving = Vector3.Distance(chat_target.transform.position, end_target.transform.position);
        journeyLengthEntering = Vector3.Distance(spawn_point.transform.position, chat_target.transform.position);
        silhouetteEntering.transform.position = spawn_point.transform.position;
        silhouetteLeaving.transform.position = chat_target.transform.position;
        randomizeList(sprites);
        silhouetteEntering.sprite = sprites[0];
        silhouetteLeaving.sprite = sprites[1];
        StartCoroutine(SlideSilhouettes());
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget(silhouetteLeaving.gameObject.transform, end_target.transform, journeyLengthLeaving);
        MoveTowardsTarget(silhouetteEntering.gameObject.transform, chat_target.transform, journeyLengthEntering);
    }

    private void MoveTowardsTarget(Transform sprite, Transform target, float journeyLength)
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * npc_movespeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        sprite.position = Vector3.Lerp(sprite.position, target.transform.position, fractionOfJourney);
    }

    private Sprite ChooseRandomSprite()
    {
        return sprites[Random.Range(0, sprites.Count)];
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

    IEnumerator SlideSilhouettes()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            startTime = Time.time;
            silhouetteLeaving.sprite = silhouetteEntering.sprite;
            silhouetteLeaving.gameObject.transform.position = silhouetteEntering.gameObject.transform.position;
            silhouetteEntering.gameObject.transform.position = spawn_point.transform.position;
            spriteIndex = spriteIndex < sprites.Count - 1 ? spriteIndex + 1 : 0;
            silhouetteEntering.sprite = ChooseRandomSprite();
        }
    }

    

}
