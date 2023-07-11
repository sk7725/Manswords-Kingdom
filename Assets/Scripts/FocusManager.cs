using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FocusManager : MonoBehaviour {
    public bool IsFocusing => GameManager.main.control.State == ControlPoint.ControlState.Focus;
    public float FocusHeat => focusHeat;
    public float Mana => mana;
    public float ManaFraction => mana / maxMana;

    public float LastManaFraction => lastMana / maxMana;

    [SerializeField] private Light2D globalLight, characterLight;
    [SerializeField] private float focusLightIntensity = 0.3f, focusCharacterIntensity = 0.85f;

    [SerializeField] private float focusLerpTime = 1f;
    [SerializeField] private float focusTimeScale = 0.2f;
    [SerializeField] private float focusingGainReduction = 0.5f;

    public float maxMana = 100f;
    public float manaUse = 20f;

    private float focusHeat = 0f;
    private float mana = 0f;
    private float lastMana = 0f;

    private void Start() {
        mana = lastMana = 0f;
        focusHeat = 0f;
    }

    private void Update() {
        if (!IsFocusing) lastMana = mana;
        else {
            mana -= Time.unscaledDeltaTime * manaUse;
        }

        focusHeat = Mathf.Lerp(focusHeat, IsFocusing ? 1f : 0f, focusLerpTime * Time.unscaledDeltaTime);
        if (focusHeat < 0.001f && !IsFocusing) focusHeat = 0;
        else if (focusHeat > 0.999f && IsFocusing) focusHeat = 1;

        globalLight.intensity = Mathf.Lerp(1, focusLightIntensity, focusHeat);
        characterLight.intensity = Mathf.Lerp(1, focusCharacterIntensity, focusHeat);
        Time.timeScale = Mathf.Lerp(1, focusTimeScale, focusHeat);
        Physics2D.simulationMode = focusHeat > 0.3f ? SimulationMode2D.Update : SimulationMode2D.FixedUpdate;
    }

    public bool CanFocus() {
        return mana > 0f;
    }

    public void AddMana(float amount) {
        if (IsFocusing) amount *= focusingGainReduction;
        mana += amount;
        if (mana > maxMana) mana = maxMana;
        if (IsFocusing && mana > lastMana) mana = lastMana;
    }
}