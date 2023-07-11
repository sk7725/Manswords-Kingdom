using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {
    [SerializeField] private Image hpBar, lastHPBar;

    private float lastHealth = 0f;

    private void Start() {
        lastHealth = GameManager.main.player.HealthFraction;
    }

    void Update() {
        float f = GameManager.main.player.HealthFraction;
        if (f >= lastHealth) lastHealth = f;
        else lastHealth = Mathf.Lerp(lastHealth, f, Time.deltaTime * 6f);

        hpBar.fillAmount = f;
        lastHPBar.fillAmount = lastHealth;
    }
}
