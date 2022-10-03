using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public GameObject emojiHappyPrefab;
    public GameObject emojiAngryPrefab;
    public GameObject emojiLustPrefab;
    public GameObject emojiSadPrefab;
    public GameObject canvas;
    public enum EmojiHeld {none, lust, happy, sad, angry }
    public EmojiHeld emojiHeld = EmojiHeld.none;

    [SerializeField] private float hand_speed = 1;

    //to be set by the overall game manager
    public Camera cam;

    public GameObject emojiSticker;
    private float journeyLength = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //journeyLength = Vector3.Distance(transform.position, Input.mousePosition); 
        // Distance moved equals elapsed time times speed..
        //float distCovered = (Time.time - 1) * hand_speed;

        // Fraction of journey completed equals current distance divided by total distance.
       // float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        //transform.position = Vector3.Lerp(transform.position, Input.mousePosition, fractionOfJourney);
        transform.position = Input.mousePosition;
        if(emojiSticker != null)
        {
            emojiSticker.transform.position = transform.position;
        }
    }

    private void PickupEmoji(GameObject emoji)
    {
        emojiSticker = Instantiate(emoji);
        emojiSticker.transform.SetParent(canvas.transform);
        emojiSticker.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100);

        //emojiSticker.transform.SetParent(gameObject.transform, true);
        emojiSticker.transform.SetParent(canvas.transform);

    }

    public void PlaceEmoji(Transform buttonTransform)
    {
        if(emojiHeld == EmojiHeld.none)
        {
            return;
        }
        emojiSticker.transform.SetParent(buttonTransform, true);
        GetComponent<AudioSource>().Play();
        EmptyHand();
    }

    public void HoldHappy()
    {
        CheckIfAlreadyHolding();
        emojiHeld = EmojiHeld.happy;
        PickupEmoji(emojiHappyPrefab);
        //Debug.Log("Current Held Emoji sticker obj: " + emojiSticker.name);
    }
    public void HoldLust()
    {
        CheckIfAlreadyHolding();
        emojiHeld = EmojiHeld.lust;
        PickupEmoji(emojiLustPrefab);
        //Debug.Log("Current Held Emoji sticker obj: " + emojiSticker.name);
    }
    public void HoldSad()
    {
        CheckIfAlreadyHolding();
        emojiHeld = EmojiHeld.sad;
        PickupEmoji(emojiSadPrefab);
        //Debug.Log("Current Held Emoji sticker obj: " + emojiSticker.name);
    }
    public void HoldAngry()
    {
        CheckIfAlreadyHolding();
        emojiHeld = EmojiHeld.angry;
        PickupEmoji(emojiAngryPrefab);
        //Debug.Log("Current Held Emoji sticker obj: " + emojiSticker.name);
    }

    public void EmptyHand()
    {
        emojiSticker = null;
        emojiHeld = EmojiHeld.none;
    }

    private void CheckIfAlreadyHolding()
    {
        if(emojiSticker != null)
        {
            Destroy(emojiSticker);
            emojiSticker = null;
        }
    }
}
