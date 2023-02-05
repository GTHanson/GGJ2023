using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicFade : MonoBehaviour
{
    [SerializeField]
    private AudioClip trainMusic;
    [SerializeField]
    private AudioClip cannonMusic;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void ToggleMusic(bool swap)
    {
        source.Stop();
        if (swap)
        {
            source.clip = trainMusic;
            source.time = Time.timeSinceLevelLoad % trainMusic.length;
            source.Play();
        }
        else
        {
            source.clip = cannonMusic;
            source.time = Time.timeSinceLevelLoad % cannonMusic.length;
        }
        source.Play();
    }
}
