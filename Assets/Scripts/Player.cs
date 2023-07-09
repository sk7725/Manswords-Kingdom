using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Settings")]
    public Rigidbody2D rigid;
    public int maxHealth = 100;

    private int hp = 100;
    public int Health => hp;

    private void Start() {
        hp = maxHealth;
    }

    public void Damage(int damage) {
        //todo
        hp -= damage;
        if(hp <= 0) {
            Kill();
        }
    }

    public void Heal(int heal) {
        //todo
        hp += heal;
        if(hp > maxHealth) hp = maxHealth;
    }

    public void Kill() {
        hp = 0;
        //todo
    }
}
