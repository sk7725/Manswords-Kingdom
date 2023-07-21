using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager main;

    [SerializeField] private AudioSource globalSource;

    private void Awake() {
        main = this;
    }

    //todo port princesssavior
}
