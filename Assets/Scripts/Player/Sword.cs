using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    public Rigidbody2D rigid;
    public SwordRenderer render;
    public Spawner spawner;
    public FocusSkill skill;

    public float manaGain = 2f;

    private void Start() {
        skill.Init();
    }

    public void SetSkill(FocusSkill skill) {
        this.skill = skill;
        skill.Init();
    }

    public void Hit() {
        GameManager.main.focus.AddMana(manaGain);
        GameManager.main.focus.RegisterCombo();
    }

    public void StartManaCombo() {
        skill.BeforeSkill();
    }

    public void EndManaCombo(int combo) {
        Debug.Log($"Combos: {combo}");
        if(combo > 0) {
            skill.UseSkill(combo);
        }
    }
}
