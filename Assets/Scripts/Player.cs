using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Settings")]
    public Rigidbody2D rigid;
    public int maxHealth = 100;
    public float minimumDamageThreshold2 = 1f;
    public float velocityDamageScale = 5f;
    public float velocityDamageStart = 15f;
    public float velocityDamageMax = 30f;

    private int hp = 100;
    public int Health => hp;

    private void Start() {
        hp = maxHealth;
    }

    private void Update() {
        //Debug.Log($"VEL: {rigid.velocity.sqrMagnitude} DMG: {GetVelocityDamage()}");
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

    public float GetVelocityDamage() {
        float v = rigid.velocity.sqrMagnitude;
        if (v < minimumDamageThreshold2) {
            return 0;
        }
        float d = velocityDamageStart + Mathf.Sqrt(v - minimumDamageThreshold2) * velocityDamageScale;
        return Mathf.Min(d, velocityDamageMax);
    }
}
