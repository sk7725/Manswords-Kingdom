using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusBar : MonoBehaviour {
    [SerializeField] private Image focusBar, lastFocusBar;

    private float lastFocusFraction = 0f;

    private void Start() {
        lastFocusFraction = GameManager.main.focus.LastManaFraction;
    }

    void Update() {
        float f = GameManager.main.focus.LastManaFraction;
        if(f >= lastFocusFraction || GameManager.main.focus.IsFocusing) lastFocusFraction = f;
        else lastFocusFraction = Mathf.Lerp(lastFocusFraction, f, Time.deltaTime * 6f);

        focusBar.fillAmount = GameManager.main.focus.ManaFraction;
        lastFocusBar.fillAmount = lastFocusFraction;
    }
}
