using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BubbleBehavior : NPC
{

    [SerializeField] private string dialogue_string;

    [SerializeField] private TextMeshProUGUI bubble_dialogue;
    [SerializeField] private Image image_displayed;
    [SerializeField] private int sprite_index = 0;
    [SerializeField] private Collider2D text_collider;
    [SerializeField] private GameObject target;

    private float journeyLength;

    // Start is called before the first frame update
    void Start()
    {
        image_displayed.alphaHitTestMinimumThreshold = 0.5f;
        dialogue_string = bubble_dialogue.text;
        bubble_dialogue.text = "";
        sprite_index = Random.Range(0, bubble_sprites.Count - 1);


        DisplayText();
        StartCoroutine(SwitchSprite());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveTowardsTarget();
    }

    public void DisplayText()
    {
        StartCoroutine(TypeDialogue(dialogue_string));
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
        transform.localScale = Vector3.Lerp(Vector3.zero, 1.5f*Vector3.one, fractionOfJourney*2);
        // Debug.Log("distCovered: " + distCovered);
        // Debug.Log("fractionOfJourney: " + fractionOfJourney);
        // Debug.Log("JourneyLength: " + journeyLength);
    }


    IEnumerator TypeDialogue (string dialogue)
    {
        bubble_dialogue.text = "";
        foreach(char letter in dialogue.ToCharArray())
        {
            
            bubble_dialogue.text += letter;
            yield return new WaitForSeconds(TextDelaySeconds);
        }
    }

    IEnumerator SwitchSprite()
    {
        while (true)
        {
            sprite_index = sprite_index < bubble_sprites.Count - 1 ? sprite_index + 1 : 0;
            image_displayed.sprite = bubble_sprites[sprite_index];
            yield return new WaitForSeconds(SpriteDelay * TextDelaySeconds);
        }
        

    }
}
