using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAudio : MonoBehaviour
{

    private AudioSource _audio;

    [SerializeField] private AudioClip audio_punch;
    [SerializeField] private AudioClip audio_kick;
    
    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }

    public void PlayAudio(string action)
    {
        switch (action)
        {
            case "punch":
                _audio.PlayOneShot(audio_punch);
                break;
            
            case "kick":
                _audio.PlayOneShot(audio_kick);
                break;
        }
    }
}
