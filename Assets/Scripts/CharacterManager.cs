using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> npcPrefabs;

    private NPC currentNpcScript;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play()
    {
        NextNPC();
    }

    IEnumerator NextNPC()
    {

        yield return new WaitForSeconds(10);
        NextNPC();
    }


}
