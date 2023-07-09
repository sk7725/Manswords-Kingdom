using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] music;

    public int lastPlayed = 999;

    void Update()
    {
        if (!source.isPlaying) {
            int i = 0;
            if(lastPlayed < 0) {
                i = Random.Range(0, music.Length);
            }
            else {
                i = Random.Range(0, music.Length - 1);
                if (i >= lastPlayed) i++;
            }
            source.clip = music[i];
            lastPlayed = i;
            source.Play();
        }
    }
}
