using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePassed : MonoBehaviour
{
    public TextMeshProUGUI time, wave;
    void Start()
    {
        time.text = GameManager.main.time.ToTimeString();
        wave.text = $"Wave {GameManager.main.wave}";
    }

    void Update()
    {
        time.text = GameManager.main.time.ToTimeString();
        wave.text = $"Wave {GameManager.main.wave}";
    }
}
