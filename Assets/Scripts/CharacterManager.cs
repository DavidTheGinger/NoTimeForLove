using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> npcPrefabs;

    [SerializeField] private NPC currentNpcScript;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager.UpdateMusic(currentNpcScript.tracks);
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
