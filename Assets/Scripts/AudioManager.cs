using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private List<AudioSource> audioSources;
    [SerializeField] private bool[] startingTracks;
    [SerializeField] private AudioClip[] clips;
    public float baseClipVolume = 1.0f;
    public float audioFadeSpeed = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        InitializeAudioSources();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeAudioSources()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < startingTracks.Length; i++)
        {
            audioSources.Add(gameObject.AddComponent(typeof(AudioSource)) as AudioSource);
            //for each audio source set volume to their base volume if enabled, otherwise silent at 0
            audioSources[i].clip = clips[i];
            audioSources[i].volume = startingTracks[i] ? baseClipVolume : 0;
            audioSources[i].loop = true;
            audioSources[i].enabled = true;
            audioSources[i].loop = true;
            audioSources[i].Play();
        }
    }

    public void UpdateMusic(bool[] tracks)
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            if (tracks[i]) {
                StartCoroutine(FadeAudioIO(i, baseClipVolume));
            }
            else
            {
                StartCoroutine(FadeAudioIO(i, 0));
            }
        }
    }

    IEnumerator FadeAudioIO(int i, float targetVol)
    {
        //if target is less than current volume
        while (audioSources[i].volume > targetVol)
        {
            audioSources[i].volume -= Time.deltaTime * audioFadeSpeed;
            if (audioSources[i].volume < targetVol)
            {
                audioSources[i].volume = targetVol;
            }
            yield return null;
        }
        //if target is greater than current volume
        while (audioSources[i].volume > targetVol)
        {
            audioSources[i].volume += Time.deltaTime * audioFadeSpeed;
            if (audioSources[i].volume > targetVol)
            {
                audioSources[i].volume = targetVol;
            }
            yield return null;
        }
    }
}
