using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public GameObject emojiHappyPrefab;
    public GameObject emojiAngryPrefab;
    public GameObject emojiLustPrefab;
    public GameObject emojiSadPrefab;
    public enum EmojiHeld {none, lust, happy, sad, angry }
    public EmojiHeld emojiHeld = EmojiHeld.none;

    [SerializeField] private float hand_speed = 1;

    //to be set by the overall game manager
    public GameObject currentNpc;
    public Camera cam;

    private GameObject emojiSticker;
    private float journeyLength = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        journeyLength = Vector3.Distance(transform.position, cam.ScreenToWorldPoint(Input.mousePosition)); 
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - 1) * hand_speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(transform.position, cam.ScreenToWorldPoint(Input.mousePosition), fractionOfJourney);
    }

    private void PickupEmoji(GameObject emoji)
    {
        emojiSticker = Instantiate(emoji);
        emojiSticker.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
        emojiSticker.transform.SetParent(gameObject.transform, true);
    }

    public void PlaceEmoji()
    {
        emojiSticker.transform.SetParent(currentNpc.transform, true);
        EmptyHand();
    }

    public void HoldHappy()
    {
        emojiHeld = EmojiHeld.happy;
        PickupEmoji(emojiHappyPrefab);
    }
    public void HoldLust()
    {
        emojiHeld = EmojiHeld.lust;
        PickupEmoji(emojiLustPrefab);
    }
    public void HoldSad()
    {
        emojiHeld = EmojiHeld.sad;
        PickupEmoji(emojiSadPrefab);
    }
    public void HoldAngry()
    {
        emojiHeld = EmojiHeld.angry;
        PickupEmoji(emojiAngryPrefab);
    }

    public void EmptyHand()
    {
        emojiHeld = EmojiHeld.none;
    }
}
