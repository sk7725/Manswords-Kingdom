using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Settings")]
    public Rigidbody2D rigid;
    public PlayerRenderer render;

    public int maxHealth = 100;
    public float minimumDamageThreshold2 = 1f;
    public float velocityDamageScale = 5f;
    public float velocityDamageStart = 15f;
    public float velocityDamageMax = 30f;
    public float invincibilityTime = 0.5f;

    [Header("Fx")]
    [SerializeField] private GameObject damageFx;
    [SerializeField] private GameObject healFx;
    [SerializeField] private GameObject deathFx;

    private int hp = 100;
    private bool dead = false;
    private float invincibility = 0f;

    public int Health => hp;
    public float HealthFraction {
        get { return hp / (float)maxHealth; }
    }

    private void Start() {
        hp = maxHealth;
        dead = false;
        invincibility = 0f;
    }

    private void Update() {
        if(invincibility > 0) {
            invincibility -= Time.unscaledDeltaTime;
        }
    }

    public void Damage(int damage, Vector3? point = null) {
        if (dead || invincibility > 0) return;
        hp -= damage;
        invincibility = invincibilityTime;

        if (point.HasValue) {
            Fx.Play(damageFx, point.Value);
        }
        else {
            Fx.Play(damageFx, transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 0));
        }

        if(hp <= 0) {
            Kill();
        }
        else {
            render.Hurt();
        }
    }

    public void Heal(int heal) {
        if (dead) return;
        hp += heal;
        if(hp > maxHealth) hp = maxHealth;
        render.SetFaceFor(4, 1.5f);
        gameObject.PlayFx(healFx);
    }

    public void Kill() {
        if(dead) return;
        hp = 0;
        dead = true;
        render.gameObject.SetActive(false);
        Fx.Play(deathFx, transform.position);
        GameManager.main.GameOver();
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
