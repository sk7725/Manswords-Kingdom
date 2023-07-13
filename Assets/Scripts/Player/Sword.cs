using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    public Rigidbody2D rigid;
    public SwordRenderer render;

    public float manaGain = 2f;

    public void Hit() {
        GameManager.main.focus.AddMana(manaGain);
        GameManager.main.focus.RegisterCombo();
    }

    public void EndManaCombo(int combo) {
        Debug.Log($"Combos: {combo}");
        //todo
    }
}