using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour {
    [SerializeField] private Rigidbody2D rigid;

    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float knockback = 5f;

    [Header("Fx")]
    [SerializeField] private GameObject spawnFx;
    [SerializeField] private GameObject despawnFx;
    [SerializeField] private GameObject hitSwordFx;

    [Header("Events")]
    [SerializeField] private ProjectileDespawnEvent onDespawn = new();

    private float time = 0f;

    [System.Serializable]
    public class ProjectileDespawnEvent: UnityEvent { }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            HitPlayer(GameManager.main.player);
        }
        else if (collision.gameObject.CompareTag("Sword")) {
            HitSword(GameManager.main.control.sword);
        }
        else if (collision.gameObject.CompareTag("Ground")) {
            HitGround();
        }
    }

    protected virtual void Start() {
        rigid.velocity = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed, Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed);
        gameObject.PlayFx(spawnFx);
        time = 0f;
    }

    protected virtual void Update() {
        time += Time.deltaTime;
        if (time > lifetime) Despawn();
    }

    public virtual void HitPlayer(Player player) {
        player.Damage(damage);
        player.rigid.velocity = rigid.velocity.normalized * knockback;
        Despawn();
    }

    public virtual void HitSword(Sword sword) {
        sword.Hit();
        gameObject.PlayFx(hitSwordFx);
        Despawn();
    }

    public virtual void HitGround() {
        Despawn();
    }

    public virtual void Despawn() {
        onDespawn.Invoke();
        gameObject.PlayFx(despawnFx);
        Destroy(gameObject);
    }
}
