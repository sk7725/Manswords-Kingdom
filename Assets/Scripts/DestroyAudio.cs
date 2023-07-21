using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudio : MonoBehaviour {
    [SerializeField] private AudioSource source;

    void Update() {
        if (!source.isPlaying) Destroy(source.gameObject);
    }
}
