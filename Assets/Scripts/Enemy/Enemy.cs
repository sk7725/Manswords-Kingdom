using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {
    public Rigidbody2D rigid;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    public float maxHealth = 30f;
    [SerializeField] private float knockback = 4f;
    [SerializeField] private float selfKnockback = 0.2f;
    [SerializeField] private float selfResistance = 0.9f;

    [Header("Animation")]
    [SerializeField] private float despawnTime = 0f;
    [SerializeField] private string deathAnimation = "", attackAnimation = "";

    [Header("Fx")]
    [SerializeField] private GameObject destroyFx;
    [SerializeField] private GameObject hitFx;

    [Header("Events")]
    public EnemyDeathEvent onDeath = new();

    [System.Serializable]
    public class EnemyDeathEvent : UnityEvent { }

    private float health = 30f;
    private bool dead = false;
    public Vector2 vel;
    public Vector2 move;
    private Color _blackColor;

    public bool Alive { get { return !dead; } }

    protected virtual void Start() {
        health = maxHealth;
        dead = false;
        col.enabled = true;
        vel = move = Vector2.zero;
        _blackColor = sprite.sharedMaterial.color;
    }

    private void Update() {
        if (!dead) {
            UpdateBehavior();
        }
    }

    private void FixedUpdate() {
        vel *= selfResistance;
        rigid.MovePosition(transform.position + (Vector3)vel + (Vector3)move);
        move = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (dead) return;
        if (other.gameObject.CompareTag("Player")) {
            HitPlayer(GameManager.main.player);
        }
    }

    protected virtual void UpdateBehavior() {

    }

    protected virtual void HitPlayer(Player player) {
        //Debug.Log(player.GetVelocityDamage());
        float dmg = player.GetVelocityDamage();
        if(dmg < 1) {
            //knock the player away
            player.rigid.velocity = (player.transform.position - transform.position).normalized * knockback;
        }
        else {
            ProcessDamage(dmg, player.rigid.velocity.normalized);
        }
    }

    public virtual void Damage(float dmg) {
        ProcessDamage(dmg, Vector2.zero);
    }

    public virtual void Damage(float dmg, AllyProjectile source) {
        ProcessDamage(dmg, source.rigid.velocity.normalized);
    }

    protected virtual void ProcessDamage(float damage, Vector2 knockbackDirection) {
        health -= damage;
        //take knockback
        if(vel.sqrMagnitude > 0.01f) vel = knockbackDirection * selfKnockback;
        gameObject.PlayFx(hitFx);
        if (health <= 0) Kill();
        else {
            StopAllCoroutines();
            StartCoroutine(IFlash(Color.white));
        }
    }

    public virtual void OnAttack() {
        PlayAnimation(attackAnimation);
    }

    public void PlayAnimation(string name) {
        if(string.IsNullOrEmpty(name)) return;
        animator.SetTrigger(name);
    }

    public virtual void Kill() {
        if (dead) return;
        StopAllCoroutines();
        sprite.material.color = _blackColor;
        dead = true;
        health = 0;
        col.enabled = false;
        PlayAnimation(deathAnimation);
        
        if(despawnTime > 0.1f) {
            StartCoroutine(IDeath());
        }
        else {
            Despawn();
        }
    }

    protected virtual void Despawn() {
        gameObject.PlayFx(destroyFx);
        Destroy(gameObject);
    }

    private WaitForSeconds _waiter = new WaitForSeconds(0.1f);
    IEnumerator IFlash(Color c) {
        for (int i = 0; i < 3; i++) {
            sprite.material.color = c;
            yield return _waiter;
            sprite.material.color = _blackColor;
            yield return _waiter;
        }
    }

    IEnumerator IDeath() {
        yield return new WaitForSeconds(despawnTime);
        Despawn();
    }
}
