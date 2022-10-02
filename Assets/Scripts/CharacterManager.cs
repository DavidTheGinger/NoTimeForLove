using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] private AudioClip[] clips;
    private bool[] tracks;

    // Start is called before the first frame update
    void Start()
    {
        
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
    
    
    private void PlayMusic()
    {
        for(int i = 0; i < tracks.Length; i++)
        {
            if(tracks[i]) { source.PlayOneShot(clips[i]); }
        }
    }

}
