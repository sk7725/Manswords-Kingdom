using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {
    [SerializeField] private Image image;

    void Update() {
        image.fillAmount = GameManager.main.player.HealthFraction;
    }
}
